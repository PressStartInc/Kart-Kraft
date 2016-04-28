using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HMissile : MonoBehaviour {
	public int curWaypointNumber;
	public Vector2 initPos;
	public c_terraingen_r6 terraingen;
	public c_waypoint_r1 waypoint;
	private InvController invController;
	public string s_player;
	private bool b_AI;
	private c_AI_r1 AIScript;

	public void Start(){
		initPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
		invController = GetComponent<InvController>();
		s_player = invController.s_player;
		b_AI = invController.b_AI;
		AIScript = GetComponent<c_AI_r1>();
		for(int i = 0; i < terraingen.go_focalPoint.Length; i++) {
			if(transform == terraingen.go_focalPoint[i].transform) {
				curWaypointNumber = terraingen.i_waypoint[i];
			}
		}
	}

	void Update(){
		if(curWaypointNumber==waypoint.l_waypoints.Count-1|| (b_AI && AIScript.b_useItem) ||(!b_AI && Input.GetButton("p"+s_player+"Item"))) {
			AIScript.b_useItem = false;
			detonate();
		}
		transform.LookAt(new Vector3(waypoint.l_waypoints[++curWaypointNumber].x,terraingen.SampleTerrain(new Vector2(waypoint.l_waypoints[curWaypointNumber].x,waypoint.l_waypoints[curWaypointNumber].y),terraingen.f_trackRoughness)*terraingen.i_yRes+5,waypoint.l_waypoints[curWaypointNumber].y), Vector3.up);
		transform.Translate(transform.forward*Time.deltaTime*10);
	}

	public void detonate(){
		
	}
}