using UnityEngine;

public class transmitBall : MonoBehaviour
{
    [SerializeField] private Animator am;

    [SerializeField] private LayerMask whatIsAGround;
    [SerializeField] private LayerMask whatIsCGround;

    [Tooltip("碰到该图层上的碰撞体时立即停止")]
    [SerializeField] private LayerMask whatIsBGround;
    [SerializeField] private LayerMask whatIsFakeCGround;

    [Tooltip("用于 CircleCast 的半径；若有 CircleCollider2D 则优先用其半径")]
    [SerializeField] private float ballCastRadius = 0.12f;

    [Tooltip("命中后沿运动方向少移一点，避免嵌进碰撞体")]
    [SerializeField] private float hitSkin = 0.02f;

    [Header("抛出速度（标量，单位/秒）")]
    [SerializeField] private float launchSpeedNormal;
    [SerializeField] private float launchSpeedCharged;

    [Header("匀减速（标量，单位/秒²）")]
    [SerializeField] private float deceleration = 10f;

    public bool isMain;  //注意：所有用到这个值的地方都是进行的特殊处理，思考逻辑时需要仔细阅读代码
    public Player mianPlayer;
    public Player notmianPlayer;


    public bool isFlying;
    private Vector2 flyDirection;
    private float currentSpeed;
    private Player owner;
    private CircleCollider2D circleCollider2D;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        SoundManager.instance.Play(1, "ThrowDaoju", false);
        SoundManager.instance.Play(3, "Soul", true);
        // am.SetBool("",true);
    }
    
    public void SetOwner(Player player)
    {
        owner = player;
    }

    private void OnDestroy()
    {
        if (owner != null)
            owner.ClearActiveTransmitBallReference(this);
        SoundManager.instance.Stop(3, "Soul");
    }

    private float GetCastRadius()
    {
        if (circleCollider2D == null)
            return ballCastRadius;
        float s = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y));
        return circleCollider2D.radius * s;
    }

    public void Launch(TeleportAimDirection dir, bool isCharged)
    {
        Vector2 d = DirectionToVector2(dir);
        if (d.sqrMagnitude < 0.0001f)
            return;

        flyDirection = d.normalized;
        currentSpeed = isCharged ? launchSpeedCharged : launchSpeedNormal;
        isFlying = true;
    }

    private void Update()
    {
        if (!isFlying)
            return;

        if(IsInGroundFun())
            owner.isBallInGround = true;
        else
            owner.isBallInGround = false;

        // 副球：主球已结束飞行（撞墙/减速停等）时，本球也立刻停下
        if (!isMain && mianPlayer != null)
        {
            transmitBall mainBall = mianPlayer.activeTransmitBall;
            if (mainBall != null && mainBall != this && !mainBall.isFlying)
            {
                StopFlying();
                return;
            }
        }

        currentSpeed = Mathf.Max(0f, currentSpeed - deceleration * Time.deltaTime);
        if (currentSpeed <= 0f)
        {
            StopFlying();
            return;
        }

        Vector2 start = transform.position;
        float step = currentSpeed * Time.deltaTime;
        float radius = GetCastRadius();

        // RaycastHit2D hit = Physics2D.CircleCast(start, radius, flyDirection, step, whatIsGround);
        // if (hit.collider != null)
        // {
        //     float travel = Mathf.Max(0f, hit.distance - hitSkin);
        //     transform.position = start + flyDirection * travel;
        //     StopFlying();
        //     return;
        // }

        // transform.position = start + flyDirection * step;

        LayerMask castMask = whatIsBGround | whatIsFakeCGround;
        RaycastHit2D hit = Physics2D.CircleCast(start, radius, flyDirection, step, castMask);
        if (hit.collider != null)
        {
            int layer = hit.collider.gameObject.layer;
            // 先命中假地板：只销毁，球不刹停，本帧继续走完 step（穿过刚拆掉的一块）
            if (((1 << layer) & whatIsFakeCGround) != 0)
            {
                Destroy(hit.collider.gameObject); // 若碰撞体在子物体上、要删整坨假地板，可改成 Destroy(hit.collider.transform.root.gameObject)
                transform.position = start + flyDirection * step;
                return;
            }
            // 普通地面：保持你原来的刹停逻辑
            if (((1 << layer) & whatIsBGround) != 0)
            {
                float travel = Mathf.Max(0f, hit.distance - hitSkin);
                transform.position = start + flyDirection * travel;
                StopFlying();
                SoundManager.instance.Play(5, "DaojuImpact", false);
                return;
            }
        }
        transform.position = start + flyDirection * step;
    }

    private void StopFlying()
    {
        Debug.Log($"{owner.gameObject.name} 停下飞行");
        if (!isFlying)
            return;
        isFlying = false;
        currentSpeed = 0f;
        OnStopInAir();
    }

    private static Vector2 DirectionToVector2(TeleportAimDirection dir)
    {
        switch (dir)
        {
            case TeleportAimDirection.Up: return Vector2.up;
            case TeleportAimDirection.Down: return Vector2.down;
            case TeleportAimDirection.Right: return Vector2.right;
            case TeleportAimDirection.Left: return Vector2.left;
            default: return Vector2.zero;
        }
    }

    private void OnStopInAir()
    {
        // 停住后：通知玩家可传送、播放特效等
    }

    /// <summary>
    /// 检测物体中心（transform.position）是否与 A/B/C 型地面图层上的碰撞体重叠。
    /// 未碰到上述图层返回 false，碰到任一返回 true。
    /// </summary>
    public bool IsInGroundFun()
    {
        Vector2 center = transform.position;
        LayerMask mask = whatIsAGround | whatIsBGround | whatIsCGround;
        Collider2D hit = Physics2D.OverlapPoint(center, mask);
        return hit != null;
    }
}