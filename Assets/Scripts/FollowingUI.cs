using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingUI : MonoBehaviour
{

    private static string SWORDMAN_UI = "FollowingUISword(Clone)";
    private static string ARCHER_UI = "FollowingUIArrow(Clone)";

    public GameObject FollowingTarget { get; set; }
	public GameObject ParentBuilding;

	public FollowingUI(GameObject target) {
		this.FollowingTarget = target;
	}


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		Follow();
	}
	
    /// <summary>
	/// 点击UI开始将村民转换
	/// </summary>
	private void ClickToProduce() {
        print(SWORDMAN_UI);
        print(gameObject.name);
        if(gameObject.name==SWORDMAN_UI)
        {
            print("SwordMAn");
            ParentBuilding.GetComponent<Factory_SwordMan>().villager = FollowingTarget;
            ParentBuilding.GetComponent<Factory_SwordMan>().OnUIclicked();
        }
        if (gameObject.name == ARCHER_UI)
        {
            ParentBuilding.GetComponent<Factory_Archer>().villager = FollowingTarget;
            ParentBuilding.GetComponent<Factory_Archer>().OnUIclicked();
        }
    }

    /// <summary>
    /// UI跟随
    /// </summary>
	private void Follow() {
        if (ParentBuilding ==null||FollowingTarget==null)
        {
            return;
        }
        if (!ParentBuilding.GetComponent<BaseBuildingObject>().Complete)
        {
            transform.position = new Vector3(0, 1000, 0);
        }
        else
        {
            if (gameObject.name == SWORDMAN_UI)
            {
                this.transform.position = new Vector3(FollowingTarget.transform.position.x,
                                            FollowingTarget.transform.position.y + 0.5f,
                                            FollowingTarget.transform.position.z);
            }
            if (gameObject.name == ARCHER_UI)
            {
                this.transform.position = new Vector3(FollowingTarget.transform.position.x,
                                            FollowingTarget.transform.position.y + 1f,
                                            FollowingTarget.transform.position.z);
            }
        }
    }


    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickToProduce();
        }
    }
}
