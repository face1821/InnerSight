using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        player.isDead = true;
        rb.bodyType = RigidbodyType2D.Static;
        SoundManager.Instance.Play(8, "Restart", false, true);
        
        if (Random.value < 0.5f)
            SoundManager.Instance.Play(13, "SiLe1", false, true);
        else
            SoundManager.Instance.Play(13, "SiLe2", false, true);
            
    }

    public override void Exit()
    {
        base.Exit();
        player.isDead = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rb.velocity.y);
    }
}