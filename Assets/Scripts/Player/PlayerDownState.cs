using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownState : PlayerState
{
    public PlayerDownState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.transmitBallLockedUntilGrounded = false;
    }

    public override void Update()
    {
        base.Update();

        //此脚本的状态为下落状态
        if(player.IsGroundDetected())
        {
            //若接触到地面了则转换到idel状态
            stateMachine.ChangeState(player.idleState);
            return;
        }

        //如果主玩家可以使用土狼跳，则跳跃
        if (Input.GetKeyDown(KeyCode.Space) && !player.IsAlreadyJumped && player.isMain && player.CanUseCoyoteJump())
        {
            player.ConsumeCoyoteJump();
            stateMachine.ChangeState(player.jumpState);
            player.notmainPlayer.stateMachine.ChangeState(player.notmainPlayer.jumpState);
            return;
        }

        if(xInput != 0)
        {
            //若角色在下落过程中有x方向的属于，角色在空中也可以相对缓慢移动(0.8倍)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
        }

        if(player.isMain == false && player.mainPlayer.IsWallDetected())
            player.SetVelocity(0, rb.velocity.y);
    }
}
