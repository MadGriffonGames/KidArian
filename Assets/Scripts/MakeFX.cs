﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeFX : MonoBehaviour
{
    private static MakeFX instance;

    public static MakeFX Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<MakeFX>();
            return instance;
        }
    }

    [SerializeField]
    private static Transform position;

    [SerializeField]
    private GameObject dust;

    void Start ()
    {
        position = GetComponent<Transform>();
    }
	
	void Update ()
    {
		
	}

    public void MakeDust()
    {
        Instantiate(dust, position.localPosition + new Vector3(0, -0.7f, 0), Quaternion.identity);
    }
}
