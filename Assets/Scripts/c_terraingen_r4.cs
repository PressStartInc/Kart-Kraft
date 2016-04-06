using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class c_terraingen_r4 : MonoBehaviour {
	public CapsuleCollider c_trackCollider;
	public c_trackgen_r1 c_trackGen;
	public GameObject go_block;
    public GameObject[] go_focalPoint, go_playerPlacement;
	public GameObject[,,] go_localBlocks;
    public int[] i_placement, i_waypoint;
	public int i_xzRes, i_yRes,i_lead;
	public int[,] i_heightmap, i_trackCount;
	public float f_blendAmount, f_trackRoughness, f_elevationSmoothHeight,f_elevationSmoothStrength;
    public bool b_Randomize = false;
	public float[] f_sampleSizes;
	private Vector2[] v2_perlinOrigins;
	public Vector2[] v2_curPos, v2_prevPos;
	public List<Transform> l_track;
    public c_envgen_r1 c_envgen;
    public LayerMask lm_terrainCheck;
	// Use this for initialization
    void Awake() {
        Time.timeScale = 1.0f;
    }
	void Start () {
        Time.timeScale = 1.0f;
        v2_curPos = new Vector2[go_focalPoint.Length];
        v2_prevPos = new Vector2[go_focalPoint.Length];
        v2_perlinOrigins = new Vector2[f_sampleSizes.Length];
        i_placement = new int[go_focalPoint.Length];
        i_waypoint = new int[go_focalPoint.Length];
		go_localBlocks = new GameObject[go_focalPoint.Length,i_xzRes,i_xzRes];
		v2_perlinOrigins[0] = new Vector2(Random.Range(0f,10f),Random.Range(0,10f));
		v2_perlinOrigins[1] = new Vector2(Random.Range(0f,10f),Random.Range(0,10f));
		
		if(b_Randomize) Randomize();
        for(int i = 0; i < go_focalPoint.Length; i++) {
            i_placement[i] = i;
            go_focalPoint[i].transform.position = new Vector3(go_focalPoint[0].transform.position.x+(i%2f)*2f,go_focalPoint[0].transform.position.y,go_focalPoint[0].transform.position.z-4*i);
           	go_focalPoint[i].transform.position = new Vector3(go_focalPoint[i].transform.position.x,(SampleTerrain(new Vector2(go_focalPoint[i].transform.position.x,go_focalPoint[i].transform.position.z),f_trackRoughness)*i_yRes)+5.5f,go_focalPoint[i].transform.position.z);
            v2_curPos[i].x = v2_prevPos[i].x = Mathf.Floor(go_focalPoint[i].transform.position.x);
		    v2_curPos[i].y = v2_prevPos[i].y = Mathf.Floor(go_focalPoint[i].transform.position.z);
            UpdateTerrain(i, true, -1);    
        }
		l_track = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        
		if(Input.GetKeyUp(KeyCode.F1)) {
            Time.timeScale = 1.0f;
            Application.LoadLevel(Application.loadedLevel);
        }/*
        if(i_waypoint[0] < i_waypoint[1]) {
            i_placement[0] = 1;
            i_placement[1] = 0;
        }
        else {
            i_placement[0] = 0;
            i_placement[1] = 1;
        }*/
        for(int i = 0; i < go_focalPoint.Length; i++) {
            if(go_focalPoint[i].transform.tag == "Player")
            	go_playerPlacement[i].GetComponent<UnityEngine.UI.Text>().text = (i_placement[i]+1).ToString();
		    v2_curPos[i] = new Vector2(Mathf.Floor(go_focalPoint[i].transform.position.x),Mathf.Floor(go_focalPoint[i].transform.position.z));
		    if(v2_curPos[i] != v2_prevPos[i]) {
    				//Determine direction the character moved
    				//int i_direction; //0 = left, 1 = up, 2 = right, 3 = down
    				if(v2_curPos[i].x < v2_prevPos[i].x)
    					UpdateTerrain(i,false,0);
    				if(v2_curPos[i].x > v2_prevPos[i].x)
    					UpdateTerrain(i,false,2);
    				if(v2_curPos[i].y > v2_prevPos[i].y)
    					UpdateTerrain(i,false,1);
    				if(v2_curPos[i].y < v2_prevPos[i].y)
                        UpdateTerrain(i,false,3);
    			}
    		v2_prevPos[i] = v2_curPos[i];
        }
	}
	void UpdateTerrain(int player, bool b_init,int i_direction) {
        //print(b_init);
		Vector2[] v2_perlinPos = new Vector2[2];		
		float f_height;
		v2_perlinPos[0].x = (v2_curPos[player].x/(float)i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[0].y = (v2_curPos[player].y/(float)i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[1].x = (v2_curPos[player].x/(float)i_xzRes)*f_sampleSizes[1];
		v2_perlinPos[1].y = (v2_curPos[player].y/(float)i_xzRes)*f_sampleSizes[1];
		if(b_init) {      
			for(int i = 0; i < i_xzRes; i++) {
				for(int j = 0; j < i_xzRes; j++) {
					bool b_trackExists = false;
					Vector2 v2_cellPos = new Vector2(v2_curPos[player].x-i_xzRes/2+i,v2_curPos[player].y-i_xzRes/2+j);
					Vector3 v3_blockPos = new Vector3(v2_curPos[player].x-(i_xzRes/2)+i,Mathf.Ceil(SampleTerrain(v2_cellPos,f_blendAmount)*i_yRes),v2_curPos[player].y-(i_xzRes/2)+j);
                    if(Physics.CheckBox(v3_blockPos,new Vector3(0.4f,10f,0.4f),Quaternion.identity,lm_terrainCheck))
                    b_trackExists = true;
                    //RaycastHit hit;
                    //if(Physics.Raycast(new Vector3(v3_blockPos.x,v3_blockPos.y+10f,v3_blockPos.z),Vector3.down,out hit,20f)){
                        
                    //}
                        
                        
					//for(int k = 0; k < transform.GetChild(0).childCount; k++) {
					//	Transform t_track = transform.GetChild(0).GetChild(k);
					//	if(t_track.position.x == v3_blockPos.x && t_track.position.z == v3_blockPos.z) {
							//b_trackExists = true;
					//	}
					//}

					if(go_localBlocks[player,i,j] == null) {
						go_localBlocks[player,i,j] = (GameObject)Instantiate(go_block,v3_blockPos,Quaternion.identity);
						go_localBlocks[player,i,j].transform.parent = gameObject.transform;
                        go_localBlocks[player,i,j].GetComponent<Renderer>().material = c_envgen.m_terrain1;
                        //go_localBlocks[player,i,j].transform.name = i + "," + j;
					}

					else go_localBlocks[player,i,j].transform.position = v3_blockPos;
					if(b_trackExists) go_localBlocks[player,i,j].SetActive(false);
                   // if(Physics.CheckBox(v3_blockPos,new Vector3(0.5f,5f,0.5f),Quaternion.identity)) {
                    //    go_localBlocks[player,i,j].SetActive(false);
                    //} 
					else go_localBlocks[player,i,j].SetActive(true);
				}
			}
		}
		else {
			GameObject go_tempBlock;
			int i_newX = 0,i_newY = 0;
			GameObject go_currentBlock;
			for(int i = 0; i < i_xzRes; i++) {
				switch(i_direction) {
					case 0:
                        GameObject go_tempBlock2, go_tempBlock1 = go_localBlocks[player,i_xzRes-1,i];
                        //go_localBlocks[player,0,i] = go_localBlocks[player,i_xzRes-1,i];
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[player,j,i];
                            go_localBlocks[player,j,i] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                            
                        }
                        i_newX = 0;
                        i_newY = i;
						break;
					case 1:
                        go_tempBlock1 = go_localBlocks[player,i,0];
                        //go_localBlocks[player,i,i_xzRes-1] = go_localBlocks[player,i,0];
                        //print(go_localBlocks[player,i,i_xzRes-1].transform.name);
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[player,i,i_xzRes-1-j];
                            go_localBlocks[player,i,i_xzRes-1-j] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                        }
						i_newX = i;
						i_newY = i_xzRes-1;
						break;
					case 2:
                        go_tempBlock1 = go_localBlocks[player,0,i];
                        //go_localBlocks[player,i_xzRes-1,i] = go_localBlocks[player,0,i];
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[player,i_xzRes-1-j,i];
                            go_localBlocks[player,i_xzRes-1-j,i] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                        }
						i_newX = i_xzRes-1;
						i_newY = i;
						break;
					default:
                        go_tempBlock1 = go_localBlocks[player,i,i_xzRes-1];
                        //go_localBlocks[player,i,0] = go_localBlocks[player,i,i_xzRes-1];
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[player,i,j];
                            go_localBlocks[player,i,j] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                        }
						i_newX = i;
						i_newY = 0;
                        break;
				}

				// now do the same process as above algorithm
				//*************//
				bool b_trackExists = false; 
				Vector2 v2_cellPos = new Vector2(v2_curPos[player].x-i_xzRes/2+i_newX,v2_curPos[player].y-i_xzRes/2+i_newY);
				Vector3 v3_blockPos = new Vector3(v2_curPos[player].x-(i_xzRes/2)+i_newX,Mathf.Ceil(SampleTerrain(v2_cellPos,f_blendAmount)*i_yRes),v2_curPos[player].y-(i_xzRes/2)+i_newY);
                if(Physics.CheckBox(v3_blockPos,new Vector3(0.4f,10f,0.4f),Quaternion.identity,lm_terrainCheck))
                    go_localBlocks[player,i_newX,i_newY].SetActive(false);
                else
                    go_localBlocks[player,i_newX,i_newY].SetActive(true);
                //    b_trackExists = true;
				//Sif(go_localBlocks[player,i_newX,i_newY] == null) {
				//	go_localBlocks[player,i_newX,i_newY] = (GameObject)Instantiate(go_block,v3_blockPos,Quaternion.identity);
				//	go_localBlocks[player,i_newX,i_newY].transform.parent = gameObject.transform;
				//}
				go_localBlocks[player,i_newX,i_newY].transform.position = v3_blockPos;
				//if(b_trackExists) go_localBlocks[player,i_newX,i_newY].SetActive(false);
				//else go_localBlocks[player,i_newX,i_newY].SetActive(true);
				//*************//                
			}
            for(int i = 0; i < i_xzRes;i++) {
               for(int j = 0; j < i_xzRes; j++) {
                    //for(int k = 0; k < go_focalPoint.Length;k++){
               	
                        if(!go_localBlocks[player,j,i].activeSelf) {
                            RaycastHit hit;
                            if(!Physics.Raycast(new Vector3(go_localBlocks[player,j,i].transform.position.x,go_localBlocks[player,j,i].transform.position.y+10,go_localBlocks[player,j,i].transform.position.z),Vector3.down,out hit,20f,lm_terrainCheck)){
                                go_localBlocks[player,j,i].SetActive(true);
                            }
                            else {
                                go_localBlocks[player,j,i].SetActive(false);
                            }
                        }
                        
					//float f_distance = Vector2.Distance(new Vector2(go_localBlocks[player,j,i].transform.position.x,go_localBlocks[player,j,i].transform.position.z),new Vector2(go_focalPoint[player].transform.position.x,go_focalPoint[player].transform.position.z));
					//if(f_distance < c_trackCollider.radius+1) {
					//	go_localBlocks[player,i,j].GetComponent<Collider>().enabled = true;
					//}
					//else go_localBlocks[player,i,j].GetComponent<Collider>().enabled = false;
                  //  }                
                }
            }
		}
	}
			/*
            for(int i = 0; i < i_xzRes;i++) {
               for(int j = 0; j < i_xzRes; j++) {
                    bool b_exists = false;
                    for(int k = 0; k < player;k++){
                        //if(!go_localBlocks[player,i,j].activeSelf) {
                            Vector4 v4_boundingBox = new Vector4(v2_curPos[k].x-i_xzRes/2f,v2_curPos[k].y-i_xzRes/2f,v2_curPos[k].x+i_xzRes/2f,v2_curPos[k].y+i_xzRes/2f);
                            if(go_localBlocks[player,i,j].transform.position.x > v4_boundingBox.x &&
                                go_localBlocks[player,i,j].transform.position.z > v4_boundingBox.y && 
                                go_localBlocks[player,i,j].transform.position.x < v4_boundingBox.z && 
                                go_localBlocks[player,i,j].transform.position.z < v4_boundingBox.w){
                                b_exists = true;
                            }
                        //}
                    }
                    //if(!go_localBlocks[player,i,j].activeSelf) {
                    	for(int k = 0; k < l_track.Count; k++) {
                    		if(go_localBlocks[player,i,j].transform.position.x == l_track[k].transform.position.x &&
                    			go_localBlocks[player,i,j].transform.position.z == l_track[k].transform.position.z)
                    			b_exists = true;
                    	}
                    	if(!b_exists) {
                       		go_localBlocks[player,i,j].SetActive(true);
                    	}
                    	else go_localBlocks[player,i,j].SetActive(false);Zz
                    //}
                }
            }
        }
	}*/
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


