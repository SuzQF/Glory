using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster {

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
       
        DecideDeath();
        Move();
    }


    private void Awake()
    {
        Initialize(50, 50, 10, 1.0f, 0.7f, "Monster", 0.28f);
        MoveTarget_Pos = new Vector3(-8, -2.725f, 0);
    }

}
