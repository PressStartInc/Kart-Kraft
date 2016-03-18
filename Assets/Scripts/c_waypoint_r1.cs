using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class c_waypoint_r1 : MonoBehaviour {
    public GameObject go_lead, go_waypoint;
    public c_terraingen_r4 c_terraingen;
    public Vector2 v2_curPos, v2_prevPos;
    public List<Transform> l_waypoints;
	// Use this for initialization
	void Start () {
        go_lead = c_terraingen.go_focalPoint[0];
        for(int i = 0; i < 20; i++) {
            GameObject go_curWaypoint = (GameObject)Instantiate(go_waypoint,new Vector3(go_lead.transform.position.x,go_lead.transform.position.y,go_lead.transform.position.z+20-i),Quaternion.identity);
            l_waypoints.Add(go_curWaypoint.transform);
        }
        v2_curPos.x = v2_prevPos.x = Mathf.Floor(go_lead.transform.position.x);
		v2_curPos.y = v2_prevPos.y = Mathf.Floor(go_lead.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        v2_curPos = new Vector2(Mathf.Floor(go_lead.transform.position.x),Mathf.Floor(go_lead.transform.position.z));
        if(v2_curPos != v2_prevPos){
            GameObject go_curWaypoint = (GameObject)Instantiate(go_waypoint,new Vector3(go_lead.transform.position.x,go_lead.transform.position.y,go_lead.transform.position.z),Quaternion.identity);
            l_waypoints.Add(go_curWaypoint.transform);
        } 	
        v2_prevPos = v2_curPos;
    } 
}
