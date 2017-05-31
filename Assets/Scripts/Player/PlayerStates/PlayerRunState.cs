﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IPlayerState
{
    public void Enter(Player player)
    {
        player.myArmature.animation.FadeIn("run", -1, -1);
    }

    public void Execute()
    {
        if (Input.GetAxis("Horizontal") == 0)
        {
            Player.Instance.ChangeState(new PlayerIdleState());
        }
        if (Player.Instance.Jump || !Player.Instance.OnGround)
        {
            Player.Instance.ChangeState(new PlayerJumpState());
        }
        if (Player.Instance.Attack && Mathf.Abs(Player.Instance.myRigidbody.velocity.x) >= 7.5f)
        {
            Player.Instance.ChangeState(new PlayerRunAttackState());
        }
        else if (Player.Instance.Attack && Mathf.Abs(Player.Instance.myRigidbody.velocity.x) < 7.5f)
        {
            Player.Instance.ChangeState(new PlayerAttackState());
        }
        if (Player.Instance.takeHit)
        {
            Player.Instance.ChangeState(new PlayerTakeHitState());
        }
    }

    public void Exit() { }
}
