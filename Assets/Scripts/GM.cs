using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{

    //每种资源类型的名称常量
    public const string MAX_POPULATION = "MaxPopulation";
    public const string CURRENT_POPULATION = "CurrentPopulation";
    public const string FOOD = "Food";
    public const string WOOD = "Wood";
    public const string STONE = "Stone";
    public const string IRON = "Iron";

    private int _timeRatio = 2;//现实时间与游戏时间的比例  单位：s/h
    private float _lastTime = 0;//记录上一次修改时间时的时间

    private static int CurrentPopulation = 0; //当前人口
    private static int MaxPopilation = 10;//最大人口
    private static int Food = 100;//粮食
    private static int Wood = 100;//木材
    private static int Stone = 100;//石料
    private static int Iron = 100;//铁矿

    private List<Swordman> _swordmanList;//剑士List
    private List<Archer> archerList;//弓手List
    private List<Villager> villagerList;//村民List
    private List<Skeleton> skeletonList;//骷髅List

    //Text为游戏中显示Text的控件
    public Text PopulationTxt; //人口
    public Text FoodTxt; //粮食
    public Text WoodTxt; //木材
    public Text StoneTxt;//石料
    public Text IronTxt; //铁矿
    public Text TimerTxt;//时间

    public static int GameShowDay;    //游戏内日期
    public static int GameShowHour;    //游戏内小时


    public static EventHandler NewUnitEvent;  //新单位创建事件
    public static EventHandler ResourceChangeEvent;//资源修改事件

    // Start is called before the first frame update
    void Start()
    {
        NewUnitEvent += AddNewUnit;
        _swordmanList = new List<Swordman>();
        ResourceChangeEvent += ProduceResource;
    }

    // Update is called once per frame
    void Update()
    {
        SetShowTime();
        SetShowResource();
    }


    /// <summary>
    /// 新单位添加事件处理，将单位加入对应的LIst
    /// </summary>
    /// <param name="newUnit"></param>
    /// <param name="e"></param>
    public void AddNewUnit(object newUnit, EventArgs e)
    {
        GameObject gameObject = newUnit as GameObject;
        if (gameObject != null)
        {
            switch (gameObject.name)
            {
                case "SwordMan(Clone)":
                    _swordmanList.Add(gameObject.GetComponent<Swordman>());
                    print(Time.time + "  " + _swordmanList[0].Attack_Distance + "  " + _swordmanList[0].Attack_Speed);
                    break;
                case "Archer(Clone)":
                    break;

                default:
                    break;
            }
        }

    }


    /// <summary>
    /// 消耗资源  减少资源量
    /// </summary>
    /// <param name="rCe"></param>
    /// <returns></returns>
    public static bool TryCostResource(ResourceCountEventArg rCe)
    {
        bool result = true;
        if (
            Food < rCe.Food
            && Wood < rCe.Wood
            && Stone < rCe.Stone
            && Iron < rCe.Iron
            )
        {
            result = false;
        }
        else
        {
            
            Food -= rCe.Food;
            Wood -= rCe.Wood;
            Stone -= rCe.Stone;
            Iron -= rCe.Iron;
        }
        return result;
    }


    /// <summary>
    /// 设置游戏显示时间
    /// </summary>
    public void SetShowTime()
    {
        if (Time.time - _lastTime < _timeRatio)
        {
            return;
        }
        GameShowHour = ((int)Time.time) / _timeRatio;
        GameShowDay = ((int)Time.time) / (_timeRatio * 24);
		if (GameShowHour==2) {
			MiqiNoHappyHouse.IsCanSpawn = true;
		}
        TimerTxt.text = "时间：" + GameShowDay + "天" + GameShowHour + "时";
        _lastTime = Time.time;
        //Test();
    }

    private void Test()
    {
        ProduceResource(new object(),new ResourceCountEventArg(0, 0, 10, 10, 10, 10));
    }

    /// <summary>
    /// 持续刷新资源数量
    /// </summary>
    public void SetShowResource()
    {
        PopulationTxt.text = CurrentPopulation + "/" + MaxPopilation;
        FoodTxt.text = Food + "";
        WoodTxt.text = Wood + "";
        StoneTxt.text = Stone + "";
        IronTxt.text = Iron + "";
    }


    /// <summary>
    /// 生产资源数量  增加资源量
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="value"></param>
    public void ProduceResource(object sender,EventArgs e)
    {
        ResourceCountEventArg resourceCountEventArg = e as ResourceCountEventArg;
        MaxPopilation += resourceCountEventArg.MaxPopulation;
        CurrentPopulation += resourceCountEventArg.CurrentPopulation;
        Food += resourceCountEventArg.Food;
        Wood += resourceCountEventArg.Wood;
        Stone += resourceCountEventArg.Stone;
        Iron += resourceCountEventArg.Iron;
    }

    /// <summary>
    /// 创建存档
    /// </summary>
    /// <returns></returns>
    public bool Save()
    {
        bool result = false;





        return result;
    }
}
