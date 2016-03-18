using UnityEngine;
using System.Collections;

public class c_trackgen_r1 : MonoBehaviour {
	public c_terraingen_r4 c_terraingen;
    public c_envgen_r1 c_envgen;
    public c_waypoint_r1 c_waypoint;
	public GameObject go_track,go_followee;
	public GameObject go_trackHolder;
    public int i_trackCount, i_numTracks;
    public Collider co_start;
	// Use this for initialization
	void Start () {
	print("track test");
	}
	
	// Update is called once per frame
	void Update () {
	    //transform.position = c_waypoint.go_lead.transform.position;
        if(go_followee != c_waypoint.go_lead) {
            go_followee = c_waypoint.go_lead;
            //transform.localPosition = Vector3.zero;
         if(co_start.enabled) co_start.enabled = false;   
        }
    transform.position = go_followee.transform.position;
	}
	void OnTriggerExit(Collider c_collision) {
        if(c_collision.transform.name == "p_track(New)"){
            c_collision.transform.name = "p_track(Old)";
        }   
    }
	void OnTriggerEnter(Collider c_collision) {
		if(c_collision.transform.name == "p_terraincell(Clone)" || c_collision.transform.name == "p_track(Old)"){
			Vector2 v2_cellPos = new Vector2(Mathf.Floor(c_collision.transform.position.x),Mathf.Floor(c_collision.transform.position.z));
			bool b_alreadySpawned = false;
            if(c_collision.transform.name == "p_track(Old)") {
                c_terraingen.l_track.Remove(c_collision.gameObject.transform);
                i_trackCount+=-1;
                Destroy(c_collision.gameObject);
                b_alreadySpawned = false;
            }
            else {
			    for(int i = 0; i < go_trackHolder.transform.childCount;i++) {
    				if(go_trackHolder.transform.GetChild(i).transform.position.x == v2_cellPos.x &&
    					go_trackHolder.transform.GetChild(i).transform.position.z == v2_cellPos.y) b_alreadySpawned = true;
    				}
            }
			if(!b_alreadySpawned){
				Vector2[] v2_cornerPoints = new Vector2[4]; //bottom left, top right, bottom right, top left
				
				v2_cornerPoints[0] = new Vector2(c_collision.transform.position.x-c_collision.transform.localScale.x/2f,c_collision.transform.position.z-c_collision.transform.localScale.z/2f);
				v2_cornerPoints[1] = new Vector2(c_collision.transform.position.x+c_collision.transform.localScale.x/2f,c_collision.transform.position.z+c_collision.transform.localScale.z/2f);
				v2_cornerPoints[2] = new Vector2(c_collision.transform.position.x+c_collision.transform.localScale.x/2f,c_collision.transform.position.z-c_collision.transform.localScale.z/2f);
				v2_cornerPoints[3] = new Vector2(c_collision.transform.position.x-c_collision.transform.localScale.x/2f,c_collision.transform.position.z+c_collision.transform.localScale.z/2f);
				if(c_collision.transform.name == "p_terraincell(Clone)" && c_collision.transform.gameObject.activeSelf)c_collision.transform.gameObject.SetActive(false);
				GameObject go_curTrack = (GameObject)Instantiate(go_track,new Vector3(v2_cellPos.x,5,v2_cellPos.y),Quaternion.identity);
				go_curTrack.transform.Rotate(new Vector3(90,0,0));
				go_curTrack.transform.name = "p_track(New)";
				go_curTrack.transform.parent = go_trackHolder.transform;
				c_terraingen.l_track.Add(go_curTrack.transform);
                
				Mesh mesh = go_curTrack.GetComponent<MeshFilter>().mesh;
				//print(mesh.vertices[0]);
				Vector3[] vertices = mesh.vertices;
				Vector3[] normals = mesh.normals;
				
				int i_objY;
				float[] f_rawY = new float[4];
				f_rawY[0] = c_terraingen.SampleTerrain(v2_cornerPoints[0],c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
				i_objY = (int)Mathf.Floor(f_rawY[0]);
				f_rawY[1] = c_terraingen.SampleTerrain(v2_cornerPoints[1],c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
				if(i_objY > (int)Mathf.Floor(f_rawY[1])) i_objY = (int)Mathf.Floor(f_rawY[1]);
				f_rawY[2] = c_terraingen.SampleTerrain(v2_cornerPoints[2],c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
				if(i_objY > (int)Mathf.Floor(f_rawY[2])) i_objY = (int)Mathf.Floor(f_rawY[2]);
				f_rawY[3] = c_terraingen.SampleTerrain(v2_cornerPoints[3],c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
				if(i_objY > (int)Mathf.Floor(f_rawY[3])) i_objY = (int)Mathf.Floor(f_rawY[3]);
				go_curTrack.transform.position = new Vector3(go_curTrack.transform.position.x,go_curTrack.transform.position.y+i_objY,go_curTrack.transform.position.z);
                go_curTrack.GetComponent<Renderer>().material = c_envgen.m_track;            
				//float f_newVertex = c_terraingen.SampleTerrain(v2_cornerPoints[0])*c_terraingen.i_yRes;
				vertices[0] = new Vector3(vertices[0].x,vertices[0].y,-f_rawY[0]+i_objY);
				//f_newVertex = c_terraingen.SampleTerrain(v2_cornerPoints[1])*c_terraingen.i_yRes;
				vertices[1] = new Vector3(vertices[1].x,vertices[1].y,-f_rawY[1]+i_objY);
				//f_newVertex = c_terraingen.SampleTerrain(v2_cornerPoints[2])*c_terraingen.i_yRes;
				vertices[2] = new Vector3(vertices[2].x,vertices[2].y,-f_rawY[2]+i_objY);
				//f_newVertex = c_terraingen.SampleTerrain(v2_cornerPoints[3])*c_terraingen.i_yRes;
				vertices[3] = new Vector3(vertices[3].x,vertices[3].y,-f_rawY[3]+i_objY);
                mesh.MarkDynamic();
				mesh.vertices = vertices;
				go_curTrack.GetComponent<MeshCollider>().sharedMesh = null;
				go_curTrack.GetComponent<MeshCollider>().sharedMesh = mesh;
				mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                i_trackCount++;
                for(int i = 0; i < i_trackCount; i++) {
                    //c_terraingen.l_track[i].GetComponent<Renderer>().material.SetColor("_Color",c_terraingen.l_track[i].GetComponent<Renderer>().material.GetColor("_Color")*0.98f);
                    if(i > i_numTracks) {
                        GameObject go_destroyObject = c_terraingen.l_track[0].gameObject;
                        i_trackCount+=-1; 
                        c_terraingen.l_track.RemoveAt(0);
                        Destroy(go_destroyObject);
                    }   
                }
			}
		}
	}
}
