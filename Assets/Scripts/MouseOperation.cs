using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOperation : MonoBehaviour
{
    /// <summary>
    /// 当前选中目标
    /// </summary>
    public static GameObject SelectItem;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        GetLeftClickObject();
        GetRightClickObject();
    }
    
    /// <summary>
    /// 左键点击获取目标
    /// </summary>
    private void GetLeftClickObject()
    {
        if (UIControler.IsUIClicked == true)
        {
            return;
        }
        if (UIControler.IsUIClicked == null)
        {
            MoveOrder();
            UIControler.IsUIClicked = false;         
            return;
        }
        //获取左键点击位置的物体对象
        if (Input.GetMouseButtonUp(0))
        {
            Collider2D[] collider2Ds = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (collider2Ds.Length > 0) 
            {
                foreach (Collider2D item in collider2Ds)
                {
                    if ((item as BoxCollider2D) != null && item.gameObject.tag == "Mankind")
                    {
                        SelectItem = item.gameObject;
                        print("选中" + item.name);                     
                        break;
                    }
                    SelectItem = null;
                }

                ///当选中目标时村民的情况，是否展示UI
                if (SelectItem!=null && GameObject.Find(SelectItem.name).GetComponent<Villager>())
                    UIshow(true);
                else
                    UIshow(false);
            }
            else
                SelectItem = null;
        }   
    }

    /// <summary>
    /// 右键给出指令目标
    /// </summary>
    private void GetRightClickObject()
    {  
        if (Input.GetMouseButtonUp(1) && SelectItem!=null)
        {
            //如果点击右键时是建造中状态 则取消建造
            if (UIControler.IsUIClicked == true)
            {
                UIControler.IsUIClicked = false;
                BuildingUI.IsCancelBuild = true;
                return;
            }
            //获取右键点击位置的物体对象
            Collider2D[] collider2Ds = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (collider2Ds.Length > 0)
            {
                foreach (Collider2D item in collider2Ds)
                {
                    if ((item as BoxCollider2D) != null)
                    {
                        if (item.tag=="Monster")
                        {
                            AttackOrder(item.gameObject);
                            return;
                        }
                        if(item.tag=="Building")
                        {
                            WorkOrder(item.gameObject);
                            return;
                        }
                        if(item.tag=="Resources")
                        {
                            WorkOrder(item.gameObject);
                        }
                        break;
                    }
                    else
                    {

                        MoveOrder();
                    }
                }
            }
            else
            {
                MoveOrder();
            }
        }
    }

    /// <summary>
    /// 判断目标是否有效
    /// </summary>
    /// <returns></returns>
    private bool SelectItemIsVaild()
    {
        if (SelectItem)
            return true;
        return false;
    }

    /// <summary>
    /// 移动指令
    /// </summary>
    private void MoveOrder()
    {
        var v = GameObject.Find(SelectItem.name).GetComponent<Mankind>();
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);        
        v.AttackTarget_Pos = v.transform.position;
        v.MoveTarget_Pos = pos;
        if(UIControler.IsUIClicked!=null)
            v.MissionTarget = null;
    }

    /// <summary>
    /// 攻击指令
    /// </summary>
    private void AttackOrder(GameObject Enemy)
    {
        var v = GameObject.Find(SelectItem.name).GetComponent<Mankind>();

        v.attackObject = Enemy.gameObject;
        v.AttackTarget_Pos = Enemy.transform.position;
        v.MoveTarget_Pos = Enemy.transform.position;
        v.MissionTarget = Enemy;
    }

    /// <summary>
    /// 工作指令
    /// </summary>
    private void WorkOrder(GameObject WorkTarget)
    {
        var v = GameObject.Find(SelectItem.name).GetComponent<Mankind>();
        v.MoveTarget_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v.MissionTarget = WorkTarget;
    }

    /// <summary>
    /// UI显示
    /// </summary>
    private void UIshow(bool b)
    {
        if (b)
        {
            GameObject.Find("Canvas/ui").gameObject.transform.localPosition = new Vector3(0,0,0);
        }
        else
        {
            GameObject.Find("Canvas/ui").gameObject.transform.localPosition = new Vector3(0, 1000, 0);
        }     
    }
}
