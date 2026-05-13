using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        //设置速度，实现移动效果
        player.SetVelocity(xInput * player.moveSpeed , rb.velocity.y);
        if (xInput == 0)
            stateMachine.ChangeState(player.idleState);
        if(player.isMain == false && player.mainPlayer.IsWallDetected())
            player.SetVelocity(0, rb.velocity.y);
    }
}
