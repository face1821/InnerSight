using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatState : PlayerGroundedState
{
    public PlayerSquatState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.EnterSquatCollider();
        player.spriterd.sprite = player.squatStateImg;
    }

    public override void Exit()
    {
        player.ExitSquatCollider();
        player.spriterd.sprite = player.originImg;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.currentState != this)
            return;
        player.SetVelocity(xInput * player.moveSpeed * 0.7f, rb.velocity.y);
        if(yInput >= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

}
