using UnityEngine;
using System.Collections;

public class c_powerupSpawn : MonoBehaviour {
	public c_waypoint_r1 c_waypoint;
	public c_trackgen_r2 c_trackgen;
	public c_terraingen_r6 c_terraingen;
	public GameObject go_powerup;
	public Transform t_leader;
	public int i_spawnInterval;
	public CapsuleCollider i_trackCollider;
	public int i_powerups2Spawn;
	public float f_powerupSpread;
	public int i_respawnTime;
	public int i_prevWaypoint;
	public int i_curWaypoint;

	// Use this for initialization
	void Start () {
	i_curWaypoint = c_waypoint.l_waypoints.Count;
	i_prevWaypoint = i_curWaypoint;
	t_leader = c_terraingen.go_focalPoint[c_terraingen.i_lead].transform;
	i_powerups2Spawn = (int)Mathf.Clamp(c_terraingen.go_focalPoint.Length/4f*3f,1f,Mathf.Infinity);
	f_powerupSpread = (((float)i_trackCollider.radius*1.5f)/i_powerups2Spawn)/t_leader.localScale.x;
	i_spawnInterval = Random.Range(35,75);

	}

	// Update is called once per frame
	void Update () {
	t_leader = c_terraingen.go_focalPoint[c_terraingen.i_lead].transform;
	i_curWaypoint = c_waypoint.l_waypoints.Count;
	if(i_curWaypoint != i_prevWaypoint) {
		NextWaypoint();
	}
	i_prevWaypoint = i_curWaypoint;
	}
	void NextWaypoint(){
		if(i_curWaypoint%i_spawnInterval == 0) {
			SpawnPowerups();
		}
	}

	void SpawnPowerups() {
		for(int i = 0; i < i_powerups2Spawn; i++) {
			float f_x = -(i_trackCollider.radius)+i*f_powerupSpread;
			float f_y = c_terraingen.SampleTerrain(new Vector2(t_leader.TransformPoint(f_x,0,0).x,t_leader.TransformPoint(f_x,0,0).z),c_terraingen.f_trackRoughness)*c_terraingen.i_yRes+5.25f;
			print(f_y);
			Instantiate(go_powerup,new Vector3(t_leader.TransformPoint(f_x,0,0).x,f_y,t_leader.TransformPoint(f_x,0,0).z),Quaternion.identity);
		}
	}
}
