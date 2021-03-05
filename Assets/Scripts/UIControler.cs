using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIControler : MonoBehaviour, IPointerClickHandler {
	public static EventHandler UI_Clicked;

	public GameObject BuildingUI;
	private GameObject _buildingTempUI;
    public static bool? IsUIClicked =false;

    public ResourceCountEventArg BuildCost = new ResourceCountEventArg(0, 0, 1, 12, 14, 13);
	public void OnPointerClick(PointerEventData eventData) {
		print("UI Clicked");
        if (GM.TryCostResource(BuildCost))
        {
            print("JudgeOK");
            _buildingTempUI = Instantiate(BuildingUI, Input.mousePosition, this.transform.rotation);
            _buildingTempUI.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            IsUIClicked = true;
        }
        //_granary.transform.parent = GameObject.Find("Ground/BuildingUI").transform;
    }

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		
	}

    private void Awake()
    {
        
    }
}
