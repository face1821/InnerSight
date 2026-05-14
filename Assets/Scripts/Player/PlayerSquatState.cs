using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatState : PlayerGroundedState
{
    public PlayerSquatState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        player.EnterSquatCollider();
        player.spriterd.sprite = player.squatStateImg;
        // SoundManager.instance.Play(2, "BuildUp", false);
    }

    public override void Exit()
    {
        player.ExitSquatCollider();
        player.spriterd.sprite = player.originImg;

        SoundManager.Instance.Pause(2, "SilentWalk_Fast");

        base.Exit();
    }

    //全是if else，看着辣眼睛
    public override void Update()
    {
        base.Update();
        if (stateMachine.currentState != this)
            return;

        if (xInput != 0)
        {
            if (!player.isMain && player.mainPlayer.IsWallDownDetected())
            {
                //如果玩家是A玩家并且B玩家的WallDown检测到了墙壁
                player.SetVelocity(0, rb.velocity.y);
            }
            else
            {
                player.SetVelocity(xInput * player.moveSpeed * 0.7f, rb.velocity.y);
                if (!player.isPlaySilentWalk)
                {
                    SoundManager.Instance.Play(2, "SilentWalk_Fast", true);
                    player.isPlaySilentWalk = true;
                }
            }
        }
        else
        {
            if (player.isPlaySilentWalk)
            {
                player.isPlaySilentWalk = false;
                SoundManager.Instance.Pause(2, "SilentWalk_Fast");
            }
        }

        if (yInput >= 0 && !player.IsSquatHeadDetected())
        {
            if (!player.isMain && player.mainPlayer.IsSquatHeadDetected())
            {
                return;
            }

            stateMachine.ChangeState(player.idleState);
        }
    }
}