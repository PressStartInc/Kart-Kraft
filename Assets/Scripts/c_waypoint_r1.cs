using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class c_waypoint_r1 : MonoBehaviour {
    public GameObject go_lead;
    public c_terraingen_r4 c_terraingen;
    public Vector2 v2_curPos, v2_prevPos;
    public List<Vector2> l_waypoints;
	// Use this for initialization
	void Start () {
        go_lead = c_terraingen.go_focalPoint[0];
        for(int i = 0; i < 20; i++) {
            l_waypoints.Add(new Vector2(go_lead.transform.position.x,go_lead.transform.position.z+20-i));
        }
        v2_curPos.x = v2_prevPos.x = Mathf.Floor(go_lead.transform.position.x);
		v2_curPos.y = v2_prevPos.y = Mathf.Floor(go_lead.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        v2_curPos = new Vector2(Mathf.Floor(go_lead.transform.position.x),Mathf.Floor(go_lead.transform.position.z));
        if(v2_curPos != v2_prevPos){
            //GameObject go_curWaypoint = (GameObject)Instantiate(go_waypoint,new Vector3(go_lead.transform.position.x,go_lead.transform.position.y,go_lead.transform.position.z),Quaternion.identity);
            l_waypoints.Add(new Vector2(go_lead.transform.position.x,go_lead.transform.position.z));
        } 	
        int i_waypointCount = l_waypoints.Count;
        for(int i = 0; i < c_terraingen.go_focalPoint.Length;i++) {
            int i_closestWaypoint = c_terraingen.i_waypoint[i];
            for(int j = i_closestWaypoint+1; j < i_waypointCount; j++){
                if(Vector2.Distance(l_waypoints[j],c_terraingen.v2_curPos[i]) < Vector2.Distance(l_waypoints[i_closestWaypoint],c_terraingen.v2_curPos[i])) {
                    i_closestWaypoint = j;
                }
            }
        c_terraingen.i_waypoint[i] = i_closestWaypoint;
        if(c_terraingen.i_waypoint[i] == c_terraingen.i_waypoint[c_terraingen.i_lead]) {
            Vector2 v2_challenger = new Vector2(c_terraingen.go_focalPoint[i].transform.position.x,c_terraingen.go_focalPoint[i].transform.position.z);
            Vector2 v2_leader = new Vector2(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.position.x,c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.position.z);
            if(Vector2.Distance(v2_challenger,new Vector2(v2_leader.x,(v2_leader.y-10))) < Vector2.Distance(v2_leader,new Vector2(v2_leader.x,(v2_leader.y-10)))) {
                c_terraingen.i_lead = i;
                go_lead = c_terraingen.go_focalPoint[i];
            }
        }
        }
        v2_prevPos = v2_curPos;
    } 
}
