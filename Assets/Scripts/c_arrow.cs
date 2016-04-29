using UnityEngine;
using System.Collections;

public class c_arrow : MonoBehaviour {
	public c_terraingen_r6 c_terrainGen;
	public c_waypoint_r1 c_waypointGen;
	public int i_player;
	Vector2 v2_nextWaypoint;
	// Use this for initialization
	void Start () {
		//print(c_terrainGen.i_waypoint.Length + ", " + c_waypointGen.l_waypoints.Count);

	//
	}

	// Update is called once per frame
	void Update () {
	if(i_player != c_terrainGen.i_lead) {
		transform.GetChild(0).gameObject.SetActive(true);
		for(int i = 0; i < c_terrainGen.go_focalPoint.Length; i++) {
			if(c_terrainGen.i_placement[i] == (c_terrainGen.i_placement[i_player]+1))
			v2_nextWaypoint = new Vector2(c_terrainGen.go_focalPoint[i].transform.position.x,c_terrainGen.go_focalPoint[i].transform.position.z);
			}
		float f_y = transform.TransformPoint(Vector3.zero).y;
		transform.LookAt(new Vector3(v2_nextWaypoint.x,f_y,v2_nextWaypoint.y));
		}
	else {
		transform.GetChild(0).gameObject.SetActive(false);
	}
	}
}
