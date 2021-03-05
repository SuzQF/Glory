using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory_SwordMan : BaseBuildingObject, IEFactory
{
    /// <summary>
    /// 预制体剑士实例
    /// </summary>
    public BaseObject swordMan;
    /// <summary>
    /// 跟随UI
    /// </summary>
    public GameObject FollowingUI;
    /// <summary>
    /// 待转换成Swordman的村民实例
    /// </summary>
    public GameObject villager = null;

    private List<GameObject> UIList;//显示的UIList
    private bool isNewVillager = false;//判断是否为新村民

    private void Awake()
    {
        HP_Max = 100;
        HP_Current = 0;
        Complete = false;

        UIList = new List<GameObject>();
    }

    /// <summary>
    /// 实现接口-生成剑士方法
    /// </summary>
    /// <param name="createPos">生成位置</param>
    /// <returns></returns>
    public BaseObject CreateMankind()
    {
        return Instantiate(swordMan, 
            new Vector3(transform.position.x, transform.position.y - 1, 0), 
            Quaternion.identity);
    }

    /// <summary>
    /// 转换UI点击事件触发方法
    /// </summary>
    public void OnUIclicked()
    {
        if (villager != null)
        {
            //隐藏村民
            villager.transform.position = new Vector3(villager.transform.position.x, -8f, 0);
            Invoke("VillagerToSwordMan", 2f);
        }
    }

    /// <summary>
    /// 转换方法，Villager->Swordman
    /// </summary>
    private void VillagerToSwordMan()
    {
        if (villager != null)
        {
            Destroy(villager);
            var mankind = CreateMankind();
            GM.NewUnitEvent(mankind, null);
        }
    }

    /// <summary>
    /// 判断村民进入
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameObject.Find(collision.name).GetComponent<Villager>())
        {
            if (collision.GetComponent<BoxCollider2D>().IsTouching(gameObject.GetComponent<BoxCollider2D>()))
            {
                if (JudgeVillager(collision.gameObject))
                {
                    ShowUI(collision.gameObject);
                }
            }

        }
    }

    /// <summary>
    /// 判断村民退出
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameObject.Find(collision.name).GetComponent<Villager>())
        {
            RemoveVillager(collision.gameObject);
        }
    }

    /// <summary>
    /// 村民离开时判定摧毁UI
    /// </summary>
    /// <param name="villager"></param>
    private void RemoveVillager(GameObject villager)
    {
        if (UIList.Count==0)
        {
            return;
        }
        for (int i = 0; i < UIList.Count; i++)
        {
            if (UIList[i]==null)
            {
                return;
            }
            if (UIList[i].GetComponent<FollowingUI>().FollowingTarget == villager)
            {
                Destroy(UIList[i]);
                UIList.RemoveAt(i);
                break;
            }
        }

    }

    private bool JudgeVillager(GameObject Villager)
    {
        isNewVillager = true;
        if (UIList.Count > 0)
        {
            for (int i = 0; i < UIList.Count; i++)
            {
                if (UIList[i].GetComponent<FollowingUI>().FollowingTarget == Villager)
                {
                    isNewVillager = false;
                    break;
                }
            }
        }
        return isNewVillager;
    }

    private void ShowUI(GameObject villagerGameObject)
    {
        print("Show");
        GameObject followingUI = Instantiate(FollowingUI, new Vector3(villagerGameObject.transform.position.x,
                                                                    villagerGameObject.transform.position.y + 0.3f,
                                                                    villagerGameObject.transform.position.z),
                                                                    Quaternion.AngleAxis(45.0f, new Vector3(0, 0, -1)));

        followingUI.GetComponent<FollowingUI>().FollowingTarget = villagerGameObject;
        followingUI.GetComponent<FollowingUI>().ParentBuilding = this.gameObject;
        UIList.Add(followingUI);
        print("Following Target locked" + followingUI.GetComponent<FollowingUI>().ParentBuilding.name);
        //followingUI.transform.parent = gameObject.transform;
    }
}
