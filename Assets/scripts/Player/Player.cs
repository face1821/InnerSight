using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public bool isBusy { get; private set; }  //玩家此时是否忙碌，如果忙碌则不让切换状态
    [Header("移动")]
    public float moveSpeed = 8f;  //移动速冻
    public float jumpForce = 12f;  //跳跃力度
    [SerializeField] private float coyoteTime = 0.1f;   // 土狼跳窗口
    private float coyoteTimer;

    public bool isMain;
    public Player mainPlayer;  //注意：只有当isMain为false的时候才能调用此对象，否则会报空

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState{ get; private set; }
    public PlayerDownState downState{ get; private set; }


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        downState = new PlayerDownState(this, stateMachine, "Down");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);  //初始化状态机

    }

    protected override void Update()
    {
        base.Update();
        UpdateCoyoteTimer();
        stateMachine.currentState.Update(); 
    }

    private void UpdateCoyoteTimer()
    {
        if (IsGroundDetected())
            coyoteTimer = coyoteTime;     // 在地面时重置窗口
        else
            coyoteTimer -= Time.deltaTime; // 离地后倒计时
    }

    public bool CanUseCoyoteJump()
    {
        return coyoteTimer > 0f;
    }

    public void ConsumeCoyoteJump()
    {
        coyoteTimer = 0f;
    }
}
