using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;  //状态机
    protected Player player;  //操控角色对象

    protected Rigidbody2D rb;  //角色的刚体

    protected float xInput;
    protected float yInput;
    private string animBoolName;  //动画控制器中的变量名称

    protected float stateTimer;  //用于计时，一直减少

    protected bool triggerCalled;  //记录某些动画是否播完，播完了就设为true，否则为false


    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        //进入任何一个状态时

        player.anim.SetBool(animBoolName,true);  //设置动画播放 
        rb = player.rb;  //获取角色刚体
        triggerCalled = false;  //每次进入任何状态时设置这个值为假，有些状态会在动画关键帧中设置此值为真，然后判断若此值为真则退出状态
    }

    public virtual void Update() 
    {
        stateTimer -= Time.deltaTime;  //用于计时，时刻减少

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        // player.anim.SetFloat("yVelocity", rb.velocity.y);  //用于控制跳跃的动作混合树
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);  //设置动画结束
    }

    public virtual void AnimationFinsihTrigger()
    {
        //控制一个状态是否执行完
        triggerCalled = true;
    }
}
