﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InteractiveObject
{

    public override void Start()
    {
        base.Start();
    }

    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.tag != "Sword")
        {
            animator.SetTrigger("collected");
            player.GotKey = true;
        }
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}

