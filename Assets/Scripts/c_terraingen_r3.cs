using UnityEngine;
using System.Collections;

public class c_terraingen_r3 : MonoBehaviour {
	public GameObject go_focalPoint, go_block;
	public GameObject[,] go_localBlocks;
	public int i_xzRes, i_yRes;
	public int[,] i_heightmap;
	public float f_blend, f_elevationSmoothHeight,f_elevationSmoothStrength;
	public float[] f_sampleSizes;
	public Vector2[] v2_perlinOrigins;
	public Vector2 v2_curPos, v2_prevPos;
	public int i_transitionInterval;
	public bool init;
	private int i_counter;
	// Use this for initialization
	void Start () {
		init = false;
		
		i_counter = 0;
		//go_focalPoint.transform.position = new Vector3(go_focalPoint.transform.position.x,i_yRes+10,go_focalPoint.transform.position.z);
		go_localBlocks = new GameObject[i_xzRes,i_xzRes];
		v2_perlinOrigins[0] = new Vector2(Random.Range(0,10),Random.Range(0,10));
		v2_perlinOrigins[1] = new Vector2(Random.Range(0,10),Random.Range(0,10));
		v2_curPos.x = v2_prevPos.x = Mathf.Floor(go_focalPoint.transform.position.x);
		v2_curPos.y = v2_prevPos.y = Mathf.Floor(go_focalPoint.transform.position.z);
		Randomize();
		//StartCoroutine(UpdateTerrain());
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.F1)) Randomize();
		v2_curPos = new Vector2(Mathf.Floor(go_focalPoint.transform.position.x),Mathf.Floor(go_focalPoint.transform.position.z));
		if(v2_curPos != v2_prevPos) {
			StartCoroutine(UpdateTerrain());
		}
		//if(Time.timeSinceLevelLoad > 60) Application.LoadLevel(Application.loadedLevel);
		v2_prevPos = v2_curPos;
	}
	IEnumerator UpdateTerrain() {
		Vector2[] v2_perlinPos = new Vector2[2];		
		float f_height;
		v2_perlinPos[0].x = (v2_curPos.x/i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[0].y = (v2_curPos.y/i_xzRes)*f_sampleSizes[0];
		v2_perlinPos[1].x = (v2_curPos.x/i_xzRes)*f_sampleSizes[1];
		v2_perlinPos[1].y = (v2_curPos.y/i_xzRes)*f_sampleSizes[1];
		for(int i = 0; i < i_xzRes; i++) {
			for(int j = 0; j < i_xzRes; j++) {
				Vector2 v2_samplePos = new Vector2(v2_perlinOrigins[0].x+v2_perlinPos[0].x-(f_sampleSizes[0]/2f)+(i/(float)i_xzRes)*f_sampleSizes[0],v2_perlinOrigins[0].y+v2_perlinPos[0].y-(f_sampleSizes[0]/2f)+(j/(float)i_xzRes)*f_sampleSizes[0]);
				//print(v2_samplePos);
				f_height = Mathf.PerlinNoise(v2_samplePos.x,v2_samplePos.y);
				if(f_height < f_elevationSmoothHeight) f_height = f_height*(1-f_elevationSmoothStrength);
				else f_height = Mathf.Lerp(f_height*(1-f_elevationSmoothStrength),f_height,(f_height-(f_elevationSmoothHeight)/(1f-f_elevationSmoothHeight))*(1f-f_elevationSmoothStrength));
				Vector3 v3_blockPos = new Vector3(v2_curPos.x-(i_xzRes/2)+i,Mathf.Floor(f_height*i_yRes),v2_curPos.y-(i_xzRes/2)+j);
				
				if(go_localBlocks[i,j] == null) {
					go_localBlocks[i,j] = (GameObject)Instantiate(go_block,v3_blockPos,Quaternion.identity);
					go_localBlocks[i,j].transform.parent = gameObject.transform;
				}
				else go_localBlocks[i,j].transform.position = v3_blockPos;
				//go_localBlocks[i,j].transform.name = "("+i+","+j+")";
				//print(Mathf.PerlinNoise(v2_perlinPos[0].x,v2_perlinPos[0].y));
				
				
				//Blend
				
				v2_samplePos = new Vector2(v2_perlinOrigins[1].x+v2_perlinPos[1].x-(f_sampleSizes[1]/2f)+(i/(float)i_xzRes)*f_sampleSizes[1],v2_perlinOrigins[1].y+v2_perlinPos[1].y-(f_sampleSizes[1]/2f)+(j/(float)i_xzRes)*f_sampleSizes[1]);
				f_height = Mathf.PerlinNoise(v2_samplePos.x,v2_samplePos.y);
				
				if(f_height < f_elevationSmoothHeight) f_height = f_height*(1-f_elevationSmoothStrength);
				else f_height = Mathf.Lerp(f_height*(1-f_elevationSmoothStrength),f_height,f_height-(f_elevationSmoothHeight)/(1f-f_elevationSmoothHeight)*(1f-f_elevationSmoothStrength));
				
				
				v3_blockPos = new Vector3(v2_curPos.x-(i_xzRes/2)+i,Mathf.Floor(f_height*i_yRes),v2_curPos.y-(i_xzRes/2)+j);
				go_localBlocks[i,j].transform.position = new Vector3(go_localBlocks[i,j].transform.position.x,Mathf.Floor(Mathf.Lerp(go_localBlocks[i,j].transform.position.y,v3_blockPos.y,f_blend)),go_localBlocks[i,j].transform.position.z);
			}
			if(i_transitionInterval != -1)i_counter++;
			if(i_counter >= i_transitionInterval && i_transitionInterval != -1) {
				i_counter += -i_transitionInterval;
				yield return null;
			}
		}
	if(!init)init = true;
	}
	public void Randomize() {
		i_yRes = Random.Range(i_xzRes/2,i_xzRes);
		//go_focalPoint.transform.position = new Vector3(go_focalPoint.transform.position.x,i_yRes/3*2,go_focalPoint.transform.position.z);
		f_blend = Random.Range(25f,75f);
		f_elevationSmoothHeight = Random.Range(0f,1f);
		f_elevationSmoothStrength = Random.Range(0f,1f);
		f_sampleSizes[0] = Random.Range(0f,4f);
		f_sampleSizes[1] = Random.Range(f_sampleSizes[0]/2,10f);
		StartCoroutine(UpdateTerrain());
	}
}


