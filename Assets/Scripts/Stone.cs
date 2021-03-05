using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stone:ResourceBaseObject {


	// Start is called before the first frame update
	void Start() {
		InitResource();
	}

	/// <summary>
	/// 初始化资源属性
	/// 出产周期、单位容量和出产资源
	/// </summary>
	private void InitResource() {
		COLD_TIME = 2;
		LastTIme = 0;
		UnitContain_Current = 0;
		UnitContain_Max = 4;
		ProduceProcess_Max = COLD_TIME;
		workerList = new List<Villager>();
	}

	// Update is called once per frame
	void Update() {
		Produce();
	}

	/// <summary>
	/// 石料的制造方法
	/// 增加工作人数的石料
	/// </summary>
	public override void Produce() {

		//如果无村民工作 退出
		if (UnitContain_Current == 0) {
			return;
		}

		//如果未到制造周期 退出
		ProduceProcess_Current = Time.time - LastTIme;
		if (ProduceProcess_Current< ProduceProcess_Max) {
			return;
		}
		LastTIme = Time.time;

		//根据工作人数修改产出
		ResourceProduct = new ResourceCountEventArg(0, 0, 0, 0, 1 * UnitContain_Current, 0);

		GM.ResourceChangeEvent(new object(), ResourceProduct);
		print(workerList.Count + " " + UnitContain_Current + "Produce");
	}
}
