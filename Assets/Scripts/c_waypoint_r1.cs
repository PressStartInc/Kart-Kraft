using UnityEngine;
using System.Collections;

public class c_waypoint_r1 : MonoBehaviour {
    public GameObject go_lead, go_waypoint;
    public c_terraingen_r4 c_terraingen;
    public Vector3 v3_curPos, v3_prevPos;
    public 
	// Use this for initialization
	void Start () {
        go_lead = c_terraingen.go_focalPoint[0];
        
	}
	
	// Update is called once per frame
	void Update () {
        v3_curPos = go_lead.transform.position;
	
    
        v3_prevPos = v3_curPos;
    } 
}
