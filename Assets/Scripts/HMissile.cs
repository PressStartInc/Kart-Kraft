using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HMissile : MonoBehaviour {
	public int curWaypointNumber;
	public Vector2 initPos;
	public GameObject go_explosion;
	public c_terraingen_r6 terraingen;
	public c_waypoint_r1 waypoint;
	public string s_player;
	private bool b_AI;
	public c_AI_r1 AIScript;
	public float f_alive;
	public void Start(){
		initPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
	}

	void Update(){
		f_alive += Time.deltaTime;
		if(curWaypointNumber==waypoint.l_waypoints.Count-1|| (b_AI && AIScript.b_useItem) ||(!b_AI && Input.GetButtonDown("p"+s_player+"Item"))) {
			if(f_alive > 0.1f) {
				if(AIScript != null )AIScript.b_useItem = false;
				detonate();
			}
		}
		if(curWaypointNumber != waypoint.l_waypoints.Count)
			transform.LookAt(new Vector3(waypoint.l_waypoints[curWaypointNumber].x,transform.position.y,waypoint.l_waypoints[curWaypointNumber].y), Vector3.up);
		else transform.LookAt(new Vector3(terraingen.go_focalPoint[terraingen.i_lead].transform.position.x,transform.position.y,terraingen.go_focalPoint[terraingen.i_lead].transform.position.z), Vector3.up);
		transform.position = new Vector3(transform.position.x,terraingen.SampleTerrain(new Vector2(transform.position.x,transform.position.z),terraingen.f_trackRoughness)*terraingen.i_yRes+2f,transform.position.z);
		if(curWaypointNumber != waypoint.l_waypoints.Count && Vector2.Distance(waypoint.l_waypoints[curWaypointNumber],new Vector2(transform.position.x,transform.position.z)) < 5f){
			curWaypointNumber++;

		}
		else if (Vector2.Distance(new Vector2(terraingen.go_focalPoint[terraingen.i_lead].transform.position.x,terraingen.go_focalPoint[terraingen.i_lead].transform.position.z), new Vector2(transform.position.x,transform.position.z)) < 2f) {
				detonate();
			}
		transform.Translate(transform.forward*Time.deltaTime*10f);
	}

	public void detonate(){
		go_explosion.SetActive(true);
		go_explosion.transform.parent = null;
		transform.GetComponent<Collider>().enabled = true;
		StartCoroutine(Destruct());
	}
	public IEnumerator Destruct() {
		yield return null;
		Destroy(gameObject);
	}
}