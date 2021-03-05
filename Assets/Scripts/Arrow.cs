using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public int Hurt=10;

    // Use this for initialization
    void Start () {

        InvokeFly();
        Invoke("AutoDestory", 10f);
    }
	
	// Update is called once per frame
	void Update () {

        
    }

    /// <summary>
    /// 箭头碰撞单位造成伤害
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="Monster")
        {
            ///如果碰撞到敌人
            Monster monster = GameObject.Find(collision.gameObject.name).GetComponent<Monster>();
            monster.HP_Current -= 10;

            GetComponent<BoxCollider2D>().enabled = false;
            CancelInvoke("Fly");
            Invoke("Vanish", 0.5f);
        }
        ///如果碰撞到地面
        else if(collision.gameObject.tag=="Ground")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            CancelInvoke("Fly");
            Invoke("Vanish", 0.5f);

        }           
    }

    /// <summary>
    /// 箭头前的Trigger碰撞到物体后的处理方法
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<BoxCollider2D>()
            .IsTouching(this.GetComponent<CircleCollider2D>()) && collision.tag=="Mankind")
        {
            print("碰撞");
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        
    }

    /// <summary>
    /// 箭头trigger离开碰撞单位时
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Mankind")
            Invoke("ResetBoxCollider", 0.04f);


    }

    /// <summary>
    /// 恢复BoxCollider
    /// </summary>
    private void ResetBoxCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    /// <summary>
    /// 重复调用Fly
    /// </summary>
    private void InvokeFly()
    {
        InvokeRepeating("Fly", 0.1f, 0.03f);
    }

    /// <summary>
    /// 弓箭飞行
    /// </summary>
    public void Fly()
    {
        if(this.transform.rotation.x==0)
        {
            transform.Translate(new Vector3(0.07f, 0, 0));
            transform.Translate(new Vector3(0, -0.001f, 0));
        }
        else
        {
            //如果当被实例化时，routation x值不为0则说明敌人在左边
            //这里用来起一个类似于传递监测值的作用
            //恢复x旋转量为0，并且旋转z为180
            transform.Translate(new Vector3(-0.07f, 0, 0));
            transform.Translate(new Vector3(0, -0.001f, 0));
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
  
    /// <summary>
    /// 消失
    /// </summary>
    private void Vanish()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 自动销毁
    /// </summary>
    private void AutoDestory()
    {
        if(IsInvoking("MakeFly"))
        {
            CancelInvoke("MakeFly");
        }
        Vanish();
    }    
}
