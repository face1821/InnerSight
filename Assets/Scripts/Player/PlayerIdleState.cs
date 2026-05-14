using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        //防止在光滑地面上由于惯性向前滑行
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Exit() { base.Exit(); }

    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            //若玩家有x轴输入则转换为移动状态
            stateMachine.ChangeState(player.moveState);
        }
        else if (player.mainPlayer.IsSquatHeadDetected())
        {
            stateMachine.ChangeState(player.squatState);
        }
    }
}