using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBaseObject:MonoBehaviour {

	public int ResourceCount { get; set; }
	public string ResourceType { get; set; }
	public int UnitContain_Current { get; set; }
	public int UnitContain_Max { get; set; }
	public float ProduceProcess_Current { get; set; }
	public int ProduceProcess_Max { get; set; }

	//所有资源子类根本的不同只在这一个属性——产出的资源不同
	protected ResourceCountEventArg ResourceProduct;

	public int COLD_TIME { get; set; }
	public float LastTIme { get; set; }
	public List<Villager> workerList;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		Produce();
	}

	public virtual void Produce() {
		
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		Villager villager = GameObject.Find(collision.name).GetComponent<Villager>();
		if (villager!=null
			&&UnitContain_Current < UnitContain_Max
			&& villager.MissionTarget == gameObject
			&& !workerList.Contains(villager)) {

			this.UnitContain_Current += 1;
			workerList.Add(villager);
		}
	}
	private void OnTriggerExit2D(Collider2D collision) {
		Villager villager = GameObject.Find(collision.name).GetComponent<Villager>();
		if (workerList.Contains(villager)) {
			this.UnitContain_Current -= 1;
			workerList.Remove(villager);
		}
	}
}
