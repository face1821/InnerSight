using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum TeleportAimDirection
{
    None,
    Up, // arrows[0]
    Right, // arrows[1]
    Down, // arrows[2]
    Left // arrows[3]
}

public class Player : Entity
{
    public string CurrentStateString;

    public bool isBusy { get; private set; } //玩家此时是否忙碌，如果忙碌则不让切换状态
    [Header("移动")]
    public float moveSpeed = 8f; //移动速冻
    public float jumpForce = 12f; //跳跃力度
    [SerializeField] private float coyoteTime = 0.1f; // 土狼跳窗口
    public float coyoteTimer;

    [SerializeField] private CapsuleCollider2D bodyCollider;
    private Vector2 originSize; //碰撞体原始大小
    private Vector2 originOffset; //碰撞体缩小后的偏移量

    public bool isMain; //注意：所有用到这个值的地方都是进行的特殊处理，思考逻辑时需要仔细阅读代码
    public Player mainPlayer;
    public Player notmainPlayer;

    public bool isBallInGround;

    [Header("传送球瞄准箭头")]
    [SerializeField] private List<GameObject> arrows;
    [SerializeField] [Range(0.05f, 1f)] private float aimArrowDimAlpha = 0.25f;
    [SerializeField] private Camera aimCamera; // 不拖则用 Camera.main
    [SerializeField] private GameObject transmitBallPrefab;
    private SpriteRenderer[] aimArrowRenderers;
    public SpriteRenderer spriterd;
    public Sprite squatStateImg;
    public Sprite originImg; //没有蹲下的序列帧动画时，暂时用这个图片替代
    public TeleportAimDirection CurrentAimDirection { get; private set; } = TeleportAimDirection.None;
    /// <summary>按住左键期间最后计算出的瞄准方向；松手发射时读取。</summary>
    private TeleportAimDirection lastAimWhileHolding = TeleportAimDirection.None;
    /// <summary>当前场上由本玩家发射的传送球；非空时不允许再发射。</summary>
    public transmitBall activeTransmitBall;

    /// <summary>空格传送到球之后为 true，直到 IsGroundDetected() 再次为 true 才允许发射下一颗。</summary>
    public bool transmitBallLockedUntilGrounded = false;

    public bool isPlaySilentWalk = false;

    private const float RandomVocalInterval = 2f;
    private float randomVocalTimer;
    private static readonly string[] RandomVocalClipNames = { "Vocal_1", "Vocal_2", "Vocal_3", "Vocal_4", "Vocal_5" };

    public bool isDead = false;

    public bool IsAlreadyJumped;

    public float ShowAlpha
    {
        get { return spriterd.color.a; }
        set
        {
            spriterd.color = new Color(spriterd.color.r, spriterd.color.g, spriterd.color.b, value);
            foreach (var item in aimArrowRenderers)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, value);
            }
        }
    }


    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDownState downState { get; private set; }
    public PlayerSquatState squatState { get; private set; }
    public PlayerDeadState deadState { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        spriterd = GetComponentInChildren<SpriteRenderer>();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        downState = new PlayerDownState(this, stateMachine, "Down");
        squatState = new PlayerSquatState(this, stateMachine, "Squat");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");

        if (arrows != null && arrows.Count >= 4)
        {
            aimArrowRenderers = new SpriteRenderer[4];
            for (int i = 0; i < 4; i++)
                aimArrowRenderers[i] = arrows[i] != null ? arrows[i].GetComponent<SpriteRenderer>() : null;
        }
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); //初始化状态机

        originSize = bodyCollider.size;
        originOffset = bodyCollider.offset;
        originImg = spriterd.sprite;

        //第一关初始状态为一直显影
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            ShowAlpha = 1;
        }
        else if (isMain)
        {
            KeepShow(3);
        }
    }

    protected override void Update()
    {
        if (isDead)
            return;
        base.Update();
        UpdateCoyoteTimer();
        stateMachine.currentState.Update();
        UpdateTeleportAimArrows();
        UpdateTransmitBallInput();
        PlaySound();
    }

    public void KeepShow(int loopCount)
    {
        DOTween.Kill(mainPlayer);

        mainPlayer.ShowAlpha = 0;
        DOTween.To(() => mainPlayer.ShowAlpha,
                x => mainPlayer.ShowAlpha = x,
                1f,
                0.5f
            ).SetLoops(loopCount * 2, LoopType.Yoyo)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                mainPlayer.ShowAlpha = 0;
                SetTeleportAimArrowsVisible(false);
            })
            .SetTarget(mainPlayer);
    }

    private void PlaySound()
    {
        if (!isMain) //注意：只有玩家B能播放音效
            return;
        if (SoundManager.Instance == null)
            return;

        randomVocalTimer += Time.deltaTime;
        if (randomVocalTimer < RandomVocalInterval)
            return;

        randomVocalTimer = 0f;
        string clipName = RandomVocalClipNames[Random.Range(0, RandomVocalClipNames.Length)];
        SoundManager.Instance.Play(4, clipName, false);
    }

    private void UpdateCoyoteTimer()
    {
        if (IsGroundDetected())
            coyoteTimer = coyoteTime; // 在地面时重置窗口
        else
            coyoteTimer -= Time.deltaTime; // 离地后倒计时
    }

    public bool CanUseCoyoteJump() { return coyoteTimer > 0f; }

    public float GetCoyoteTimerData() => coyoteTimer;

    public void ConsumeCoyoteJump() { coyoteTimer = 0f; }

    public void EnterSquatCollider()
    {
        bodyCollider.size = new Vector2(originSize.x, originSize.y * 0.5f);
        // 为了让脚底尽量贴地，中心也下移一部分（可按实际微调）
        float delta = (originSize.y - bodyCollider.size.y) * 0.5f;
        bodyCollider.offset = new Vector2(originOffset.x, originOffset.y - delta);
    }

    public void ExitSquatCollider()
    {
        bodyCollider.size = originSize;
        bodyCollider.offset = originOffset;
    }

    //更新传送瞄准箭头
    private void UpdateTeleportAimArrows()
    {
        if (GameManager.Instance.CantThrow)
            return;
        if (aimArrowRenderers == null || aimArrowRenderers.Length < 4)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            TrySpawnTransmitBallOnMouseUp();
            lastAimWhileHolding = TeleportAimDirection.None;
        }

        if (!Input.GetMouseButton(0))
        {
            SetTeleportAimArrowsVisible(false);
            CurrentAimDirection = TeleportAimDirection.None;
            return;
        }

        //点击UGUI时，返回
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 上面 !GetMouseButton(0) 已 return 掉「没按住」的情况
        if (Input.GetMouseButtonDown(0) && stateMachine.currentState == squatState && activeTransmitBall == null)
        {
            SoundManager.Instance.Play(1, "BuildUp", false);
        }

        Camera cam = aimCamera != null ? aimCamera : Camera.main;
        if (cam == null)
            return;

        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = transform.position.z;

        // //进行特殊处理
        // Vector2 aimReference = rb.position;
        // if (isMain && notmainPlayer != null && notmainPlayer.rb != null)
        //     aimReference = notmainPlayer.rb.position;
        // Vector2 delta = (Vector2)mouse - aimReference;

        // Vector2 delta = (Vector2)mouse - Vector2.zero;
        GameManager ins = GameManager.Instance;
        Vector2 xxx = ins.AllWorldCenter[ins.CurrentLevel - 1][ins.CurrentSamllLevel - 1];
        Vector2 delta = (Vector2)mouse - xxx;
        Debug.Log("xxxbbb" + xxx);
        // Vector2 delta = (Vector2)mouse - ins.WorldCenter1[ins.currentSamllLevel - 1];

        TeleportAimDirection dir;
        float ax = Mathf.Abs(delta.x);
        float ay = Mathf.Abs(delta.y);
        if (ay >= ax)
            dir = delta.y > 0f ? TeleportAimDirection.Up : TeleportAimDirection.Down;
        else
            dir = delta.x > 0f ? TeleportAimDirection.Right : TeleportAimDirection.Left;

        CurrentAimDirection = dir;
        lastAimWhileHolding = dir;
        SetTeleportAimArrowsVisible(true);

        for (int i = 0; i < 4; i++)
        {
            SpriteRenderer r = aimArrowRenderers[i];
            if (r == null) continue;

            int index = DirectionToArrowIndex(dir);
            Color c = r.color;
            c.a = (i == index) ? 1f : aimArrowDimAlpha;
            r.color = c;
        }
    }

    private static int DirectionToArrowIndex(TeleportAimDirection d)
    {
        switch (d)
        {
            case TeleportAimDirection.Up: return 0;
            case TeleportAimDirection.Right: return 1;
            case TeleportAimDirection.Down: return 2;
            case TeleportAimDirection.Left: return 3;
            default: return 0;
        }
    }

    //设置传送瞄准箭头可见
    private void SetTeleportAimArrowsVisible(bool visible)
    {
        if (mainPlayer.ShowAlpha == 0 && isMain && visible) return;
        if (arrows == null) return;
        for (int i = 0; i < arrows.Count && i < 4; i++)
        {
            if (arrows[i] != null)
                arrows[i].SetActive(visible);
        }
    }

    /// <summary>松手时：仅在当前为蹲下状态且朝向上方发射时为蓄力。</summary>
    private bool ShouldLaunchChargedTransmitBall(TeleportAimDirection fireDir)
    {
        return stateMachine != null
               && stateMachine.currentState == squatState
               && fireDir == TeleportAimDirection.Up;
    }

    //尝试在鼠标释放时生成传球球
    private void TrySpawnTransmitBallOnMouseUp()
    {
        if (lastAimWhileHolding == TeleportAimDirection.None || transmitBallPrefab == null)
            return;

        if (activeTransmitBall != null)
            return;

        if (transmitBallLockedUntilGrounded)
            return;

        Vector3 spawnPos = rb.position;
        GameObject ballObj = Instantiate(transmitBallPrefab, spawnPos, Quaternion.identity);
        transmitBall ball = ballObj.GetComponent<transmitBall>();

        if (ball == null)
        {
            Destroy(ballObj);
            return;
        }

        if (isMain)
        {
            ball.isMain = true;
            ball.notmianPlayer = notmainPlayer;
            ball.mianPlayer = this;
        }
        else
        {
            ball.isMain = false;
            ball.notmianPlayer = this;
            ball.mianPlayer = mainPlayer;
        }

        ball.SetOwner(this);
        activeTransmitBall = ball;

        bool charged = ShouldLaunchChargedTransmitBall(lastAimWhileHolding);
        GameManager ins = GameManager.Instance;
        if (charged) //用来控制音效
            ins.NowThrowState = 2;
        else
            ins.NowThrowState = 1;
        ball.Launch(lastAimWhileHolding, charged);
        
        //传送时重置土狼跳机制
        ConsumeCoyoteJump();
    }

    //更新传送球输入
    private void UpdateTransmitBallInput()
    {
        if (activeTransmitBall == null)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isBallInGround)
                return;
            if (!isMain && mainPlayer.isBallInGround)
                return;
            GameManager ins = GameManager.Instance;

            Vector2 target = activeTransmitBall.transform.position;
            rb.position = target;
            SetVelocity(0, 3);

            Destroy(activeTransmitBall.gameObject);
            activeTransmitBall = null;
            transmitBallLockedUntilGrounded = true;
            stateMachine.ChangeState(downState);
            if (ins.NowThrowState == 1)
                SoundManager.Instance.Play(1, "Transfer_1", false);
            else if (ins.NowThrowState == 2)
                SoundManager.Instance.Play(1, "Transfer_2", false);
            ins.NowThrowState = 0;
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(activeTransmitBall.gameObject);
            GameManager.Instance.NowThrowState = 0;
            activeTransmitBall = null;
        }
    }

    /// <summary>由传送球 OnDestroy 调用，避免球被其它方式销毁后无法再次发射。</summary>
    public void ClearActiveTransmitBallReference(transmitBall ball)
    {
        if (activeTransmitBall == ball)
            activeTransmitBall = null;
    }
}