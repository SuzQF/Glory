using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Mankind {

    public static int Food_Use = 10;
    public static int Wood_Use = 5;
    public static int Iron_Use = 5;

    public GameObject arrow;

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
        Initialize(50, 50, 10, 2.0f, 0.7f, "Mankind", 4.7f);
    }

    /// <summary>
    /// 攻击方法
    /// </summary>
    public override void Attack()
    {
        if((attackObject.transform.position-transform.position).x > 0)
        {
            Instantiate(arrow, transform.position + new Vector3(0.175f, 0, 0), Quaternion.identity);
        }
        else
        {
            ///这里采用四元数来实现传值监测
            ///如果是在左边，则将围绕x旋转90度
            ///来表示目标在左边
            Quaternion quaternion = Quaternion.Euler(new Vector3(-90, 0, 0));
            Instantiate(arrow, transform.position + new Vector3(-0.175f, 0, 0), quaternion);
        }
    }
}
