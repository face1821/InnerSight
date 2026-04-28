using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //设置y轴速度，实现跳跃效果
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(rb.velocity.y < 0)
            stateMachine.ChangeState(player.downState);  //若y轴速度为负，则转换为下落状态
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);  //若玩家在空中有x轴输入则也可以慢速移动
        if(player.isMain == false && player.mainPlayer.IsWallDetected())
            player.SetVelocity(0, rb.velocity.y);
    }
}

