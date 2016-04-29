using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class c_terraingen_r6 : MonoBehaviour {
	//public CapsuleCollider c_trackCollider;
	//public c_trackgen_r1 c_trackGen;
	public GameObject go_terrainObject;
	public GameObject go_detailObject;
	public GameObject go_terrainStore;
	public Material m_terrainMat;
	public Material m_trackMat;
    public GameObject[] go_focalPoint, go_playerPlacement;
    public GameObject[,] go_terrainObjects;
    public bool b_kartsPlaced;
    public int i_kartSeparation = 4;
    public int[] i_placement, i_waypoint;
	public int i_xzRes, i_yRes,i_lead;
	public int[,] i_heightmap, i_trackCount;
	public float f_blendAmount, f_trackRoughness, f_elevationSmoothHeight,f_elevationSmoothStrength;
    public bool b_Randomize = false;
	public float[] f_sampleSizes;
	private Vector2[] v2_perlinOrigins;
	public Vector2[] v2_curPos,v2_curCell,v2_prevPos,v2_prevCell, v2_direction,v2_lastPos;
	public List<Transform> l_track;
	public List<Vector2> v2_tracks;
    //public c_envgen_r1 c_envgen;
    public LayerMask lm_trackCheck,lm_terrainCheck;
    public List<Mesh> l_recalc;
    public int i_chunkSize;
    public List<Transform>[] l_chunks;
    public int i_viewDistance,i_detailRegion;
    public Transform[,,] t_detailedTerrain;
    public Transform[] t_chunkHolder;
    public Transform[] t_detailHolder;
    public Transform t_trackHolder;
	// Use this for initialization
    void Awake() {
        Time.timeScale = 1.0f;
    }
	void Start () {
        Time.timeScale = 1.0f;
        t_trackHolder = new GameObject().transform;
        t_trackHolder.name = "Track Holder";
        t_detailHolder = new Transform[go_focalPoint.Length];
        t_chunkHolder = new Transform[go_focalPoint.Length];
        t_detailedTerrain = new Transform[go_focalPoint.Length,i_chunkSize*(i_detailRegion*2+1),i_chunkSize*(i_detailRegion*2+1)];
        v2_curPos = new Vector2[go_focalPoint.Length];
        v2_curCell = new Vector2[go_focalPoint.Length];
        v2_prevPos = new Vector2[go_focalPoint.Length];
        v2_prevCell = new Vector2[go_focalPoint.Length];
		v2_lastPos = new Vector2[go_focalPoint.Length];
        v2_direction = new Vector2[go_focalPoint.Length];
        v2_perlinOrigins = new Vector2[f_sampleSizes.Length];
        l_chunks = new List<Transform>[go_focalPoint.Length];
        for(int i = 0; i < l_chunks.Length; i++) {
        	l_chunks[i] = new List<Transform>();
    	}
        i_placement = new int[go_focalPoint.Length];
        i_waypoint = new int[go_focalPoint.Length];
		//go_localBlocks = new GameObject[go_focalPoint.Length,i_xzRes,i_xzRes];
		v2_perlinOrigins[0] = new Vector2(Random.Range(0f,10f),Random.Range(0,10f));
		v2_perlinOrigins[1] = new Vector2(Random.Range(0f,10f),Random.Range(0,10f));

		if(b_Randomize) Randomize();
		for(int i = 0; i < go_focalPoint.Length; i++) {
			t_detailHolder[i] = new GameObject().transform;
        	t_detailHolder[i].name = "Player " + (i+1) + " Detail Holder";
		}

		Init();
        for(int i = 0; i < go_focalPoint.Length; i++) {
            i_placement[i] = i;
            go_focalPoint[i].transform.position = new Vector3(i_chunkSize/2f,go_focalPoint[0].transform.position.y,i_chunkSize/2f-i_kartSeparation*i);
           	go_focalPoint[i].transform.position = new Vector3(go_focalPoint[i].transform.position.x,(SampleTerrain(new Vector2(go_focalPoint[i].transform.position.x,go_focalPoint[i].transform.position.z),f_trackRoughness)*i_yRes)+5.5f,go_focalPoint[i].transform.position.z);
            v2_curPos[i].x = Mathf.Floor(go_focalPoint[i].transform.position.x/i_chunkSize);
		    v2_curPos[i].y = Mathf.Floor(go_focalPoint[i].transform.position.z/i_chunkSize);
         	t_chunkHolder[i] = new GameObject().transform;
         	t_chunkHolder[i].transform.name = "Player " + (i+1) + " chunks";   
			GenerateScenery(i);
        }
        
        b_kartsPlaced = true;
		l_track = new List<Transform>();
	}

	// Update is called once per frame
	void Init() {
		go_terrainStore = new GameObject();
		go_terrainObjects = new GameObject[i_chunkSize,i_chunkSize];
		go_terrainStore.transform.name = "terrain cells";
		
		for(int i = 0; i < i_chunkSize; i++) {
			for(int j = 0; j < i_chunkSize; j++){
				go_terrainObjects[i,j] = (GameObject)Instantiate(go_terrainObject,Vector3.zero,Quaternion.identity);
				go_terrainObjects[i,j].transform.localScale = new Vector3(1f,1f,1f);
				go_terrainObjects[i,j].transform.Rotate(new Vector3(90,0,0));
				go_terrainObjects[i,j].transform.parent = go_terrainStore.transform;
				go_terrainObjects[i,j].GetComponent<MeshRenderer>().enabled = false;
			}
		}
		for(int i = 0; i < go_focalPoint.Length; i++){
			for(int j = 0; j < i_chunkSize*(i_detailRegion*2+1); j++) {
				for(int k = 0; k < i_chunkSize*(i_detailRegion*2+1);k++) {
					Vector3 v3_terrainObjectPos = new Vector3(v2_curPos[i].x-i_chunkSize*i_detailRegion+j,0,v2_curPos[i].y-i_chunkSize*i_detailRegion+k);
					t_detailedTerrain[i,j,k] = (Transform)((GameObject)Instantiate(go_detailObject,v3_terrainObjectPos,Quaternion.identity)).transform;
					t_detailedTerrain[i,j,k].name = "terrain";
					t_detailedTerrain[i,j,k].Rotate(new Vector3(90,0,0));
					t_detailedTerrain[i,j,k].GetComponent<Renderer>().material = m_terrainMat;
					t_detailedTerrain[i,j,k].GetComponent<MeshRenderer>().enabled = true;
					t_detailedTerrain[i,j,k].parent = t_detailHolder[i];
					bool b_spotTaken = false;
					for(int l = 0; l < i; l++) {
						if(Mathf.Abs(Mathf.Floor(v3_terrainObjectPos.x/i_chunkSize)-v2_curPos[l].x) <= i_detailRegion && Mathf.Abs(Mathf.Floor(v3_terrainObjectPos.z/i_chunkSize)-v2_curPos[l].y) <= i_detailRegion){
							b_spotTaken = true;
						}
					}
					RaycastHit hit;
					if(Physics.Raycast(new Vector3(v3_terrainObjectPos.x,i_yRes,v3_terrainObjectPos.z),-Vector3.up,out hit,i_yRes*2,lm_trackCheck) || Physics.Raycast(new Vector3(v3_terrainObjectPos.x,i_yRes,v3_terrainObjectPos.z),-Vector3.up,out hit,i_yRes*2,lm_terrainCheck)){
						if(hit.transform.tag == "Terrain")
							TransformMesh(t_detailedTerrain[i,j,k].gameObject,new Vector2(v3_terrainObjectPos.x,v3_terrainObjectPos.z),1f,false);
						if(hit.transform.tag == "Track")
							b_spotTaken = true;
					}
					if(b_spotTaken)
						t_detailedTerrain[i,j,k].gameObject.SetActive(false);
				}
			}

		}
	//UpdateTerrain(0);
	}


	void UpdateTerrain(int player) {
		//OK, this is my chunk-based terrain generator aTempt
		//first check if a chunk already exists
		//Vector2 v2_playerGridPos = new Vector2(Mathf.Floor(v2_curPos[player].x/i_chunkSize),Mathf.Floor(v2_curPos[player].y/i_chunkSize));
		for(int i = 0; i < l_chunks[player].Count;i++){
			Vector2 v2_otherChunkPos = new Vector2(Mathf.Floor(l_chunks[player][i].transform.position.x/i_chunkSize),Mathf.Floor(l_chunks[player][i].transform.position.z)/i_chunkSize);
			float f_checkDistance = Vector2.Distance(v2_otherChunkPos,v2_curPos[player]);
			if(f_checkDistance > i_viewDistance){
				GameObject go_toDestroy = l_chunks[player][i].gameObject;
				l_chunks[player].RemoveAt(i);
				i--;
				Destroy(go_toDestroy);
			}
		}
		for(int i = 0; i < go_focalPoint.Length; i++) {
			for(int j = 0; j < l_chunks[i].Count; j++) {
				Vector2 v2_otherChunkPos = new Vector2(Mathf.Floor(l_chunks[i][j].transform.position.x/i_chunkSize),Mathf.Floor(l_chunks[i][j].transform.position.z)/i_chunkSize);
				if(Mathf.Abs(v2_otherChunkPos.x-v2_curPos[player].x) <= i_detailRegion && Mathf.Abs(v2_otherChunkPos.y-v2_curPos[player].y) <= i_detailRegion){
					GameObject go_toDestroy = l_chunks[i][j].gameObject;
					Vector2 v2_refPos = new Vector2((go_toDestroy.transform.position.x),(go_toDestroy.transform.position.z));
					Vector2 v2_relPos = (new Vector2(Mathf.Floor(v2_refPos.x),Mathf.Floor(v2_refPos.y))/i_chunkSize)-v2_curPos[player];
					float f_x = v2_refPos.x-v2_direction[player].x*(i_detailRegion*2+1)*i_chunkSize;
					float f_y = v2_refPos.y-v2_direction[player].y*(i_detailRegion*2+1)*i_chunkSize;
					bool b_outside = true;
					for(int k = 0; k < player+1; k++) {
//						print(v2_curPos[k] + "___ " + f_x + " -- " + Mathf.Abs(f_x-v2_curPos[k].x) + " " + f_y + " -- " + Mathf.Abs(f_y-v2_curPos[k].y));
						if(Mathf.Abs(f_x/i_chunkSize-v2_curPos[k].x) > i_detailRegion &&  Mathf.Abs(f_y/i_chunkSize-v2_curPos[k].y) > i_detailRegion){
							b_outside = false;
						}
					}
					if(b_outside) {
					Transform t_newChunk = new GameObject().transform;
					t_newChunk.gameObject.AddComponent<MeshFilter>();
					t_newChunk.gameObject.AddComponent<MeshRenderer>();
					//print("Removing: " +l_chunks[i][l_chunks[i].IndexOf(go_toDestroy.transform)].name);
					l_chunks[i].Remove(go_toDestroy.transform);
					l_chunks[i].Add(t_newChunk);
					t_newChunk.name = "c: " + Mathf.Floor(f_x/i_chunkSize) + "," + Mathf.Floor(f_y/i_chunkSize);
					t_newChunk.parent = t_chunkHolder[player];
			//print(t_newChunk.name);
					t_newChunk.position = new Vector3(f_x,0,f_y);
			//print(t_newChunk.position);
					print(f_x +", " + f_y);
					GenerateChunk(t_newChunk.gameObject, new Vector2(f_x,f_y));
					}
					else {
						l_chunks[i].Remove(go_toDestroy.transform);
						j--;
					}
					Destroy(go_toDestroy);
			//UnityEditor.EditorApplication.isPaused = true;
					SwapDetail(player,v2_direction[player], v2_refPos);
			//yield return null;
				}
			}
		}

		//StartCoroutine(GenerateScenery(player));
		GenerateScenery(player);
		
		v2_lastPos[player] = v2_prevPos[player];
	}

	void GenerateScenery(int player) {
		for(int i = 0; i < i_viewDistance*2+1;i++) {
			for(int j = 0; j < i_viewDistance*2+1;j++) {
				bool b_chunkExists = false;
				int i_x = (int)Mathf.Floor(go_focalPoint[player].transform.position.x/(float)i_chunkSize)-i_viewDistance+i;
				int i_y = (int)Mathf.Floor(go_focalPoint[player].transform.position.z/(float)i_chunkSize)-i_viewDistance+j;
//				print(v2_curPos[player] + ": " + i_x + ", " + i_y);
				Vector2 v2_chunkPos = new Vector2(i_x,i_y);
				float f_distance = 0;
				for(int k = 0; k < (player+1); k++) {
					f_distance = Vector2.Distance(v2_curPos[k],new Vector2(i-i_viewDistance,j-i_viewDistance)-v2_curPos[player]);
					//if()
				}
				
				for(int k = 0; k < (player+1); k++) {
					for(int l = 0; l < l_chunks[k].Count; l++) {
					Vector2 v2_otherChunkPos = new Vector2(Mathf.Floor(l_chunks[k][l].transform.position.x/i_chunkSize),Mathf.Floor(l_chunks[k][l].transform.position.z)/i_chunkSize);
					if(v2_chunkPos == v2_otherChunkPos || (Mathf.Abs(i_x-v2_curPos[player].x) <= i_detailRegion && Mathf.Abs(i_y-v2_curPos[player].y) <= i_detailRegion)) {
						b_chunkExists = true;
					}
				}
				}
				if(!b_chunkExists && f_distance <= i_viewDistance) {
					Transform t_newChunk = new GameObject().transform;
					t_newChunk.gameObject.AddComponent<MeshFilter>();
					t_newChunk.gameObject.AddComponent<MeshRenderer>();
					l_chunks[player].Add(t_newChunk);
					t_newChunk.name = "c: " + i_x + "," + i_y;
					t_newChunk.parent = t_chunkHolder[player];
					t_newChunk.position = new Vector3(v2_chunkPos.x*i_chunkSize,0,v2_chunkPos.y*i_chunkSize);

					GenerateChunk(t_newChunk.gameObject, new Vector2(v2_chunkPos.x*i_chunkSize,v2_chunkPos.y*i_chunkSize));
				}
			
			}
		//yield return null;		
		}
//		print(l_chunks[player].Count);
		for(int i = 0; i < i_chunkSize; i++){
			for(int j = 0; j < i_chunkSize; j++){
				go_terrainObjects[i,j].transform.parent = go_terrainStore.transform;
				go_terrainObjects[i,j].GetComponent<Renderer>().enabled = false;//SetActive(false);
			}
		}
		
	}
	
	void SwapDetail(int player, Vector2 v2_direction, Vector2 v2_refPos) {
//		print(v2_refPos);
		if(v2_direction.y != 0){
			for(int i = 0; i < i_chunkSize*(i_detailRegion*2+1);i++){
				for(int j = 0; j < i_chunkSize*(i_detailRegion*2+1);j++) {
					Vector2 v2_newPos;
					bool b_track = false;
					if(v2_direction.y == 1) {
						if(t_detailedTerrain[player,i,j].position.z < Mathf.Floor(v2_refPos.y-i_chunkSize*(i_detailRegion*2)) &&
							t_detailedTerrain[player,i,j].position.x >= Mathf.Floor(v2_refPos.x) && t_detailedTerrain[player,i,j].position.x < Mathf.Floor(v2_refPos.x)+i_chunkSize){
							v2_newPos = new Vector2(t_detailedTerrain[player,i,j].position.x,t_detailedTerrain[player,i,j].position.z+i_chunkSize*(i_detailRegion*2+1));
							t_detailedTerrain[player,i,j].position = new Vector3(v2_newPos.x,0,v2_newPos.y);
							bool b_spotTaken = false;
							for(int k = 0; k < player; k++) {
								if(Mathf.Abs(v2_newPos.x/i_chunkSize-v2_curPos[k].x) <= i_detailRegion && Mathf.Abs(v2_newPos.y/i_chunkSize-v2_curPos[k].y) <= i_detailRegion){
									b_spotTaken = true;
								}
							}
							RaycastHit hit;
							if(b_spotTaken || Physics.Raycast(new Vector3(v2_newPos.x,i_yRes,v2_newPos.y),-Vector3.up,out hit,i_yRes,lm_trackCheck)){
									if(t_detailedTerrain[player,i,j].gameObject.activeSelf)
										t_detailedTerrain[player,i,j].gameObject.SetActive(false);
								}
							else {
								if(!t_detailedTerrain[player,i,j].gameObject.activeSelf)
									t_detailedTerrain[player,i,j].gameObject.SetActive(true);
								TransformMesh(t_detailedTerrain[player,i,j].gameObject,v2_newPos,1f,false);
								}
						}
					}
					else if(v2_direction.y == -1) {
						if(t_detailedTerrain[player,i,j].position.z >= Mathf.Floor(v2_refPos.y+i_chunkSize*(i_detailRegion*2+1)) &&
							t_detailedTerrain[player,i,j].position.x >= Mathf.Floor(v2_refPos.x) && t_detailedTerrain[player,i,j].position.x < Mathf.Floor(v2_refPos.x)+i_chunkSize){
							v2_newPos = new Vector2(t_detailedTerrain[player,i,j].position.x,t_detailedTerrain[player,i,j].position.z-i_chunkSize*(i_detailRegion*2+1));
							t_detailedTerrain[player,i,j].position = new Vector3(v2_newPos.x,0,v2_newPos.y);	
							bool b_spotTaken = false;
							for(int k = 0; k < player; k++) {
								if(Mathf.Abs(v2_newPos.x/i_chunkSize-v2_curPos[k].x) <= i_detailRegion && Mathf.Abs(v2_newPos.y/i_chunkSize-v2_curPos[k].y) <= i_detailRegion){
									b_spotTaken = true;
								}
							}
							RaycastHit hit;
							if(b_spotTaken || Physics.Raycast(new Vector3(v2_newPos.x,i_yRes,v2_newPos.y),-Vector3.up,out hit,i_yRes,lm_trackCheck)){
									if(t_detailedTerrain[player,i,j].gameObject.activeSelf)
										t_detailedTerrain[player,i,j].gameObject.SetActive(false);
								}
							else {
								if(!t_detailedTerrain[player,i,j].gameObject.activeSelf)
									t_detailedTerrain[player,i,j].gameObject.SetActive(true);
								TransformMesh(t_detailedTerrain[player,i,j].gameObject,v2_newPos,1f,false);
								}
						}
					}
				}
			}
		}
		if(v2_direction.x != 0) {
//			print(v2_refPos + " " +Mathf.Floor(v2_refPos.x+i_chunkSize*(i_detailRegion*2+1)));
			for(int i = 0; i < i_chunkSize*(i_detailRegion*2+1);i++){
				for(int j = 0; j < i_chunkSize*(i_detailRegion*2+1);j++) {
					Vector2 v2_newPos;
					bool b_track = false;
					if(v2_direction.x == 1) {
						if(t_detailedTerrain[player,i,j].position.x < Mathf.Floor(v2_refPos.x-i_chunkSize*(i_detailRegion*2)) &&
							t_detailedTerrain[player,i,j].position.z >= Mathf.Floor(v2_refPos.y) && t_detailedTerrain[player,i,j].position.z < Mathf.Floor(v2_refPos.y)+i_chunkSize){
							v2_newPos = new Vector2(t_detailedTerrain[player,i,j].position.x+i_chunkSize*(i_detailRegion*2+1),t_detailedTerrain[player,i,j].position.z);
							t_detailedTerrain[player,i,j].position = new Vector3(v2_newPos.x,0,v2_newPos.y);	
							bool b_spotTaken = false;
							for(int k = 0; k < player; k++) {
								if(Mathf.Abs(v2_newPos.x/i_chunkSize-v2_curPos[k].x) <= i_detailRegion && Mathf.Abs(v2_newPos.y/i_chunkSize-v2_curPos[k].y) <= i_detailRegion){
									b_spotTaken = true;
								}
							}
							RaycastHit hit;
							if(b_spotTaken || Physics.Raycast(new Vector3(v2_newPos.x,i_yRes,v2_newPos.y),-Vector3.up,out hit,i_yRes,lm_trackCheck)){
									if(t_detailedTerrain[player,i,j].gameObject.activeSelf)
										t_detailedTerrain[player,i,j].gameObject.SetActive(false);
								}
							else {
								if(!t_detailedTerrain[player,i,j].gameObject.activeSelf)
									t_detailedTerrain[player,i,j].gameObject.SetActive(true);
								TransformMesh(t_detailedTerrain[player,i,j].gameObject,v2_newPos,1f,false);
								}
						}
					}
					else if(v2_direction.x == -1) {
						if(t_detailedTerrain[player,i,j].position.x >= Mathf.Floor(v2_refPos.x+i_chunkSize*(i_detailRegion*2+1)) &&
							t_detailedTerrain[player,i,j].position.z >= Mathf.Floor(v2_refPos.y) && t_detailedTerrain[player,i,j].position.z < Mathf.Floor(v2_refPos.y)+i_chunkSize){						
							v2_newPos = new Vector2(t_detailedTerrain[player,i,j].position.x-i_chunkSize*(i_detailRegion*2+1),t_detailedTerrain[player,i,j].position.z);
							t_detailedTerrain[player,i,j].position = new Vector3(v2_newPos.x,0,v2_newPos.y);	
							bool b_spotTaken = false;
							for(int k = 0; k < player; k++) {
								if(Mathf.Abs(v2_newPos.x/i_chunkSize-v2_curPos[k].x) <= i_detailRegion && Mathf.Abs(v2_newPos.y/i_chunkSize-v2_curPos[k].y) <= i_detailRegion){
									b_spotTaken = true;
								}
							}
							RaycastHit hit;
							if(b_spotTaken || Physics.Raycast(new Vector3(v2_newPos.x,i_yRes,v2_newPos.y),-Vector3.up,out hit,i_yRes,lm_trackCheck)){
									if(t_detailedTerrain[player,i,j].gameObject.activeSelf)
										t_detailedTerrain[player,i,j].gameObject.SetActive(false);
								}
							else {
								if(!t_detailedTerrain[player,i,j].gameObject.activeSelf)
									t_detailedTerrain[player,i,j].gameObject.SetActive(true);
								TransformMesh(t_detailedTerrain[player,i,j].gameObject,v2_newPos,1f,false);
								}
						}
					}

				}
			}
		}
	}


	public void UpdateTrack(int i_player, GameObject go_track) {
		
		Vector2 v2_trackPos = new Vector2(go_track.transform.position.x,go_track.transform.position.z);
		TransformMesh(go_track,new Vector2(go_track.transform.position.x,go_track.transform.position.z),1f,true);
		RaycastHit hit;
		if(Physics.Raycast(new Vector3(v2_trackPos.x,i_yRes,v2_trackPos.y),-Vector3.up,out hit,i_yRes,lm_terrainCheck)){
			if(hit.transform.gameObject.activeSelf)
				hit.transform.gameObject.SetActive(false);
			}
			//t_detailedTerrain[i_player,i,j].GetComponent<Renderer>().material.color = Color.red
		for(int i = (int)v2_trackPos.x-1; i < (int)v2_trackPos.x+2; i++) {
			for(int j = (int)v2_trackPos.y-1; j < (int)v2_trackPos.y+2; j++) {
				//hit = null;
				if(!Physics.Raycast(new Vector3(i,i_yRes,j),-Vector3.up,out hit,i_yRes,lm_trackCheck) && Physics.Raycast(new Vector3(i,i_yRes,j),-Vector3.up,out hit,i_yRes,lm_terrainCheck)){
					TransformMesh(hit.transform.gameObject,new Vector2(i,j),1f,false);
					}
				}
			}
		}


		void GenerateChunk(GameObject go_chunk, Vector2 v2_chunkPos) {
			for(int i = 0; i < i_chunkSize; i+=1) {
				for(int j = 0; j < i_chunkSize; j+=1) {
					int i_x = (int)v2_chunkPos.x+i;
					int i_y = (int)v2_chunkPos.y+j;
					go_terrainObjects[i,j].transform.parent = go_chunk.transform;
					go_terrainObjects[i,j].transform.position = new Vector3(i,0,j);				
					if(!go_terrainObjects[i,j].activeSelf)go_terrainObjects[i,j].SetActive(true);
				}
			}
			for(int i = 0; i < i_chunkSize; i+=1) {
				for(int j = 0; j < i_chunkSize; j+=1) {
					int i_x = (int)v2_chunkPos.x+i;
					int i_y = (int)v2_chunkPos.y+j;
					bool b_track = false;

					RaycastHit hit;
					if(!Physics.Raycast(new Vector3(i_x,i_yRes,i_y),-Vector3.up,out hit, i_yRes, lm_trackCheck)){
						go_terrainObjects[i,j].SetActive(true);
						TransformMesh(go_terrainObjects[i,j],new Vector2(i_x,i_y),1f,false);
					}
					else{
						//go_terrainObjects[i,j].transform.position = new Vector3(i,-10,j);				
						go_terrainObjects[i,j].SetActive(false);
					}
				}
			}
			MeshFilter[] meshFilters = go_chunk.GetComponentsInChildren<MeshFilter>();
			CombineInstance[] combine = new CombineInstance[meshFilters.Length-1];
			for(int i = 1; i < meshFilters.Length; i++) {
				combine[i-1].mesh = meshFilters[i].sharedMesh;
				combine[i-1].transform = meshFilters[i].transform.localToWorldMatrix;
			}
			go_chunk.GetComponent<MeshFilter>().mesh = new Mesh();
			go_chunk.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
			l_recalc.Add(go_chunk.GetComponent<MeshFilter>().mesh);
			go_chunk.GetComponent<MeshRenderer>().material = m_terrainMat;
		}

	public void Randomize() {
		i_yRes = Random.Range(i_xzRes/2,i_xzRes);
		f_blendAmount = Random.Range(0.5f,0.95f);
		f_trackRoughness = Random.Range(f_blendAmount*0.5f,f_blendAmount);
		f_elevationSmoothHeight = Random.Range(0.0f,0.8f);
		f_elevationSmoothStrength = Random.Range(0f,0.95f);
		f_sampleSizes[0] = Random.Range(0f,2f);
		f_sampleSizes[1] = Random.Range(5f,10f);

	}


	public float SampleTerrain(Vector2 v2_position, float f_blend) {
		Vector2[] v2_perlinPos = new Vector2[2];
		v2_perlinPos[0].x = (v2_position.x/(float)i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[0].y = (v2_position.y/(float)i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[1].x = (v2_position.x/(float)i_xzRes)*f_sampleSizes[1];
		v2_perlinPos[1].y = (v2_position.y/(float)i_xzRes)*f_sampleSizes[1];
		Vector2 v2_samplePos = new Vector2(v2_perlinOrigins[0].x+v2_perlinPos[0].x,v2_perlinOrigins[0].y+v2_perlinPos[0].y);
		float f_height = Mathf.PerlinNoise(v2_samplePos.x,v2_samplePos.y);
		if(f_height < f_elevationSmoothHeight) f_height = f_height*(1-f_elevationSmoothStrength);
		//else f_height = Mathf.Lerp(f_height*(1-f_elevationSmoothStrength),f_height,(f_height-(f_elevationSmoothHeight)/(1f-f_elevationSmoothHeight))*(1f-f_elevationSmoothStrength));
        else f_height = Mathf.Lerp(f_height*(1-f_elevationSmoothStrength),f_height,(f_height-f_elevationSmoothHeight)/(1f-f_elevationSmoothHeight));

		v2_samplePos = new Vector2(v2_perlinOrigins[1].x+v2_perlinPos[1].x,v2_perlinOrigins[1].y+v2_perlinPos[1].y);
		float f_height2 = Mathf.PerlinNoise(v2_samplePos.x,v2_samplePos.y);

		if(f_height2 < f_elevationSmoothHeight) f_height2 = f_height2*(1-f_elevationSmoothStrength);
		else f_height2 = Mathf.Lerp(f_height2*(1-f_elevationSmoothStrength),f_height2,f_height2-(f_elevationSmoothHeight)/(1f-f_elevationSmoothHeight));
		//Average Blend
        //float f_finalHeight = Mathf.Lerp(f_height,f_height2,f_blend/100f);

        //Addivitve Blend
        float f_finalHeight = Mathf.Clamp(f_height+f_height2*f_blend*0.2f,0f,1f);
		//print((v2_samplePos + ":: " + f_height + " + " + f_height2 + " = " + f_finalHeight));
		return f_finalHeight;
	}

	public int TransformMesh(GameObject go_curCell, Vector2 v2_cellPos,float f_cellSize,bool b_track){
				Vector2[] v2_cornerPoints = new Vector2[4]; //boTom left, top right, boTom right, top left

				v2_cornerPoints[0] = new Vector2(v2_cellPos.x-f_cellSize/2f,v2_cellPos.y-f_cellSize/2f);
				v2_cornerPoints[1] = new Vector2(v2_cellPos.x+f_cellSize/2f,v2_cellPos.y+f_cellSize/2f);
				v2_cornerPoints[2] = new Vector2(v2_cellPos.x+f_cellSize/2f,v2_cellPos.y-f_cellSize/2f);
				v2_cornerPoints[3] = new Vector2(v2_cellPos.x-f_cellSize/2f,v2_cellPos.y+f_cellSize/2f);

				Mesh mesh = go_curCell.GetComponent<MeshFilter>().mesh;
				//print(mesh.vertices[0]);
				Vector3[] vertices = mesh.vertices;
				Vector3[] normals = mesh.normals;

				int i_objY;
				float[] f_rawY = new float[4];
				if(b_track) {
					f_rawY[0] = (SampleTerrain(v2_cornerPoints[0],f_trackRoughness)*i_yRes);
					i_objY = (int)Mathf.Floor(f_rawY[0]);
					f_rawY[1] = (SampleTerrain(v2_cornerPoints[1],f_trackRoughness)*i_yRes);
					if(i_objY > (int)Mathf.Floor(f_rawY[1])) i_objY = (int)Mathf.Floor(f_rawY[1]);
					f_rawY[2] = (SampleTerrain(v2_cornerPoints[2],f_trackRoughness)*i_yRes);
					if(i_objY > (int)Mathf.Floor(f_rawY[2])) i_objY = (int)Mathf.Floor(f_rawY[2]);
					f_rawY[3] = (SampleTerrain(v2_cornerPoints[3],f_trackRoughness)*i_yRes);
					if(i_objY > (int)Mathf.Floor(f_rawY[3])) i_objY = (int)Mathf.Floor(f_rawY[3]);
				}
				else {
					
					bool[,] b_trackNearby = new bool[3,3];
					if(go_curCell.transform.tag == "Terrain") {
						for(int i = 0; i < 3; i++) {
							for(int j = 0; j < 3; j++) {
							RaycastHit hit;
							if(i != 1 || j != 1) {
								int i_x = (int)(i-1+v2_cellPos.x);
								int i_y = (int)(j-1+v2_cellPos.y);
								if(Physics.Raycast(new Vector3(i_x,i_yRes,i_y),-Vector3.up,out hit,i_yRes,lm_trackCheck)){

									b_trackNearby[i,j] = true;
									}							
								}
							}
						}
					}
					/*
					for(int i = 0; i < v2_tracks.Count; i++) {
						if(v2_tracks[i]-v2_cellPos == new Vector2(-1,-1)) {
							b_trackNearby[0,0] = true;
						} else if(v2_tracks[i]-v2_cellPos == new Vector2(-1,0)) {
							b_trackNearby[0,1] = true;
						} else if(v2_tracks[i]-v2_cellPos == new Vector2(-1,1)) {
							b_trackNearby[0,2] = true;					
						} else if(v2_tracks[i]-v2_cellPos == new Vector2(0,-1)) {
							b_trackNearby[1,0] = true;							
						} else if(v2_tracks[i]-v2_cellPos == new Vector2(0,1)) {
							b_trackNearby[1,2] = true;							
						} else if(v2_tracks[i]-v2_cellPos == new Vector2(1,-1)) {
							b_trackNearby[2,0] = true;							
						} else if(v2_tracks[i]-v2_cellPos == new Vector2(1,0)) {
							b_trackNearby[2,1] = true;							
						} else if(v2_tracks[i]-v2_cellPos == new Vector2(1,1)) {
							b_trackNearby[2,2] = true;							
						}
					}*/

					if(b_trackNearby[0,0] || b_trackNearby[0,1] || b_trackNearby[1,0])
						f_rawY[0] = (SampleTerrain(v2_cornerPoints[0],f_trackRoughness)*i_yRes);
					else
						f_rawY[0] = Mathf.Ceil(SampleTerrain(v2_cornerPoints[0],f_blendAmount)*i_yRes);
					i_objY = (int)Mathf.Floor(f_rawY[0]);

					if(b_trackNearby[1,2] || b_trackNearby[2,1] || b_trackNearby[2,2])
						f_rawY[1] = (SampleTerrain(v2_cornerPoints[1],f_trackRoughness)*i_yRes);
					else
						f_rawY[1] = Mathf.Ceil(SampleTerrain(v2_cornerPoints[1],f_blendAmount)*i_yRes);
					if(i_objY > (int)Mathf.Floor(f_rawY[1])) i_objY = (int)Mathf.Floor(f_rawY[1]);

					if(b_trackNearby[1,0] || b_trackNearby[2,0] || b_trackNearby[2,1])
						f_rawY[2] = (SampleTerrain(v2_cornerPoints[2],f_trackRoughness)*i_yRes);
					else
						f_rawY[2] = Mathf.Ceil(SampleTerrain(v2_cornerPoints[2],f_blendAmount)*i_yRes);
					if(i_objY > (int)Mathf.Floor(f_rawY[2])) i_objY = (int)Mathf.Floor(f_rawY[2]);
					
					if(b_trackNearby[0,1] || b_trackNearby[0,2] || b_trackNearby[1,2]) 
						f_rawY[3] = (SampleTerrain(v2_cornerPoints[3],f_trackRoughness)*i_yRes);
					else 
						f_rawY[3] = Mathf.Ceil(SampleTerrain(v2_cornerPoints[3],f_blendAmount)*i_yRes);
					if(i_objY > (int)Mathf.Floor(f_rawY[3])) i_objY = (int)Mathf.Floor(f_rawY[3]);							
				}
				//i_objY = 0;
				//go_curCell.transform.position = new Vector3(go_curCell.transform.position.x,go_curCell.transform.position.y+i_objY,go_curCell.transform.position.z);
                //go_curCell.GetComponent<Renderer>().material = c_envgen.m_trackMat;
				//float f_newVertex = SampleTerrain(v2_cornerPoints[0])*i_yRes;
				vertices[0] = new Vector3(vertices[0].x,vertices[0].y,-f_rawY[0]);//+i_objY);
				//f_newVertex = SampleTerrain(v2_cornerPoints[1])*i_yRes;
				vertices[1] = new Vector3(vertices[1].x,vertices[1].y,-f_rawY[1]);
				//f_newVertex = SampleTerrain(v2_cornerPoints[2])*i_yRes;
				vertices[2] = new Vector3(vertices[2].x,vertices[2].y,-f_rawY[2]);
				//f_newVertex = SampleTerrain(v2_cornerPoints[3])*i_yRes;
				vertices[3] = new Vector3(vertices[3].x,vertices[3].y,-f_rawY[3]);
                mesh.MarkDynamic();
				mesh.vertices = vertices;
				//go_curCell.GetComponent<MeshCollider>().sharedMesh = null;
				//go_curCell.GetComponent<MeshCollider>().sharedMesh = mesh;
				mesh.RecalculateBounds();
				if(go_curCell.name == "terrain" || go_curCell.name == "track")
					mesh.RecalculateNormals();					
                return 0;
	}
	void Update () {
		for(int i = 0; i < Mathf.Ceil(l_recalc.Count/10f); i++) {
			if(l_recalc.Count > 0) {
            	l_recalc[0].RecalculateNormals();
            	l_recalc.RemoveAt(0);
            }
		}
		if(Input.GetKeyUp(KeyCode.F1)) {
            //Time.timeScale = 1.0f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        bool b_update = false;
        for(int i = 0; i < go_focalPoint.Length;i++) {
        	v2_curCell[i] = new Vector2(Mathf.Floor(go_focalPoint[i].transform.position.x),Mathf.Floor(go_focalPoint[i].transform.position.z));
        	v2_curPos[i] = new Vector2(Mathf.Floor(go_focalPoint[i].transform.position.x/(float)i_chunkSize),Mathf.Floor(go_focalPoint[i].transform.position.z/(float)i_chunkSize));
        	if(v2_curPos[i] != v2_prevPos[i]) {
        		v2_direction[i] = (v2_curPos[i]-v2_prevPos[i]);
        		UpdateTerrain(i);
        	}
        	v2_prevPos[i] = v2_curPos[i];
        	v2_prevCell[i] = v2_curCell[i];
        }
        if(b_update) {
        	for(int i = 0; i < go_focalPoint.Length;i++){
        		
        	}
        }
	}
}
