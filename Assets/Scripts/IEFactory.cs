using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人类单位工厂模式-接口
/// </summary>
public interface IEFactory  {

    /// <summary>
    /// 生产单位接口方法
    /// </summary>
    /// <param name="cretaePos">生成位置</param>
    /// <returns></returns>
    BaseObject CreateMankind();
}
