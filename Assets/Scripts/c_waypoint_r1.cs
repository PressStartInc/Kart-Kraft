using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class c_waypoint_r1 : MonoBehaviour {
    public GameObject go_lead, go_winnerText, go_debug;//,go_distanceToFinish;
    public int i_maxWaypoints, i_waypointCounter;
    public int i_lowMaxWaypoints,i_hiMaxWaypoints;
    public c_terraingen_r6 c_terraingen;
    public Vector2 v2_curCell, v2_prevCell;
    public List<Vector2> l_waypoints;
    public int i_waypointDistanceCounter, i_waypointDistance;
	// Use this for initialization
	void Start () {
        i_maxWaypoints = Random.Range(i_lowMaxWaypoints,i_hiMaxWaypoints);
        go_winnerText.SetActive(false);
        go_lead = c_terraingen.go_focalPoint[0];
        i_waypointCounter = 0;
        for(int i = 0; i < 40; i++) {
            i_waypointDistanceCounter++;
                if(i_waypointDistanceCounter == i_waypointDistance) {
                l_waypoints.Add(new Vector2(go_lead.transform.position.x,go_lead.transform.position.z-40+i));
                //GameObject go_waypoint = (GameObject)Instantiate(go_debug,new Vector3(go_lead.transform.position.x,go_lead.transform.position.y,go_lead.transform.position.z-40+i),Quaternion.identity);
               // go_waypoint.transform.name = l_waypoints.Count.ToString();
                i_waypointCounter++;
                i_waypointDistanceCounter = 0;
            }
        }
        v2_curCell.x = v2_prevCell.x = Mathf.Floor(go_lead.transform.position.x);
		v2_curCell.y = v2_prevCell.y = Mathf.Floor(go_lead.transform.position.z);
	}

	// Update is called once per frame
	void Update () {
        float f_km = Mathf.Ceil(((i_maxWaypoints-i_waypointCounter)*5f)/10f)/100f;
        //go_distanceToFinish.GetComponent<UnityEngine.UI.Text>().text = "Distance to finish: " + f_km + " km";
        if(i_waypointCounter >= i_maxWaypoints) {
            go_winnerText.SetActive(true);
            //go_distanceToFinish.SetActive(false);
            go_winnerText.GetComponent<UnityEngine.UI.Text>().text = "The winner is\n Player " + (c_terraingen.i_lead+1) + "!";
            Time.timeScale = 0.0f;
        }
        v2_curCell = new Vector2(Mathf.Floor(go_lead.transform.position.x),Mathf.Floor(go_lead.transform.position.z));
        if(v2_curCell != v2_prevCell){
            i_waypointDistanceCounter++;
            if(i_waypointDistanceCounter == i_waypointDistance) {
                i_waypointDistanceCounter += -i_waypointDistance;
                //GameObject go_curWaypoint = (GameObject)Instantiate(go_waypoint,new Vector3(go_lead.transform.position.x,go_lead.transform.position.y,go_lead.transform.position.z),Quaternion.identity);
                l_waypoints.Add(new Vector2(Mathf.Floor(go_lead.transform.position.x),Mathf.Floor(go_lead.transform.position.z)));
                //GameObject go_waypoint = (GameObject)Instantiate(go_debug,go_lead.transform.position,Quaternion.identity);
               // go_waypoint.transform.name = l_waypoints.Count.ToString();
                i_waypointCounter ++;
                }
        }
        for(int i = 0; i < c_terraingen.go_focalPoint.Length;i++) {
            int i_closestWaypoint = c_terraingen.i_waypoint[i];
            for(int j = i_closestWaypoint+1; j < i_waypointCounter; j++){
                if(Vector2.Distance(l_waypoints[j],c_terraingen.v2_curCell[i]) < Vector2.Distance(l_waypoints[i_closestWaypoint],c_terraingen.v2_curCell[i])) {
                    //print(i +": " + j + " vs " + i_closestWaypoint +" -- " +Vector2.Distance(l_waypoints[j],c_terraingen.v2_curCell[i]) + " " + Vector2.Distance(l_waypoints[i_closestWaypoint],c_terraingen.v2_curCell[i]));
                    i_closestWaypoint = j;
                }
            }
        c_terraingen.i_waypoint[i] = i_closestWaypoint;
        for(int j = 0; j < c_terraingen.go_focalPoint.Length;j++) {
            if(c_terraingen.i_waypoint[i] == c_terraingen.i_waypoint[j] && i != j) {
                if(c_terraingen.i_placement[i] > c_terraingen.i_placement[j]) {
                Vector2 v2_challenger = new Vector2(c_terraingen.go_focalPoint[j].transform.TransformPoint(1,0,10).x,c_terraingen.go_focalPoint[i].transform.position.z);
                Vector2 v2_leader = new Vector2(c_terraingen.go_focalPoint[j].transform.position.x,c_terraingen.go_focalPoint[j].transform.position.z);
                Vector2 v2_comparePoint = new Vector2(c_terraingen.go_focalPoint[j].transform.TransformPoint(0,0,10).x,c_terraingen.go_focalPoint[j].transform.TransformPoint(0,0,10).z);
                float f_compareAngle = (Vector2.Angle(v2_leader-v2_challenger,v2_leader-v2_comparePoint));
//                print(c_terraingen.go_focalPoint[j].transform.name + ": " + v2_leader + " " + c_terraingen.go_focalPoint[i].transform.name + ": " + v2_challenger + " Compare: " + v2_comparePoint + " ---- " + Vector2.Angle(v2_leader-v2_challenger,v2_leader-v2_comparePoint));
                if(f_compareAngle < 90f){
                    if(c_terraingen.i_lead == j) {
                        go_lead = c_terraingen.go_focalPoint[i];
                        c_terraingen.i_lead = i;
                        }
                    int i_temp = c_terraingen.i_placement[i];
                    c_terraingen.i_placement[i] = c_terraingen.i_placement[j];
                    c_terraingen.i_placement[j] = i_temp;
                    }
                }

            }
        }/*
        if(c_terraingen.i_waypoint[i] == c_terraingen.i_waypoint[c_terraingen.i_lead] && i != c_terraingen.i_lead) {
            Vector2 v2_challenger = new Vector2(c_terraingen.go_focalPoint[i].transform.position.x,c_terraingen.go_focalPoint[i].transform.position.z);
            Vector2 v2_leader = new Vector2(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.position.x,c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.position.z);
            Vector2 v2_comparePoint = new Vector2(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.TransformPoint(0,0,10).x,c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.TransformPoint(0,0,10).z);
            //print(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.name + ": " + v2_leader + " " + c_terraingen.go_focalPoint[i].transform.name + ": " + v2_challenger + " Compare: " + v2_comparePoint + " ---- " + Vector2.Angle(v2_leader-v2_challenger,v2_leader-v2_comparePoint));
            float f_compareAngle = (Vector2.Angle(v2_leader-v2_challenger,v2_leader-v2_comparePoint));
            //if(Vector2.Distance(v2_challenger,new Vector2(v2_leader.x,(v2_leader.y-10))) < Vector2.Distance(v2_leader,new Vector2(v2_leader.x,(v2_leader.y-10)))) {
            if(f_compareAngle < 90f){
                c_terraingen.i_lead = i;
                go_lead = c_terraingen.go_focalPoint[i];
            }
        }*/
        }
        v2_prevCell = v2_curCell;
    }
}
