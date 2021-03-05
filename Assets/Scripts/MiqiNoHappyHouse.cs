using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiqiNoHappyHouse : MonoBehaviour {
	public static bool IsCanSpawn = false;

	public GameObject Skeleton;
	public GameObject SkeletonArcher;

	private int COLD_TIME = 30;
	private float _lastTime = 0;
	private int i = 0;
	private List<GameObject> monsterList;
	// Start is called before the first frame update
	void Start() {
		monsterList = new List<GameObject>() {Skeleton/*, SkeletonArcher, Skeleton, SkeletonArcher, Skeleton, SkeletonArcher, Skeleton, SkeletonArcher, Skeleton, SkeletonArcher*/ };
	}

	// Update is called once per frame
	void Update() {
		MonsterSpawn();
	}

	private void MonsterSpawn() {

		if (IsCanSpawn) {
			if (Time.time - _lastTime >= COLD_TIME) {
				_lastTime = Time.time;
				if (i == monsterList.Count) {
					i = 0;
					print("No more");
					IsCanSpawn = false;
					return;
				}
				Instantiate(monsterList[i], this.transform.position, Quaternion.identity);
				print(Time.time);
				
				i++;
			}
		}
	}
}
