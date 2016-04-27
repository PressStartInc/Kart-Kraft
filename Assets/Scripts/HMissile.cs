using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HMissile : MonoBehaviour {
	public int curWaypoint;
	public Vector2 initPos;
	public c_terraingen_r4 terraingen;
	public c_waypoint_r1 waypoint;

	public void Start(){
		initPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
		for(int i = 0; i < terraingen.go_focalPoint.Length; i++) {
			if(transform == terraingen.go_focalPoint[i].transform) {
				curWaypoint = waypoint.l_waypoints[terraingen.i_waypoint[i]];
			}
		}
	}

	void Update(){
		if(curWaypoint==waypoint.l_waypoints.Count-1||Input.GetButton("p"+s_player+"Item") detonate();
		transform.LookAt(new Vector3(waypoint.l_waypoints[++curWaypoint].x,terraingen.SampleTerrain(new Vector2(waypoint.l_waypoints[curWaypoint].x,waypoint.l_waypoints[curWaypoint].y),terraingen.f_trackRoughness)*terraingen.i_yRes+5,waypoint.l_waypoints[curWaypoint].y), Vector3.up);
		transform.Translate(transform.forward*Time.deltaTime*10);
	}

	void detonate(){
		
	}
}