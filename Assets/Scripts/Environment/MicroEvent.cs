﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroEvent : MonoBehaviour
{
    [SerializeField]
    GameObject lightningBolt;
    [SerializeField]
    GameObject targetDisablingObject;
    [SerializeField]
    GameObject targetEnablingObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lightningBolt.SetActive(true);
            StartCoroutine(ChangeObjects());
        }
    }

    IEnumerator ChangeObjects()
    {
        yield return new WaitForSeconds(0.2f);
        targetDisablingObject.SetActive(false);
        targetEnablingObject.SetActive(true);
    }
}