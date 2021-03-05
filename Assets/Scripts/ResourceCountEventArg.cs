using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCountEventArg : EventArgs
{
    public int CurrentPopulation = 0; //当前人口
    public int MaxPopulation = 0;//最大人口
    public int Food = 0;//粮食
    public int Wood = 0;//木材
    public int Stone = 0;//石料
    public int Iron = 0;//铁矿



	public ResourceCountEventArg(int currentPopulation, int maxPopilation, int food, int wood, int stone, int iron) {

        CurrentPopulation = currentPopulation;
        MaxPopulation = maxPopilation;
		Food = food;
		Wood = wood;
		Stone = stone;
		Iron = iron;
	}
}
