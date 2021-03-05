using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cottage : ResourceBaseObject {
	// Start is called before the first frame update
	void Start() {
		Produce();

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// 村舍生产方法 增加3人口上限
	/// 只执行一次
	/// </summary>
	public override void Produce() {
		ResourceProduct = new ResourceCountEventArg(0, 3, 0, 0, 0, 0);
		GM.ResourceChangeEvent(new object(), ResourceProduct);
	}
}
