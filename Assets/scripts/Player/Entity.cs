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
    [SerializeField] protected Transform SquatHeadCheck;
    [SerializeField] protected float SquatHeadCheckDistance;


    [SerializeField] protected LayerMask whatIsAGround;  //A型地面的图层
    [SerializeField] protected LayerMask whatIsBGround;  //地面和墙壁的图层 (B型地面)
    [SerializeField] protected LayerMask whatIsCGround;  //C型地面的图层

    [SerializeField] protected GameObject notFlipGameObject;  //不会被翻转的游戏对象

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
        Transform notFlipT = notFlipGameObject != null ? notFlipGameObject.transform : null;
    
        if (notFlipT != null)
            notFlipT.SetParent(null, true); // 保持世界位置、旋转、缩放
    
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    
        if (notFlipT != null)
            notFlipT.SetParent(transform, true); // 再挂回玩家根节点，仍保持世界变换
    }

    //检测地面
    public virtual bool IsGroundDetected()
    {
        bool isA = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsAGround);
        bool isB = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsBGround);
        bool isC = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsCGround);
        return isA || isB || isC ;
    } 
        

    //检测墙壁
    protected virtual bool IsWallUpDetected()
    {
        bool isA = Physics2D.Raycast(wallUpCheck.position, Vector2.right * facingDir, wallUpCheckDistance, whatIsAGround);
        bool isB = Physics2D.Raycast(wallUpCheck.position, Vector2.right * facingDir, wallUpCheckDistance, whatIsBGround);
        bool isC = Physics2D.Raycast(wallUpCheck.position, Vector2.right * facingDir, wallUpCheckDistance, whatIsCGround);
        return isA || isB || isC ;
    }
    protected virtual bool IsWallDownDetected()
    {
        bool isA = Physics2D.Raycast(wallDownCheck.position, Vector2.right * facingDir, wallDownCheckDistance, whatIsAGround);
        bool isB = Physics2D.Raycast(wallDownCheck.position, Vector2.right * facingDir, wallDownCheckDistance, whatIsBGround);
        bool isC = Physics2D.Raycast(wallDownCheck.position, Vector2.right * facingDir, wallDownCheckDistance, whatIsCGround);
        return isA || isB || isC ;

    }
    public virtual bool IsWallDetected() => IsWallUpDetected() || IsWallDownDetected();
    //检测天花板（正上方）
    public virtual bool IsHeadDetected()
    {
        bool isA = Physics2D.Raycast(headCheck.position, Vector2.up, headCheckDistance, whatIsAGround);
        bool isB = Physics2D.Raycast(headCheck.position, Vector2.up, headCheckDistance, whatIsBGround);
        bool isC = Physics2D.Raycast(headCheck.position, Vector2.up, headCheckDistance, whatIsCGround);
        return isA || isB || isC ;
    } 

    //检测蹲下时的天花板（正上方）
    public virtual bool IsSquatHeadDetected()
    {
        bool isA = Physics2D.Raycast(SquatHeadCheck.position, Vector2.up, SquatHeadCheckDistance, whatIsAGround);
        bool isB = Physics2D.Raycast(SquatHeadCheck.position, Vector2.up, SquatHeadCheckDistance, whatIsBGround);
        bool isC = Physics2D.Raycast(SquatHeadCheck.position, Vector2.up, SquatHeadCheckDistance, whatIsCGround);
        return isA || isB || isC ;
    } 

    //画出检测射线，方便调试
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallUpCheck.position, new Vector3(wallUpCheck.position.x + wallUpCheckDistance * facingDir, wallUpCheck.position.y));
        Gizmos.DrawLine(wallDownCheck.position, new Vector3(wallDownCheck.position.x + wallDownCheckDistance * facingDir, wallDownCheck.position.y));
        Gizmos.DrawLine(headCheck.position, new Vector3(headCheck.position.x, headCheck.position.y + headCheckDistance));
        Gizmos.DrawLine(SquatHeadCheck.position, new Vector3(SquatHeadCheck.position.x, SquatHeadCheck.position.y + SquatHeadCheckDistance));
    }

}
