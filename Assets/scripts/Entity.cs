using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallUpCheck;
    [SerializeField] protected float wallUpCheckDistance;
    [SerializeField] protected Transform wallDownCheck;
    [SerializeField] protected float wallDownCheckDistance;
    [SerializeField] protected Transform headCheck;
    [SerializeField] protected float headCheckDistance;

    [SerializeField] protected LayerMask whatIsGround;  //地面和墙壁的图层

    public int facingDir = 1;  //面朝方向，1为右，-1为左，用于计算
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
    protected virtual bool IsWallUpDetected() => Physics2D.Raycast(wallUpCheck.position, Vector2.right * facingDir, wallUpCheckDistance, whatIsGround);
    protected virtual bool IsWallDownDetected() => Physics2D.Raycast(wallDownCheck.position, Vector2.right * facingDir, wallDownCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => IsWallUpDetected() || IsWallDownDetected();
    //检测天花板（正上方）
    public virtual bool IsHeadDetected() => Physics2D.Raycast(headCheck.position, Vector2.up, headCheckDistance, whatIsGround);

    //画出检测射线，方便调试
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallUpCheck.position, new Vector3(wallUpCheck.position.x + wallUpCheckDistance * facingDir, wallUpCheck.position.y));
        Gizmos.DrawLine(wallDownCheck.position, new Vector3(wallDownCheck.position.x + wallDownCheckDistance * facingDir, wallDownCheck.position.y));
        Gizmos.DrawLine(headCheck.position, new Vector3(headCheck.position.x, headCheck.position.y + headCheckDistance));
    }

}
