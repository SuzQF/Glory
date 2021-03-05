using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_Villager:BaseBuildingObject,IEFactory{

    /// <summary>
    /// 预制体村民实例
    /// </summary>
    public BaseObject villager;
    private bool Create = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(this.Complete && !Create)
        {
            InvokeRepeating("CreateMankind",0.1f,100f);

            Create = true;
        }
    }

    private void Awake()
    {
        HP_Max = 100;
        HP_Current = 0;
        Complete = false;       
    }

    /// <summary>
    /// 实现工厂接口-生成村民方法
    /// </summary>
    /// <param name="createPos">生成位置</param>
    /// <returns></returns>
    public BaseObject CreateMankind()
    {
        var mankind = Instantiate(villager,new Vector3(-8.35f, -2.5f, 0), Quaternion.identity);
        mankind.MoveTarget_Pos = transform.position;
        GM.NewUnitEvent(mankind.gameObject, null);
        return mankind;
    }
}
