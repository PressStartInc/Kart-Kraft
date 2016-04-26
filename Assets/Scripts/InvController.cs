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
	// Object velocity = GetComponent(KartController_pat1).f_mVelocity;
	private KartController_pat1 kartController;


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
	void Awake(){
		kartController = GetComponent<KartController_pat1>();
	}

	void FixedUpdate(){
        if (rolling)
        {
            if (rollTime <= 0 || Input.GetKey(KeyCode.Tab))
            {
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
		if (Input.GetKey(KeyCode.Space)) useItem(1);
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

		}
		if(usedItem==3){

		}
		heldItem = 0;
	}	
}