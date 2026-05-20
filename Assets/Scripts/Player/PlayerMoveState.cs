using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private const int MovingSoundChannel = 15;
    private string _currentMovingClipName;

    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        string[] clipNames = { "Moving1_1", "Moving1_2", "Moving1_3", "Moving1_4" };
        int index = Random.Range(0, clipNames.Length);
        _currentMovingClipName = clipNames[index];
        SoundManager.Instance.Play(MovingSoundChannel, _currentMovingClipName, true, true);
    }

    public override void Exit()
    {
        if (!string.IsNullOrEmpty(_currentMovingClipName))
        {
            SoundManager.Instance.Stop(MovingSoundChannel, _currentMovingClipName);
            _currentMovingClipName = null;
        }

        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //设置速度，实现移动效果
        player.SetVelocity(xInput * (player.mainPlayer.stateMachine.currentState == player.mainPlayer.downState ? 0.8f : 1f) * player.moveSpeed, rb.velocity.y);
        if (xInput == 0)
            stateMachine.ChangeState(player.idleState);
        if (player.isMain == false && player.mainPlayer.IsWallDetected())
            player.SetVelocity(0, rb.velocity.y);
    }
}