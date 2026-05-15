using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        player.IsAlreadyJumped = false;
    }

    public override void Exit() { base.Exit(); }

    public override void Update()
    {
        base.Update();

        if (xInput == 0 && player.IsGroundDetected())
        {
            player.SetVelocity(0, rb.velocity.y); //防止滑行
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.downState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.isMain)
        {
            //DebugInfo.text += $"{player.gameObject.name}: ";

            //if (player.isMain == false)
            //    DebugInfo.text += "(isMain: false) ";

            //DebugInfo.text += $"{player.GetCoyoteTimerData()} ";

            // if(player.isMain == false)
            //     return;

            //DebugInfo.text += "Jump\n";

            // 地面跳（或极短离地时）都允许
            // player.ConsumeCoyoteJump();
            stateMachine.ChangeState(player.jumpState);
            player.notmainPlayer.stateMachine.ChangeState(player.notmainPlayer.jumpState);
            return;
        }

        if (yInput < 0 && stateMachine.currentState != player.squatState)
        {
            stateMachine.ChangeState(player.squatState);
        }
    }
}