﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogPatrolState : IHedgehogState
{
    private Hedgehog enemy;
    bool preAttacked = false;

    public void Enter(Hedgehog enemy)
    {
        this.enemy = enemy;
        enemy.movementSpeed = 1;
        enemy.armature.animation.timeScale = 2f;
    }

    public void Execute()
    {
        if (!preAttacked)
        {
            enemy.armature.animation.FadeIn("Roll", -1, 1);
            preAttacked = true;
        }
        if (preAttacked && enemy.armature.animation.isCompleted)
        {
            enemy.armature.animation.timeScale = 3f;
            enemy.movementSpeed = 10;
            enemy.armature.animation.FadeIn("Roll_2");
        }
        enemy.Move();
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
            enemy.ChangeState(new HedgehogIdleState());
        }
    }
}
