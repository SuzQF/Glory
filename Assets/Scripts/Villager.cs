using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Mankind {

    /// <summary>
    /// 当前村民选中的建筑
    /// </summary>
    public BaseBuildingObject selectBuilding;

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

    public void Work()
    {

    }

    /// <summary>
    /// 建造方法
    /// </summary>
    public void Build()
    {
        if(selectBuilding!=null && !IsInvoking("ValueUp_Build"))
            InvokeRepeating("ValueUp_Build", 0.5f, 1f);
    }

    /// <summary>
    /// 修复方法
    /// </summary>
    public void Repire()
    {
        if(selectBuilding!=null && !IsInvoking("ValueUp_Repire"))
            InvokeRepeating("ValueUp_Repire", 0.5f, 1f);
    }

    /// <summary>
    /// 村民范围内trriger进入物体触发
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(collision.GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>())
            && collision.tag=="Building")
        {
            if(collision!=null && MissionTarget!=null)
            {
                if (collision.name == MissionTarget.name)
                {
                    selectBuilding = GameObject.Find(collision.gameObject.name).GetComponent<BaseBuildingObject>();
                    if (selectBuilding)
                    {
                        if (selectBuilding.Complete)
                            Repire();
                        else
                            Build();
                    }
                }
            }      
        }
    }

    /// <summary>
    /// 村民范围内trriger存在物体触发
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);

        if (collision.GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>())
            && collision.tag == "Building")
        {
            if (collision != null && MissionTarget != null)
            {
                if (collision.name == MissionTarget.name)
                {
                    selectBuilding = GameObject.Find(collision.gameObject.name).GetComponent<BaseBuildingObject>();
                    if (selectBuilding)
                    {
                        if (selectBuilding.Complete)
                            Repire();
                        else
                            Build();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 村民范围内trriger退出物体触发
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(selectBuilding)
        {
            CancelBuildOrRepire();
            selectBuilding = null;
        }
    }

    /// <summary>
    /// 建造HP增加方法
    /// </summary>
    private void ValueUp_Build()
    {
        int val = selectBuilding.HP_Max - selectBuilding.HP_Current;
        if (val >= 10)
        {
            selectBuilding.HP_Current += 10;
        }
        else
        {
            selectBuilding.HP_Current += val;
            selectBuilding.Complete = true;
            selectBuilding.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
        }
    }

    /// <summary>
    /// 修复HP值增加方法
    /// </summary>
    private void ValueUp_Repire()
    {
        int val = selectBuilding.HP_Max - selectBuilding.HP_Current;
        if (val >= 10)
        {
            selectBuilding.HP_Current += 10;
        }
        else
        {
            selectBuilding.HP_Current += val;
        }
    }

    /// <summary>
    /// 取消建造或修复调用
    /// </summary>
    private void CancelBuildOrRepire()
    {
        if (IsInvoking("ValueUp_Build"))
            CancelInvoke("ValueUp_Build");
        if (IsInvoking("ValueUp_Repire"))
            CancelInvoke("ValueUp_Repire");
    }
}
