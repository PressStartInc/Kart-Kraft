using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HMissile : MonoBehaviour {
	public int curWaypoint;
	public Vector2 initPos;
	public c_terraingen_r4 terraingen;
	// public c_waypoint_r1 waypoint;

	void Start(){
		initPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
		for(int i = 0; i < terraingen.go_focalPoint.Length; i++) {
			if(transform == terraingen.go_focalPoint[i].transform) {
				curWaypoint = terraingen.i_waypoint[i];
			}
		}
	}

	void Update(){

	}

	void FixedUpdate(){
		transform.LookAt(new Vector3(terraingen.go_focalPoint[++curWaypoint].transform.position.x,transform.position.y,terraingen.go_focalPoint[curWaypoint].transform.position.x), Vector3.up);
		transform.Translate(transform.forward*Time.deltaTime*10);
	}
}