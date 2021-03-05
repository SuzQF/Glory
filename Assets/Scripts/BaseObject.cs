using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 游戏物体基类
/// </summary>
public class BaseObject : MonoBehaviour {

    /*
     * 游戏生物基类:
     * 游戏中可运动，可攻击等的生物体继承该类
     *
     * 要点:
     * 1. 初始化数据请在子类的Awake()中调用initialize()进行初始化
     * 2. 改变攻击距离请使用ChangeAttack_Distance()
     * 3. 覆写EnemyDeathHandler<T>请在目前已经有的代码上进行添加
     *
     */

    
    public int HP_Max = 0;                  //最大HP
    public int HP_Current= 0;               //当前HP
    public GameObject MissionTarget;        //任务目标
    public int Attack_Value = 0;            //攻击力
    public float Attack_Distance = 0.25f;   //攻击范围
    public float Move_Speed = 0.5f;         //移动速度
    public float Attack_Speed = 1.0f;       //攻击速度
    public GameObject attackObject;         //攻击对象


    public EventHandler deathEvent;         //自身死亡事件
    public EventHandler<AttackHurt> attackEvent;        //对目标的攻击事件


    public Animator Role_Animator;          //该单位的动画状态机
    public Vector3 MoveTarget_Pos;           //该单位的移动目标点
    public Vector3 AttackTarget_Pos;         //单位的攻击目标点(该目标点在敌方刚进入攻击范围后会被第一次赋值)

    /// <summary>
    /// 修改攻击范围 需要修改攻击范围，则调用该方法
    /// </summary>
    /// <param name="distance"></param>
    private void ChangeAttack_Distance(float distance)
    {
        CircleCollider2D collider = this.gameObject.GetComponent<CircleCollider2D>();
        if(collider.isActiveAndEnabled)
        {
            this.Attack_Distance = distance;
            collider.radius = Attack_Distance;
        }     
    }

    /// <summary>
    /// 对象初始化
    /// </summary>
    /// <param name="p_Hp_Max">最大生命值</param>
    /// <param name="p_HP_Previous">当前生命值</param>
    /// <param name="p_Attack_Value">攻击力</param>
    /// <param name="p_Attack_Speed">攻速</param>
    /// <param name="p_Move_Speed">移速</param>
    /// <param name="p_tag">标签</param>
    /// <param name="p_Attack_Distance">攻击距离</param>
    public void Initialize(int p_Hp_Max, int p_HP_Current, int p_Attack_Value,float p_Attack_Speed,float p_Move_Speed, string p_tag,
        float p_Attack_Distance)
    {
        HP_Max = p_Hp_Max;
        HP_Current = p_HP_Current;
        Attack_Value = p_Attack_Value;
        Attack_Speed = p_Attack_Speed;
        Move_Speed = p_Move_Speed;
        this.tag = p_tag;
        ChangeAttack_Distance(p_Attack_Distance);

        Role_Animator = GetComponent<Animator>();
        deathEvent += ReadyDeath;

        //移动目标与攻击目标点位为当前自身位置
        MoveTarget_Pos = transform.position;
        AttackTarget_Pos =  transform.position;
    }

   

    #region 敌人锁定判定 
    /// <summary>
    /// 确认攻击目标
    /// </summary>
    /// <param name="Enemy"></param>
    public virtual void Confirm_AttackTarget<T>(GameObject Enemy)
    {
        /*
         * 确认攻击目标:
         * 
         * 需要本单位注册 Enemy 的死亡事件
         * 需要 Enemy 注册本单位的攻击事件
         * 
         */
        if (typeof(T) == typeof(Monster))
        {
            Monster monster = GameObject.Find(Enemy.name).GetComponent<Monster>();
            monster.deathEvent += EnemyDeathHandler<T>;
            this.attackEvent += monster.SelfIsAttackedHandler;
            //获取攻击目标的position
            AttackTarget_Pos = monster.transform.position;
            attackObject = monster.gameObject;
        }
        if (typeof(T) == typeof(Mankind))
        {

            Mankind mankind = GameObject.Find(Enemy.name).GetComponent<Mankind>();
            mankind.deathEvent += EnemyDeathHandler<T>;
            this.attackEvent += mankind.SelfIsAttackedHandler;
            //获取攻击目标的position
            AttackTarget_Pos = mankind.transform.position;
            attackObject = mankind.gameObject;
        }

        if(typeof(T) == typeof(BaseBuildingObject))
        {
            BaseBuildingObject building = GameObject.Find(Enemy.name).GetComponent<BaseBuildingObject>();
            if(building.HP_Current!=0)
            {
                //获取攻击目标的position
                AttackTarget_Pos = building.transform.position;
                attackObject = building.gameObject;
            }
        }
        print(this.name + "发现有敌人 : 注册对方事件 : 锁定目标");
    }

    /// <summary>
    /// 重置攻击目标
    /// </summary>
    /// <param name="Enemy">上一个无效的目标</param>
    public virtual void Reset_AttackTarget<T>(GameObject Enemy)
    {
        /*
         * 重置攻击目标:
         * 
         * 需要本单位取消 Enemy 的死亡事件
         * 需要 Enemy 取消本单位的攻击事件
         * 
         */

        if (typeof(T) == typeof(Monster))
        {
            Monster monster = GameObject.Find(Enemy.name).GetComponent<Monster>();
            monster.deathEvent -= EnemyDeathHandler<T>;
            this.attackEvent -= monster.SelfIsAttackedHandler;

            //无攻击目标时，恢复攻击点位为自身位置
            AttackTarget_Pos = transform.position;
            attackObject = null;
        }
        if (typeof(T) == typeof(Mankind))
        {      
            Mankind mankind = GameObject.Find(Enemy.name).GetComponent<Mankind>();
            mankind.deathEvent -= EnemyDeathHandler<T>;
            this.attackEvent -= mankind.SelfIsAttackedHandler;


            //无攻击目标时，恢复攻击点位为自身位置
            AttackTarget_Pos = transform.position;
            attackObject = null;
        }
        if (typeof(T) == typeof(BaseBuildingObject))
        {
            BaseBuildingObject building = GameObject.Find(Enemy.name).GetComponent<BaseBuildingObject>();

            if (building.HP_Current != 0)
            {
                //无攻击目标时，恢复攻击点位为自身位置
                AttackTarget_Pos = building.transform.position;
                attackObject = null;
            }
        }
        print(this.name + "清空已有目标");
    }

    /// <summary>
    /// 判断攻击无攻击对象
    /// </summary>
    /// <returns></returns>
    public bool AttackTarget_IsEmpty()
    {
        if (attackObject == null)
            return true;
        return false;
    }    
    #endregion

    /// <summary>
    /// 目标敌人死亡处理方法,如果对死亡敌人有所操作，覆写该方法
    /// </summary>
    /// <param name="o">死亡目标自身</param>
    /// <param name="e"></param>
    public virtual void EnemyDeathHandler<T>(object o, EventArgs e)
    {
        var Enemy = o as GameObject;
        Reset_AttackTarget<T>(Enemy);
    }

    /// <summary>
    /// 攻击方法
    /// 每个单位都有自己的攻击方法和伤害数值，覆写该方法
    /// </summary>
    public virtual void Attack()
    {
        AttackHurt hurt = new AttackHurt(this.Attack_Value);

        //当攻击单位为人形单位时
        if (attackEvent != null)
        {
            print(this.name + "开始攻击");
            attackEvent(this.gameObject, hurt);
        }
        else
            print("Event Null");

        //当攻击单位为建筑单位时
        if (attackObject!=null && attackObject.tag == "Building")
        {
            AttackBuilding(attackObject);
        }
    }

    /// <summary>
    /// 被攻击处理方法
    /// </summary>
    /// <param name="o">攻击方</param>
    /// <param name="e"></param>
    public virtual void SelfIsAttackedHandler(object o, AttackHurt e)
    {
        //var Enemy = o as GameObject;
        ///TODO:
        ///伤害计算
        print(this.tag + "受到伤害" + e.Hurt);
        this.HP_Current = AttackHurt.HurtCalculate(this.HP_Current, e.Hurt);
    }

    /// <summary>
    /// 判断死亡
    /// </summary>
    public void DecideDeath()
    {
        if (HP_Current == 0)
        {
            print(this.name + "死亡");

            //置空物理特性，留下贴图Sprite
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            MoveTarget_Pos = transform.position;

            //调用死亡动画
            Role_Animator.SetBool("Death", true);
            HP_Current = -1;
            
            //调用死亡事件，触发
            deathEvent(this.gameObject, null);
        }
    }

    /// <summary>
    /// 准备死亡
    /// </summary>
    private void ReadyDeath(object o, EventArgs e)
    {
        Invoke("Death", 1.7F);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Death()
    {
        Destroy(this.gameObject);
    }
    
    /// <summary>
    /// 建筑攻击方法
    /// </summary>
    /// <param name="gameObject"></param>
    public void AttackBuilding(GameObject gameObject)
    {
        BaseBuildingObject build =GameObject.Find(attackObject.name).GetComponent<BaseBuildingObject>();

        ///当建筑有效时
        if(build!=null)
        {
            if (build.HP_Current > 0)
                build.HP_Current = AttackHurt.HurtCalculate(build.HP_Current, this.Attack_Value);
            else
            {
                attackObject = null;
                AttackTarget_Pos = transform.position;
            }
        }
        ///建筑无效时
        else
        {
            attackObject = null;
            AttackTarget_Pos = transform.position;
        }
    }
}
