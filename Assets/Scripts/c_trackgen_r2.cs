using UnityEngine;
using System.Collections;

public class c_trackgen_r2 : MonoBehaviour {
	public c_racecontroller c_racecontroller;
	public c_terraingen_r6 c_terraingen;
	public c_envgen_r1 c_envgen;
	public c_waypoint_r1 c_waypoint;
	public int i_trackRadius;
	public int i_trackStartLength;
	public CapsuleCollider c_trackCollider;
	public GameObject go_track;
	public Vector2 v2_leadLoc, v2_prevLeadLoc;
	public GameObject go_trackHolder;
	public int i_trackCount, i_numTracks;
	public Vector2 v2_startPos;
	public Transform[,,] t_detailedTerrain;
	public bool b_startTrackPlaced;
	// Use this for initialization
	void Start () {
//	print("track test");
	c_trackCollider.radius = i_trackRadius;

	}

	// Update is called once per frame
	void Update () {
		v2_leadLoc = new Vector2(Mathf.Floor(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.position.x),Mathf.Floor(c_terraingen.go_focalPoint[c_terraingen.i_lead].transform.position.z));
		transform.position = new Vector3(v2_leadLoc.x,0,v2_leadLoc.y);
		if(c_terraingen.b_kartsPlaced) {
			if(!b_startTrackPlaced) {
				i_trackStartLength = (int)(i_trackRadius*2 + c_terraingen.go_focalPoint[0].transform.position.z - c_terraingen.go_focalPoint[c_terraingen.go_focalPoint.Length-1].transform.position.z);
				int i_detailDimension = c_terraingen.i_chunkSize*(c_terraingen.i_detailRegion*2+1);
				t_detailedTerrain = c_terraingen.t_detailedTerrain;
				for(int k = 0; k < i_trackRadius*2; k++) {
					for(int l = 0; l < i_trackStartLength; l++){
						Vector2 v2_placement = new Vector2(v2_leadLoc.x-i_trackRadius+k,v2_leadLoc.y+i_trackRadius-i_trackStartLength+l);
						GameObject go_curTrack = (GameObject)Instantiate(go_track,new Vector3(v2_placement.x,0,v2_placement.y),Quaternion.identity);
						go_curTrack.transform.name = "track";
						go_curTrack.transform.parent = c_terraingen.t_trackHolder;
						go_curTrack.transform.Rotate(new Vector3(90,0,0));
						c_terraingen.UpdateTrack(c_terraingen.i_lead,go_curTrack);
					}
				}
				b_startTrackPlaced = true;
			}/*
			else{				
				if(v2_leadLoc != v2_prevLeadLoc) {

					for(int i = 0; i < i_trackRadius*2; i++) {
						for(int j = 0; j < i_trackRadius*2; j++) {
							Vector2 v2_trackPos = new Vector2(v2_leadLoc.x-i_trackRadius+i,v2_leadLoc.y-i_trackRadius+j);
							if(Vector2.Distance(v2_trackPos,v2_leadLoc) <= (float)i_trackRadius) {
								bool b_trackExists = false;
								for(int k = 0; k < c_terraingen.v2_tracks.Count; k++) {
									if(c_terraingen.v2_tracks[k] == v2_trackPos){
										b_trackExists = true;
									}
								}
								if(!b_trackExists) {
									c_terraingen.v2_tracks.Add(v2_trackPos);
									c_terraingen.UpdateTrack(c_terraingen.i_lead,v2_trackPos);
								}
							}
						}
					}
				}
			}*/
		}
		v2_prevLeadLoc = v2_leadLoc;
	}
	void OnTriggerEnter(Collider other) {
		if(other.transform.tag == "Terrain") {
			Vector2 v2_trackPos = new Vector2(other.transform.position.x,other.transform.position.z);
			//c_terraingen.v2_tracks.Add(v2_trackPos);
			GameObject go_curTrack = (GameObject)Instantiate(go_track,new Vector3(v2_trackPos.x,0,v2_trackPos.y),Quaternion.identity);
			go_curTrack.transform.name = "track";
			go_curTrack.transform.parent = c_terraingen.t_trackHolder;
			go_curTrack.transform.Rotate(new Vector3(90,0,0));
			c_terraingen.UpdateTrack(c_terraingen.i_lead,go_curTrack);			
		}
	}
}
