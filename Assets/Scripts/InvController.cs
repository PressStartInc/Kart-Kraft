using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvController : MonoBehaviour {
    public bool rolling;
    public float rollTime;
    public int item;
    public int heldItem;
	public bool boosting;
	public float boostDuration;
	float maxVelocity;
	private KartController_pat1 kartController;
	public string s_player;
	public HMissile hMissile;
	public GameObject sMissile;
	public bool b_AI;
	public c_AI_r1 AIScript;

	// void OnTriggerEnter(Collider other){
	// 	if(other.gameObject.CompareTag("PickUp")){
	// 		if(other.gameObject.transform.parent.CompareTag("Speed Boost")){
	// 			Destroy(other.gameObject.transform.parent.gameObject);
	// 			heldItem = 1;
	// 		}
	// 		else if(other.gameObject.transform.parent.CompareTag("Red Shell")){
	// 			Destroy(other.gameObject.transform.parent.gameObject);
	// 			heldItem = 2;							
	// 		}
	// 		else if(other.gameObject.transform.parent.CompareTag("Lead Obstacles")){
	// 			Destroy(other.gameObject.transform.parent.gameObject);
	// 			heldItem = 3;
	// 		}
	// 	}
	// }
	void Start(){
		kartController = GetComponent<KartController_pat1>();
		AIScript = GetComponent<c_AI_r1>();
		s_player = kartController.s_player;
		Debug.Log("player number= "+s_player);
		b_AI = kartController.b_AI;
	}

	void FixedUpdate(){
        if (rolling)
        {
            if (rollTime <= 0 || (b_AI && AIScript.b_useItem) || (!b_AI && Input.GetButton("p"+s_player+"Item")))
            {
            	AIScript.b_useItem = false;
                heldItem = item;
                rolling = false;
            }
            heldItem = Random.Range(1, 3);
            rollTime -= Time.deltaTime;
        }
        if (boosting){
			if(boostDuration<=0){
				boosting = false;
			}
			kartController.f_mVelocity = maxVelocity*1.2f;
			boostDuration -= Time.deltaTime;			
		}
        Debug.Log("held item = " + heldItem);
		if (AIScript.b_useItem || !b_AI && Input.GetButton("p"+s_player+"Item")) {
			AIScript.b_useItem=false;
			useItem(heldItem);
		}
	}

    public void getItem()
    {
        item = Random.Range(1, 3);
        rolling = true;
        rollTime = 5f;
    }

	void useItem(int usedItem){
		if(usedItem==1){
			heldItem = 0;
			boosting = true;
			boostDuration = 3.0f;
			maxVelocity = kartController.f_mMaxVelocity;
		}
		if(usedItem==2){
			sMissile = (GameObject)Instantiate(hMissile, transform.position, transform.rotation);
			heldItem=3;
		}
		if(usedItem==3){
			sMissile.GetComponent<HMissile>().detonate();
		}
		heldItem = 0;
	}	
}