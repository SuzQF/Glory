using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_Granary : BaseBuildingObject {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (HP_Current == 0 && Complete==true)
            Destroy(this.gameObject);
    }
}
