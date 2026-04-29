using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput == 0 && player.IsGroundDetected())
        {
            player.SetVelocity(0, rb.velocity.y);  //防止滑行
        }
        if (Input.GetKeyDown(KeyCode.W) && player.CanUseCoyoteJump())
        {
            if(player.isMain == false && !player.mainPlayer.CanUseCoyoteJump())
                return;
            // 地面跳（或极短离地时）都允许
            player.ConsumeCoyoteJump();
            stateMachine.ChangeState(player.jumpState);
        }
        if (yInput < 0)
        {
            stateMachine.ChangeState(player.squatState);
        }
    }


}
