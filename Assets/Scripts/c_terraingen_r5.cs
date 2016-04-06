using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class c_terraingen_r5 : MonoBehaviour {
	//public CapsuleCollider c_trackCollider;
	//public c_trackgen_r1 c_trackGen;
	public GameObject go_terrainObject;
	public GameObject go_terrainStore;
	public Material m_terrainMat;
    public GameObject[] go_focalPoint, go_playerPlacement;
    public GameObject[,] go_terrainObjects;
    public int[] i_placement, i_waypoint;
	public int i_xzRes, i_yRes,i_lead;
	public int[,] i_heightmap, i_trackCount;
	public float f_blendAmount, f_trackRoughness, f_elevationSmoothHeight,f_elevationSmoothStrength;
    public bool b_Randomize = false;
	public float[] f_sampleSizes;
	private Vector2[] v2_perlinOrigins;
	public Vector2[] v2_curPos, v2_prevPos;
	public List<Transform> l_track;
    //public c_envgen_r1 c_envgen;
    public LayerMask lm_terrainCheck;

    public int i_chunkSize;
    public List<Transform>[] l_chunks;
    public int i_viewDistance;
	// Use this for initialization
    void Awake() {
        Time.timeScale = 1.0f;
    }
	void Start () {
		Init();
        Time.timeScale = 1.0f;
        v2_curPos = new Vector2[go_focalPoint.Length];
        v2_prevPos = new Vector2[go_focalPoint.Length];
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
            i_placement[i] = i;
            go_focalPoint[i].transform.position = new Vector3(i_chunkSize/2f,go_focalPoint[0].transform.position.y,i_chunkSize/2f-4*i);
           	go_focalPoint[i].transform.position = new Vector3(go_focalPoint[i].transform.position.x,(SampleTerrain(new Vector2(go_focalPoint[i].transform.position.x,go_focalPoint[i].transform.position.z),f_trackRoughness)*i_yRes)+5.5f,go_focalPoint[i].transform.position.z);
            v2_curPos[i].x = Mathf.Floor(go_focalPoint[i].transform.position.x);
		    v2_curPos[i].y = Mathf.Floor(go_focalPoint[i].transform.position.z);
            UpdateTerrain(i);    
        }
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
				go_terrainObjects[i,j].transform.parent = go_terrainStore.transform;
				go_terrainObjects[i,j].active = false;
			}
		}
		print(go_terrainObjects[0,0].transform.name);
	}
	void Update () {
        
		if(Input.GetKeyUp(KeyCode.F1)) {
            //Time.timeScale = 1.0f;
            Application.LoadLevel(Application.loadedLevel);
        }
        for(int i = 0; i < go_focalPoint.Length;i++) {
        	v2_curPos[i] = new Vector2(Mathf.Floor(go_focalPoint[i].transform.position.x/(float)i_chunkSize),Mathf.Floor(go_focalPoint[i].transform.position.z/(float)i_chunkSize));
        	if(v2_curPos[i] != v2_prevPos[i]) {
        		UpdateTerrain(i);
        	}
        	v2_prevPos[i] = v2_curPos[i];
        }
	}

	void UpdateTerrain(int player) {
		//OK, this is my chunk-based terrain generator attempt
		//first check if a chunk already exists
		for(int i = 0; i < i_viewDistance*2+1;i++) {
			for(int j = 0; j < i_viewDistance*2+1;j++) {
				bool b_chunkExists = false;
				int i_x = (int)Mathf.Floor(go_focalPoint[player].transform.position.x/(float)i_chunkSize)-i_viewDistance+i;
				int i_y = (int)Mathf.Floor(go_focalPoint[player].transform.position.z/(float)i_chunkSize)-i_viewDistance+j;
				Vector2 v2_chunkPos = new Vector2(i_x*i_chunkSize,i_y*i_chunkSize);
				float f_distance = Vector2.Distance(new Vector2((Mathf.Floor(go_focalPoint[player].transform.position.x/(float)i_chunkSize))*i_chunkSize,(Mathf.Floor(go_focalPoint[player].transform.position.z/(float)i_chunkSize))*i_chunkSize),v2_chunkPos)/(float)i_chunkSize;
				//print(f_distance);
				for(int k = 0; k < l_chunks[player].Count; k++) {					
					if(v2_chunkPos == new Vector2(l_chunks[player][k].transform.position.x,l_chunks[player][k].transform.position.z) || f_distance > i_viewDistance) {
						b_chunkExists = true;
					}
					float f_checkDistance = Vector2.Distance(new Vector2((Mathf.Floor(go_focalPoint[player].transform.position.x/(float)i_chunkSize))*i_chunkSize,(Mathf.Floor(go_focalPoint[player].transform.position.z/(float)i_chunkSize))*i_chunkSize),new Vector2(l_chunks[player][k].transform.position.x,l_chunks[player][k].transform.position.z))/(float)i_chunkSize;
					//print(f_checkDistance);
					if(f_checkDistance > i_viewDistance){
						GameObject go_toDestroy = l_chunks[player][k].gameObject;
						l_chunks[player].Remove(l_chunks[player][k]);
						Destroy(go_toDestroy);
					}
				}
				if(!b_chunkExists) {
					Transform t_newChunk = new GameObject().transform;
					t_newChunk.gameObject.AddComponent<MeshFilter>();
					t_newChunk.gameObject.AddComponent<MeshRenderer>();
					l_chunks[player].Add(t_newChunk);
					t_newChunk.name = "c: " + i_x + "," + i_y;
					t_newChunk.position = new Vector3(v2_chunkPos.x,0,v2_chunkPos.y);
					GenerateChunk(t_newChunk.gameObject, v2_chunkPos);
				}
			}
		}
		for(int i = 0; i < i_chunkSize; i++){
			for(int j = 0; j < i_chunkSize; j++){
				go_terrainObjects[i,j].transform.parent = go_terrainStore.transform;
				go_terrainObjects[i,j].active = false;	
			}
		}
	}

	void GenerateChunk(GameObject go_chunk, Vector2 v2_chunkPos) {
		for(int i = 0; i < i_chunkSize; i++) {
			for(int j = 0; j < i_chunkSize; j++) {
				int i_x = (int)v2_chunkPos.x+i;
				int i_y = (int)v2_chunkPos.y+j;
				if(!go_terrainObjects[i,j].active)go_terrainObjects[i,j].active = true;
				go_terrainObjects[i,j].transform.parent = go_chunk.transform;
				go_terrainObjects[i,j].transform.position = new Vector3(i,Mathf.Ceil(SampleTerrain(new Vector2(i_x,i_y),f_blendAmount)*i_yRes),j);
			}
		//yield return null;
		}
		MeshFilter[] meshFilters = go_chunk.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length-1];
		for(int i = 1; i < meshFilters.Length; i++) {
            combine[i-1].mesh = meshFilters[i].sharedMesh;
            combine[i-1].transform = meshFilters[i].transform.localToWorldMatrix;
            //meshFilters[i].gameObject.transform.parent = go_terrainStore.transform;
            //meshFilters[i].gameObject.active = false;
        }
        //print("combining!");
       	go_chunk.GetComponent<MeshFilter>().mesh = new Mesh();
        go_chunk.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);		
        go_chunk.GetComponent<MeshRenderer>().material = m_terrainMat;
        //go_chunk.active = true;
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
}


