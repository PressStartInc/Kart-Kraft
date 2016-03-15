using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class c_terraingen_r4 : MonoBehaviour {
	public GameObject go_focalPoint, go_block;
	public GameObject[,] go_localBlocks;
	public int i_xzRes, i_yRes, i_nextX, i_nextY, i_xDir, i_yDir;
	public int[,] i_heightmap;
	public float f_blendAmount, f_trackRoughness, f_elevationSmoothHeight,f_elevationSmoothStrength;
    public bool b_Randomize = false;
	public float[] f_sampleSizes;
	private Vector2[] v2_perlinOrigins;
	private Vector2 v2_curPos, v2_prevPos;
	public List<Transform> l_track;
	// Use this for initialization
	void Start () {
        v2_perlinOrigins = new Vector2[f_sampleSizes.Length];
        i_nextX = i_nextY = -1;
        i_xDir = i_yDir = 0;
		go_localBlocks = new GameObject[i_xzRes,i_xzRes];
		v2_perlinOrigins[0] = new Vector2(Random.Range(0f,10f),Random.Range(0,10f));
		v2_perlinOrigins[1] = new Vector2(Random.Range(0f,10f),Random.Range(0,10f));
		v2_curPos.x = v2_prevPos.x = Mathf.Floor(go_focalPoint.transform.position.x);
		v2_curPos.y = v2_prevPos.y = Mathf.Floor(go_focalPoint.transform.position.z);
		if(b_Randomize) Randomize();
        UpdateTerrain(true, -1);
		l_track = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.F1)) Randomize();
		v2_curPos = new Vector2(Mathf.Floor(go_focalPoint.transform.position.x),Mathf.Floor(go_focalPoint.transform.position.z));
		if(v2_curPos != v2_prevPos) {
				//Determine direction the character moved
				int i_direction; //0 = left, 1 = up, 2 = right, 3 = down
				if(v2_curPos.x < v2_prevPos.x)
					UpdateTerrain(false,0);
				if(v2_curPos.x > v2_prevPos.x)
					UpdateTerrain(false,2);
				if(v2_curPos.y > v2_prevPos.y)
					UpdateTerrain(false,1);
				if(v2_curPos.y < v2_prevPos.y)
                    UpdateTerrain(false,3);
	
			}
		v2_prevPos = v2_curPos;
	}
	void UpdateTerrain(bool b_init,int i_direction) {
        //print(b_init);
		Vector2[] v2_perlinPos = new Vector2[2];		
		float f_height;
		v2_perlinPos[0].x = (v2_curPos.x/(float)i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[0].y = (v2_curPos.y/(float)i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[1].x = (v2_curPos.x/(float)i_xzRes)*f_sampleSizes[1];
		v2_perlinPos[1].y = (v2_curPos.y/(float)i_xzRes)*f_sampleSizes[1];
		if(b_init) {
			for(int i = 0; i < i_xzRes; i++) {
				for(int j = 0; j < i_xzRes; j++) {
					bool b_trackExists = false;
					Vector2 v2_cellPos = new Vector2(v2_curPos.x-i_xzRes/2+i,v2_curPos.y-i_xzRes/2+j);
					Vector3 v3_blockPos = new Vector3(v2_curPos.x-(i_xzRes/2)+i,Mathf.Ceil(SampleTerrain(v2_cellPos,f_blendAmount)*i_yRes),v2_curPos.y-(i_xzRes/2)+j);
					for(int k = 0; k < transform.GetChild(0).childCount; k++) {
						Transform t_track = transform.GetChild(0).GetChild(k);
						if(t_track.position.x == v3_blockPos.x && t_track.position.z == v3_blockPos.z) {
							b_trackExists = true;
						}
					}
					if(go_localBlocks[i,j] == null) {
						go_localBlocks[i,j] = (GameObject)Instantiate(go_block,v3_blockPos,Quaternion.identity);
						go_localBlocks[i,j].transform.parent = gameObject.transform;
                        //go_localBlocks[i,j].transform.name = i + "," + j;
					}
					else go_localBlocks[i,j].transform.position = v3_blockPos;
					if(b_trackExists) go_localBlocks[i,j].SetActive(false);
					else go_localBlocks[i,j].SetActive(true);
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
                        GameObject go_tempBlock2, go_tempBlock1 = go_localBlocks[i_xzRes-1,i];
                        //go_localBlocks[0,i] = go_localBlocks[i_xzRes-1,i];
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[j,i];
                            go_localBlocks[j,i] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                        }
                        i_newX = 0;
                        i_newY = i;
						break;
					case 1:
                        go_tempBlock1 = go_localBlocks[i,0];
                        //go_localBlocks[i,i_xzRes-1] = go_localBlocks[i,0];
                        //print(go_localBlocks[i,i_xzRes-1].transform.name);
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[i,i_xzRes-1-j];
                            go_localBlocks[i,i_xzRes-1-j] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                        }
						i_newX = i;
						i_newY = i_xzRes-1;
						break;
					case 2:
                        go_tempBlock1 = go_localBlocks[0,i];
                        //go_localBlocks[i_xzRes-1,i] = go_localBlocks[0,i];
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[i_xzRes-1-j,i];
                            go_localBlocks[i_xzRes-1-j,i] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                        }
						i_newX = i_xzRes-1;
						i_newY = i;
						break;
					default:
                        go_tempBlock1 = go_localBlocks[i,i_xzRes-1];
                        //go_localBlocks[i,0] = go_localBlocks[i,i_xzRes-1];
                        for(int j = 0; j < i_xzRes; j++) {
                            go_tempBlock2 = go_localBlocks[i,j];
                            go_localBlocks[i,j] = go_tempBlock1;
                            go_tempBlock1 = go_tempBlock2;
                        }
						i_newX = i;
						i_newY = 0;
                        break;
				}

				// now do the same process as above algorithm
				//*************//
				bool b_trackExists = false;
				Vector2 v2_cellPos = new Vector2(v2_curPos.x-i_xzRes/2+i_newX,v2_curPos.y-i_xzRes/2+i_newY);
				Vector3 v3_blockPos = new Vector3(v2_curPos.x-(i_xzRes/2)+i_newX,Mathf.Ceil(SampleTerrain(v2_cellPos,f_blendAmount)*i_yRes),v2_curPos.y-(i_xzRes/2)+i_newY);
				for(int k = 0; k < l_track.Count; k++) {
					Transform t_track = l_track[k];
					if(t_track.position.x == v3_blockPos.x && t_track.position.z == v3_blockPos.z) {
						b_trackExists = true;
					}
				}
				if(go_localBlocks[i_newX,i_newY] == null) {
					go_localBlocks[i_newX,i_newY] = (GameObject)Instantiate(go_block,v3_blockPos,Quaternion.identity);
					go_localBlocks[i_newX,i_newY].transform.parent = gameObject.transform;
				}
				else go_localBlocks[i_newX,i_newY].transform.position = v3_blockPos;
				if(b_trackExists) go_localBlocks[i_newX,i_newY].SetActive(false);
				else go_localBlocks[i_newX,i_newY].SetActive(true);
				//*************//
			}
		}
	}
	public void Randomize() {
		i_yRes = Random.Range(i_xzRes/2,i_xzRes);
		f_blendAmount = Random.Range(25f,5f);
		f_elevationSmoothHeight = Random.Range(0.25f,0.75f);
		f_elevationSmoothStrength = Random.Range(0f,0.95f);
		f_sampleSizes[0] = Random.Range(0f,2f);
		f_sampleSizes[1] = Random.Range(f_sampleSizes[0]/2,10f);
		
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
		float f_finalHeight = Mathf.Lerp(f_height,f_height2,f_blend/100f);
		//print((v2_samplePos + ":: " + f_height + " + " + f_height2 + " = " + f_finalHeight));
		return f_finalHeight;
	}
}


