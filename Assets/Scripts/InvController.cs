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
	public c_waypoint_r1 waypoint;
	public string s_player;
	public HMissile hMissile;
	public GameObject sMissile;
	public bool b_AI;
	public c_AI_r1 AIScript;
	public float f_rollCounter;
	public float f_cycleInterval = 0.25f;
	public AudioSource asBoost;
	public AudioSource asPickup;

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
            if (rollTime <= 0 || (b_AI && AIScript.b_useItem) || (!b_AI && Input.GetButtonDown("p"+s_player+"Item")))
            {
            	AIScript.b_useItem = false;
                heldItem = item;
                rolling = false;
                f_rollCounter = 0;
            }
            f_rollCounter += Time.deltaTime;
            if(f_rollCounter > f_cycleInterval) {
            	heldItem ++;
            	if(heldItem > 2) heldItem-=2;
            	f_rollCounter-= f_cycleInterval;
            }
            
            rollTime -= Time.deltaTime;
        }
        else {
        if(heldItem == 0)
			if(Input.GetKeyDown(KeyCode.X))
				getItem();
		if (AIScript.b_useItem || (!b_AI && Input.GetButtonDown("p"+s_player+"Item")) || (!b_AI && Input.GetKeyDown(KeyCode.Z))) {
			AIScript.b_useItem=false;
			useItem(heldItem);
		}
        }
        if (boosting){
			if(boostDuration<=0){
				boosting = false;
				asBoost.Stop();
			}
			asBoost.volume += -Time.deltaTime/3f;
			kartController.f_mVelocity = maxVelocity*1.2f;
			boostDuration -= Time.deltaTime;			
		}
//        Debug.Log("held item = " + heldItem);
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("PowerUp")){
		// invController = other.gameObject.transform.parent.GetComponent<InvController>();
		// invController.getItem();
			getItem();
			Destroy(other.transform.gameObject);
		}
		if(other.CompareTag("Rocket"))
		StartCoroutine(kartController.SpinOut());
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
			asBoost.Play();
			asBoost.volume = 1.0f;
		}
		if(usedItem==2){
			GameObject objMissile = (GameObject)Instantiate(sMissile, transform.position, transform.rotation);
			objMissile.GetComponent<HMissile>().terraingen = kartController.c_terrainGen;
			objMissile.GetComponent<HMissile>().waypoint = waypoint;
			objMissile.GetComponent<HMissile>().s_player = s_player;
			for(int i = 0; i < kartController.c_terrainGen.go_focalPoint.Length;i ++) {
				if(transform.gameObject == kartController.c_terrainGen.go_focalPoint[i]){
					objMissile.GetComponent<HMissile>().curWaypointNumber = kartController.c_terrainGen.i_waypoint[i];
				}
			}
			if(kartController.gameObject.GetComponent<c_AI_r1>() != null)
				objMissile.GetComponent<HMissile>().AIScript = kartController.gameObject.GetComponent<c_AI_r1>();

			heldItem=3;
		}
		if(usedItem==3){
			sMissile.GetComponent<HMissile>().detonate();
		}
		if(usedItem==4){

		}

		heldItem = 0;
	}	
}