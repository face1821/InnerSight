using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }

    [SerializeField] protected Transform groundCheck;  //检测地面的空物体
    [SerializeField] protected float groundCheckDistance;  //检测地面的线段长度
    [SerializeField] protected Transform wallCheck;  //检测墙壁的空物体
    [SerializeField] protected float wallCheckDistance;  //检测墙壁的线段长度
    [SerializeField] protected LayerMask whatIsGround;  //地面和墙壁的图层

    public int facingDir { get; private set; } = 1;  //面朝方向，1为右，-1为左，用于计算
    protected bool isFacingRight = true;  //是否面朝右边

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        
    }

    //设置角色速度
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    //设置角色x，y方向速度均为0
    public void SetZeroVelocity() => SetVelocity(0,0);
    
    //控制何时翻转角色
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (_x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    //翻转角色
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    //检测地面
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    //检测墙壁
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    //画出检测射线，方便调试
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

}
