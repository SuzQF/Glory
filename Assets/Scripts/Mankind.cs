using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人类基类
/// </summary>
public class Mankind : BaseObject
{

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 移动方法
    /// </summary>
    public virtual void Move()
    {

        if (attackObject != null && MissionTarget != null)
        {
            AutoGetTargetEnemy();
        }
        if (transform.position.x != MoveTarget_Pos.x)
        {         
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(MoveTarget_Pos.x, transform.position.y, 0),
               Time.deltaTime * Move_Speed);
            TrunAround();
            Role_Animator.SetFloat("Distance", Math.Abs(transform.position.x - MoveTarget_Pos.x));
        }

    }

    /// <summary>
    /// 转向
    /// </summary>
    public void TrunAround()
    {
        Vector2 direction = MoveTarget_Pos - transform.position;
        //攻击目标不在自身攻击范围情况，朝向由移动方向确定
        //否则
        //攻击目标在自身攻击范围情况，朝向由敌方位置方向确定
        if (!AttackTarget_IsEmpty())
        {
            direction = AttackTarget_Pos - transform.position;
        }

        //transform.right.x朝右1，朝左-1

        //当目标在人物前方，人物并且朝向相反时，转向
        if (direction.x > 0 && transform.right.x < 0)
            transform.right = -transform.right;
        //当目标在人物后方，人物并且朝向相反时，转向
        if (direction.x < 0 && transform.right.x > 0)
            transform.right = -transform.right;       
    }

    #region Mankind Trigger的触发
    /// <summary>
    /// 范围监测Enemy进入
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollider2D>()
            .IsTouching(this.GetComponent<CircleCollider2D>()))
        {

            //不存在攻击目标时，进入了新的Monster标签的实列，重新确认攻击目标          
            //当目标为空的情况
            if (collision.tag == "Monster" && AttackTarget_IsEmpty())
            {

                Confirm_AttackTarget<Monster>(collision.gameObject);
                return;
            }
            //当判断目标是指定目标的情况
            if (MissionTarget != null)
            {
                if ((collision.tag == "Monster" && collision.name == MissionTarget.name) && !IsInvoking("Attack"))
                {

                    MoveTarget_Pos = transform.position;
                    Confirm_AttackTarget<Monster>(collision.gameObject);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 范围监测Enemy存在于攻击范围
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollider2D>()
            .IsTouching(this.GetComponent<CircleCollider2D>()))
        {
            //不存在攻击目标时，进入了新的Monster标签的实列，重新确认攻击目标          
            //当目标为空的情况
            if (collision.tag == "Monster" && AttackTarget_IsEmpty())
            {
                print(this.gameObject + "发现新目标");
                Confirm_AttackTarget<Monster>(collision.gameObject);
                return;
            }
            //当目标是指定目标的情况
            if(MissionTarget!=null)
            {
                if ((collision.tag == "Monster" && collision.name == MissionTarget.name)&&!IsInvoking("Attack"))
                {
                    print(this.gameObject + "已经发现指定目标");
                    MoveTarget_Pos = transform.position;
                    Confirm_AttackTarget<Monster>(collision.gameObject);
                    return;
                }

            }
        }
    }

    /// <summary>
    /// 监测EnemyExit
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
       if(attackObject != null)
        {
            if (collision.name == attackObject.name)
            {
                Reset_AttackTarget<Monster>(collision.gameObject);
            }
            
        }
    }

    /// <summary>
    /// 碰撞改变BoxCollider
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.003f, 0.343f);

        //在一定时间后恢复BoxCollider正常大小
        if (!IsInvoking("ResetBoxCollider"))
            Invoke("ResetBoxCollider", 0.9f);

    }

    /// <summary>
    /// 恢复BoxCollider
    /// </summary>
    private void ResetBoxCollider()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.153f, 0.343f);
    }
    #endregion  
    /* 
     *       需要覆写的方法，添加本类的特殊方法         
     * 每种不同的类具有自己的攻击动画，以及可能的攻
     * 击效果等，所以采用Virtual了Baseobject中的方法
     * 
     */

    /// <summary>
    /// 覆写Confirm_AttackTarget，添加动画
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Enemy"></param>
    public override void Confirm_AttackTarget<T>(GameObject Enemy)
    {
        base.Confirm_AttackTarget<T>(Enemy);
        if (!IsInvoking("Attack"))
        {
            InvokeRepeating("Attack", 0.1f, Attack_Speed);
            //本单位的攻击动画切换
            Role_Animator.SetBool("Attack", true);
        }
    }

    /// <summary>
    /// 覆写Reset_AttackTarget，添加动画
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Enemy"></param>
    public override void Reset_AttackTarget<T>(GameObject Enemy)
    {
        base.Reset_AttackTarget<T>(Enemy);
        if (IsInvoking("Attack"))
        {
            CancelInvoke("Attack");
            //本单位的攻击动画切换
            Role_Animator.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// 自动索敌
    /// </summary>
    public virtual void AutoGetTargetEnemy()
    {
        if (attackObject.transform.position.x > 0)
        {
            MoveTarget_Pos = attackObject.transform.position -
            new Vector3(GetComponent<CircleCollider2D>().radius, 0, 0);
        }
        else
        {
            MoveTarget_Pos = attackObject.transform.position +
            new Vector3(GetComponent<CircleCollider2D>().radius, 0, 0);
        }
    }

}


