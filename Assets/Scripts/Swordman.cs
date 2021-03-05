using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : Mankind {

    public static int Food_Use = 10;
    public static int Wood_Use = 5;
    public static int Iron_Use = 5;

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
        Initialize(50, 50, 10, 1.0f, 0.7f, "Mankind", 0.28f);        
    }



}
