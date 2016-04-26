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
	if(c_terrainGen.i_waypoint[i_player] != c_waypointGen.l_waypoints.Count-1) {
		transform.GetChild(0).gameObject.SetActive(true);
		v2_nextWaypoint = c_waypointGen.l_waypoints[c_terrainGen.i_waypoint[i_player]+1];
		for(int i = c_terrainGen.i_waypoint[i_player]+1; i < c_waypointGen.l_waypoints.Count; i++) {
			if(Vector2.Distance(new Vector2(c_terrainGen.go_focalPoint[i_player].transform.position.x,c_terrainGen.go_focalPoint[i_player].transform.position.z),c_waypointGen.l_waypoints[i]) <
				Vector2.Distance(new Vector2(c_terrainGen.go_focalPoint[i_player].transform.position.x,c_terrainGen.go_focalPoint[i_player].transform.position.z),v2_nextWaypoint)) {
				v2_nextWaypoint = c_waypointGen.l_waypoints[i];
				}
			}
		float f_y = transform.TransformPoint(Vector3.zero).y;
		transform.LookAt(new Vector3(v2_nextWaypoint.x,f_y,v2_nextWaypoint.y));
		}
	else {
		transform.GetChild(0).gameObject.SetActive(false);
	}
	}
}
