using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BuildingUI : MonoBehaviour {

	private Color RED = new Color(1, 0, 0, 0.5f);  //不可建红色常量
	private Color GREEN = new Color(0, 1, 0, 0.5f); //可建绿色常量

	private Renderer rend;  //存储Render以改变颜色

	private float FreezeY = -1.62f;	//固定Y轴
	private bool IsCanBuild;	//建筑是否可建造

    public BaseBuildingObject Building;
    public static bool IsCancelBuild;
    

	// Start is called before the first frame update
	void Start() {
		IsCanBuild = true;
		rend = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void Update() {
		UIMove();
		ColorChange();
        CancleBuild();
		GetSettled();
	}

    private void CancleBuild()
    {
        if (IsCancelBuild)
        {
            Destroy(this.gameObject);
            print("Cancel build");
        }
    }

    /// <summary>
    /// 建造方法
    /// </summary>
    private void GetSettled() {
		if (IsCanBuild && Input.GetMouseButton(0) && MouseOperation.SelectItem != null)
        {
            //建造或通知建造
            BaseBuildingObject temp =  Instantiate(Building, this.transform.position, Quaternion.identity);
            Villager villager =  GameObject.Find(MouseOperation.SelectItem.name).GetComponent<Villager>();
            temp.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
    
            if(villager !=null)
            {
                villager.MissionTarget = temp.gameObject;
                print(temp.gameObject);
                villager.MoveTarget_Pos = temp.transform.position;
                villager.selectBuilding = temp;
            }
            IsCanBuild = false;
            Destroy(this.gameObject);
            UIControler.IsUIClicked = null;
        }
	}

	/// <summary>
	/// 通过改变颜色显示可建造与否
	/// </summary>
	private void ColorChange() {
		//可建造为绿色
		if (IsCanBuild) {
			rend.material.color = GREEN;
		}

		//不可建造为红色
		else {
			rend.material.color = RED;
		}
	}
	
    /// <summary>
	/// UI的移动方法，Y轴不变，保持在吸附在地面
	/// 目前平地无影响，若要坡道地形通过设置父级GameObject限制坐标也许可行
	/// </summary>
	public void UIMove() {
		var tempTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(tempTarget.x, FreezeY, 0.01f);
	}

	/// <summary>
	/// 碰撞检测：碰撞体内存在建筑则不可建造
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Building") {
			IsCanBuild = false;
		}
	}
	
    /// <summary>
	/// 碰撞检测：离开建筑区域则可以建造
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Building") {
			IsCanBuild = true;
		}
	}
}
