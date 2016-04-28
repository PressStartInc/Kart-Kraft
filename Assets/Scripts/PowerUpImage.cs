using UnityEngine;
using System.Collections;

public class PowerUpImage : MonoBehaviour {
	public InvController script;
	public GameObject a;
	public 
	void Start () {
		script = a.GetComponent<InvController>();
	}
	void FixedUpdate () {
		/*
		if (InvController.heldItem == 1) {
			//a.transform.GetChild(0).GetComponent<Image>.overrideSprite =  Resources.Load<Sprite>("Textures/sprite");
		} 
		else if (InvController.heldItem == 2) {

		}
		else if (InvController.heldItem == 3) {

		}
		*/
	}
}
