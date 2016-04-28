using UnityEngine;
using System.Collections;

public class c_AI_r1 : MonoBehaviour {
	public c_terraingen_r6 c_terraingen;
	public c_racecontroller c_racecontroller;
	public c_waypoint_r1 c_waypoint;
	public bool b_randomize;
	public int i_kartRef, i_curWaypoint, i_nextWaypoint,i_waypointDirection;
	public Vector2 v2_nextWaypointPos;
	public KartController_pat1 c_kartcontroller;
	public float f_angle, f_cross, f_angleLenience, f_anglePanic;
	public bool b_start, b_sharpTurn;
	public float f_turnDuration, f_minTurnDuration,f_maxTurnDuration,f_turnTimeout, f_minTurnTimeout,f_maxTurnTimeout;
	public float f_turnDurationTimer,f_turnTimeoutTimer;
	public int i_turnDirection;
	public AIState state;
	public Transform t_debugBall;
	public bool b_useItem;
	// Use this for initialization
	public enum AIState {
		leader,
		challenger,
		follower
	}
	void Randomize(){
		f_minTurnDuration = Random.Range(0f,1f);
		f_maxTurnDuration = Random.Range(f_minTurnDuration+0.5f,f_minTurnDuration+2f);
		f_minTurnTimeout = Random.Range(0f,5f);
		f_maxTurnTimeout = Random.Range(f_minTurnTimeout+1f,f_minTurnTimeout+5f);
		f_angleLenience = Random.Range(5f,30f);
		f_anglePanic = Random.Range(f_angleLenience+5f,f_angleLenience+20f);

	}

	void Start () {
		if(b_randomize) Randomize();
		state = AIState.follower;
		c_kartcontroller.b_AI = true;
		i_turnDirection = -1;
		f_turnTimeout = Random.Range(f_minTurnTimeout,f_maxTurnTimeout);
		f_turnDuration = Random.Range(f_minTurnDuration,f_maxTurnDuration);
		b_sharpTurn = (Random.Range(0f,1f) > 0.85f) ? true : false;

		for(int i = 0; i < c_terraingen.go_focalPoint.Length; i++) {
			if(transform == c_terraingen.go_focalPoint[i].transform) {
				i_kartRef = i;
			}
		}
	}

	void MakeDecision() {}
	
	// Update is called once per frame
	void Update () {
	b_start = c_racecontroller.b_start;
	//t_debugBall.position = new Vector3(c_waypoint.l_waypoints[i_nextWaypoint].x,transform.position.y,c_waypoint.l_waypoints[i_nextWaypoint].y);
	if(b_start) {
		switch(state) {
			case AIState.leader:
				Leader();
				break;
			case AIState.challenger:
				Challenger();
				break;
			case AIState.follower:
				Follower();
				break;
			}
		}
	}
	void Leader(){

		if(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform != transform) {
			if(c_terraingen.i_placement[i_kartRef] == 1){
				c_kartcontroller.f_mMaxVelocity=c_kartcontroller.f_mMaxVelocity/0.9f;
				state = AIState.challenger;
				}
			else {
				c_kartcontroller.f_mMaxVelocity=c_kartcontroller.f_mMaxVelocity/0.9f;
				state = AIState.follower;
			}
			}
			f_turnTimeoutTimer += Time.smoothDeltaTime;
			if(f_turnTimeoutTimer > f_turnTimeout) {
				if(i_turnDirection == -1)
					i_turnDirection = (Random.Range(0f,1f) > 0.5f) ? 0 : 1;
				f_turnDurationTimer+=Time.smoothDeltaTime;
				if(b_sharpTurn && c_kartcontroller.f_mVelocity > c_kartcontroller.f_mMaxVelocity*0.1f)
					c_kartcontroller.b_AIForward = false;
				else 
					c_kartcontroller.b_AIForward = true;
				c_kartcontroller.i_AIDirection =  i_turnDirection;
				if(f_turnDurationTimer > f_turnDuration){
					f_turnTimeoutTimer = 0;
					f_turnDurationTimer = 0;
					
					f_turnTimeout = Random.Range(f_minTurnTimeout,f_maxTurnTimeout);
					f_turnDuration = Random.Range(f_minTurnDuration,f_maxTurnDuration);
					b_sharpTurn = (Random.Range(0f,1f) > 0.85f) ? true : false;
					i_turnDirection = -1;
					c_kartcontroller.i_AIDirection =  i_turnDirection;
				}

		}
		else 
			c_kartcontroller.b_AIForward = true;
	}
	void Challenger() {
		i_curWaypoint = c_terraingen.i_waypoint[i_kartRef];
		if(i_curWaypoint < c_terraingen.i_waypoint[c_terraingen.i_lead])i_nextWaypoint = i_curWaypoint+1;
		if(c_terraingen.i_placement[i_kartRef] == 0){
			c_kartcontroller.f_mMaxVelocity=c_kartcontroller.f_mMaxVelocity*0.9f;
			state = AIState.leader;
		}
		else if (c_terraingen.i_placement[i_kartRef] > 1)
			state = AIState.follower;

		if(i_curWaypoint < c_terraingen.i_waypoint[c_terraingen.i_lead]) {
		f_cross = Vector3.Cross(new Vector2(transform.TransformPoint(0,0,1).x,transform.TransformPoint(0,0,1).z)-new Vector2(transform.position.x,transform.position.z),(c_waypoint.l_waypoints[i_nextWaypoint])-new Vector2(transform.position.x,transform.position.z)).z;
			if(f_cross > 0)
			i_waypointDirection = 0; //left
		else i_waypointDirection = 1; //right			
		f_angle = Vector2.Angle(new Vector2(transform.TransformPoint(0,0,1).x,transform.TransformPoint(0,0,1).z)-new Vector2(transform.position.x,transform.position.z),c_waypoint.l_waypoints[i_nextWaypoint]-new Vector2(transform.position.x,transform.position.z));
		if(f_angle > f_angleLenience ) {
				c_kartcontroller.i_AIDirection = i_waypointDirection;
			}
		else c_kartcontroller.i_AIDirection = -1;
		if(f_angle > f_anglePanic) {
				if(c_kartcontroller.f_mVelocity > c_kartcontroller.f_mMaxVelocity*0.75f)
					c_kartcontroller.b_AIForward = false;
				else c_kartcontroller.b_AIForward = true;
			}
		else c_kartcontroller.b_AIForward = true;
		}
		else c_kartcontroller.b_AIForward = true;
	}
	void Follower(){

		i_curWaypoint = c_terraingen.i_waypoint[i_kartRef];
		i_nextWaypoint = i_curWaypoint+1;
		v2_nextWaypointPos = c_waypoint.l_waypoints[i_nextWaypoint];
		if(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform == transform){
			c_kartcontroller.f_mMaxVelocity=c_kartcontroller.f_mMaxVelocity*0.9f;
			state = AIState.leader;
		}
		if(c_terraingen.i_placement[i_kartRef] == 1){
			state = AIState.challenger;
		}

		if(i_curWaypoint < c_terraingen.i_waypoint[c_terraingen.i_lead]) {
		if(Vector3.Cross(new Vector2(transform.TransformPoint(0,0,1).x,transform.TransformPoint(0,0,1).z)-new Vector2(transform.position.x,transform.position.z),(c_waypoint.l_waypoints[i_nextWaypoint])-new Vector2(transform.position.x,transform.position.z)).z > 0)
			i_waypointDirection = 0; //left
		else i_waypointDirection = 1; //right			
		f_angle = Vector2.Angle(new Vector2(transform.TransformPoint(0,0,1).x,transform.TransformPoint(0,0,1).z)-new Vector2(transform.position.x,transform.position.z),c_waypoint.l_waypoints[i_nextWaypoint]-new Vector2(transform.position.x,transform.position.z));
		if(f_angle > f_angleLenience ) {
				c_kartcontroller.i_AIDirection = i_waypointDirection;
			}
		else c_kartcontroller.i_AIDirection = -1;
		if(f_angle > f_anglePanic) {
				if(c_kartcontroller.f_mVelocity > c_kartcontroller.f_mMaxVelocity*0.75f)
					c_kartcontroller.b_AIForward = false;
				else c_kartcontroller.b_AIForward = true;
			}
		else c_kartcontroller.b_AIForward = true;
		}
		else c_kartcontroller.b_AIForward = true;
	}
}
