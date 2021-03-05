using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Monster基类
/// </summary>
public class Monster : BaseObject {


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Monster的移动方法
    /// </summary>
    public void Move()
    {
        //当不存在攻击目标时，向终点位置前进
        if(attackObject == null && HP_Current != -1 && (MoveTarget_Pos.x != transform.position.x))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(MoveTarget_Pos.x, transform.position.y, 0),
               Time.deltaTime * Move_Speed);
            TurnAround();
        }
    }

    /// <summary>
    /// 转向
    /// </summary>
    private void TurnAround()
    {
        Vector2 direction = MoveTarget_Pos - transform.position;
        if (!AttackTarget_IsEmpty())
            direction = AttackTarget_Pos - transform.position;

        //transform.right.x朝右-1，朝左1

        if (direction.x < 0 && transform.right.x < 0)
            transform.right = -transform.right;
        if(direction.x > 0 && transform.right.x > 0)
            transform.right = -transform.right;
    }

    #region Monster Trigger的触发
    /// <summary>
    /// 范围监测Enemy进入
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollider2D>()
            .IsTouching(this.GetComponent<CircleCollider2D>()))
        {
            //不存在攻击目标时，进入了新的Mankind标签的实列，重新确认攻击目标
            if (collision.tag == "Mankind" && AttackTarget_IsEmpty())
            {
                Confirm_AttackTarget<Mankind>(collision.gameObject);
            }
            //不存在攻击目标时，进入了新的Building标签的实列，重新确认攻击目标
            if (collision.tag == "Building" && AttackTarget_IsEmpty())
            {
                Confirm_AttackTarget<BaseBuildingObject>(collision.gameObject);
            }
        }
    }

    /// <summary>
    /// 范围监测Enemy存在于攻击范围
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollider2D>()
            .IsTouching(this.GetComponent<CircleCollider2D>()))
        {
            //不存在攻击目标时，进入了新的Mankind标签的实列，重新确认攻击目标
            if (collision.tag == "Mankind" && AttackTarget_IsEmpty())
            {
                Confirm_AttackTarget<Mankind>(collision.gameObject);
            }
            //不存在攻击目标时，进入了新的Building标签的实列，重新确认攻击目标
            if (collision.tag == "Building" && AttackTarget_IsEmpty())
            {
                Confirm_AttackTarget<BaseBuildingObject>(collision.gameObject);
            }
        }

    }

    /// <summary>
    /// 监测EnemyExit
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(attackObject!=null)
        {
            if (collision.name == attackObject.name)
            {
                //攻击单位是人类时的重置方法
               if(attackObject.tag=="Mankind")
                {
                    Reset_AttackTarget<Mankind>(collision.gameObject);
                    return;
                }
               //攻击单位是建筑时的重置方法
               if(attackObject.tag=="Building")
                {
                    Reset_AttackTarget<BaseBuildingObject>(collision.gameObject);
                    return;
                }                   
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
        GetComponent<BoxCollider2D>().size = new Vector2(0.18f, 0.4f);
        GetComponent<BoxCollider2D>().offset = new Vector2(0.012f, -0.060f);
    }
    #endregion

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
    /// <typeparam name="T">攻击目标类型</typeparam>
    /// <param name="Enemy">攻击目标GameObject组件</param>
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
}
