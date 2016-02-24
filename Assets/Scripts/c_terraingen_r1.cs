using UnityEngine;
using System.Collections;

public class c_terraingen_old : MonoBehaviour {
	
	public GameObject p_terrainCell;
	public int i_resolution;
	public int i_height;
	public float[] f_sampleDetails;
	private int[,] i_heightmap;
	private Vector2 v2_position; 
	private Vector2[] v2_perlinOrigins;
	public bool[] b_dampenDetail;
	public float f_altDampenStart;
	public float f_altDampenStrength;
	
	public string[] s_blendTypes; //add, subtract, average
	public float[] f_blendStrengths;
	
	// Use this for initialization
	void Start () {
	GenerateTerrain();
	Camera.main.transform.position = new Vector3(i_resolution/2,i_resolution/2,20);
	Camera.main.transform.Translate(0,0,-i_resolution/2);
	}
	void Update() {
		if(Input.GetKey(KeyCode.Space)) Application.LoadLevel(Application.loadedLevel);
	}
	// Simple implementation of terrain generation using built-in perlin noise 
	void GenerateTerrain() {
		//create heightmap with size parameters
		i_heightmap = new int[i_resolution,i_resolution];
		//Pick random positions on the perlin noise texture
		v2_perlinOrigins = new Vector2[3];
		v2_perlinOrigins[0] = new Vector2(Random.Range(0f,10f),Random.Range(0f,10f));
		//v2_perlinOrigins[0] = new Vector2(9,9);
		v2_perlinOrigins[1] = new Vector2(Random.Range(0f,10f),Random.Range(0f,10f));
		//v2_perlinOrigins[1] = new Vector2(8,5);
		//v2_perlinOrigins[2] = new Vector2(Random.Range(0f,10f),Random.Range(0f,10f));
		//Sample perlin noise texture for height starting from origins
		for(int i = 0; i < i_resolution; i++){
			for(int j = 0; j < i_resolution; j++){
				float f_sampleSum;
				//Main shape
				float f_sampleStep = f_sampleDetails[0]/(float)i_resolution;
				Vector2 v2_samplePosition = new Vector2(v2_perlinOrigins[0].x+(f_sampleStep*i),v2_perlinOrigins[0].y+(f_sampleStep*j));
				//simplest way to wrap is to mirror...
				if(v2_samplePosition.x > 10) v2_samplePosition.x = 20-v2_samplePosition.x;
				if(v2_samplePosition.x > 20) v2_samplePosition.x += -20;
				if(v2_samplePosition.y > 10) v2_samplePosition.y = 20-v2_samplePosition.y;
				if(v2_samplePosition.y > 20) v2_samplePosition.y += -20;
				if(v2_samplePosition.x < 0) v2_samplePosition.x += -(v2_samplePosition.x);
				if(v2_samplePosition.x < -10) v2_samplePosition.x += 20;
				if(v2_samplePosition.y < 0) v2_samplePosition.y += -(v2_samplePosition.y);
				if(v2_samplePosition.y < -10) v2_samplePosition.y += 20;
				float f_rawSample = Mathf.PerlinNoise(v2_samplePosition.x,v2_samplePosition.y);
				
	
				//Apply Altitude Dampener
				if(b_dampenDetail[0]) f_rawSample = AltitudeDampen(f_rawSample);
				f_sampleSum = f_rawSample;
				
				//Medium detail
				f_sampleStep = f_sampleDetails[1]/(float)i_resolution;
				v2_samplePosition = new Vector2(v2_perlinOrigins[1].x+(f_sampleStep*i),v2_perlinOrigins[1].y+(f_sampleStep*j));
				if(v2_samplePosition.x > 10) v2_samplePosition.x = 20-v2_samplePosition.x;
				if(v2_samplePosition.x > 20) v2_samplePosition.x += -20;
				if(v2_samplePosition.y > 10) v2_samplePosition.y = 20-v2_samplePosition.y;
				if(v2_samplePosition.y > 20) v2_samplePosition.y += -20;
				if(v2_samplePosition.x < 0) v2_samplePosition.x += -(v2_samplePosition.x);
				if(v2_samplePosition.x < -10) v2_samplePosition.x += 20;
				if(v2_samplePosition.y < 0) v2_samplePosition.y += -(v2_samplePosition.y);
				if(v2_samplePosition.y < -10) v2_samplePosition.y += 20;
				f_rawSample = Mathf.PerlinNoise(v2_samplePosition.x,v2_samplePosition.y);
				//Apply Altitude Dampener
				if(b_dampenDetail[0]) f_rawSample = AltitudeDampen(f_rawSample);
				switch(s_blendTypes[0]) {
					case "add":
						f_sampleSum=Mathf.Clamp(f_sampleSum+(f_rawSample*f_blendStrengths[0]),0,1);
						break;
					case "subtract":
						f_sampleSum=Mathf.Clamp(f_sampleSum-(f_rawSample*f_blendStrengths[0]),0,1);
						break;
					case "average":
						f_sampleSum=Mathf.Clamp((f_sampleSum+Mathf.Lerp(f_sampleSum,f_rawSample,f_blendStrengths[0]))/2f,0,1);
						break;
				}
				//fine detail
				f_sampleStep = f_sampleDetails[2]/(float)i_resolution;
				v2_samplePosition = new Vector2(v2_perlinOrigins[2].x+(f_sampleStep*i),v2_perlinOrigins[2].y+(f_sampleStep*j));
				if(v2_samplePosition.x > 10) v2_samplePosition.x = 20-v2_samplePosition.x;
				if(v2_samplePosition.x > 20) v2_samplePosition.x += -20;
				if(v2_samplePosition.y > 10) v2_samplePosition.y = 20-v2_samplePosition.y;
				if(v2_samplePosition.y > 20) v2_samplePosition.y += -20;
				if(v2_samplePosition.x < 0) v2_samplePosition.x += -(v2_samplePosition.x);
				if(v2_samplePosition.x < -10) v2_samplePosition.x += 20;
				if(v2_samplePosition.y < 0) v2_samplePosition.y += -(v2_samplePosition.y);
				if(v2_samplePosition.y < -10) v2_samplePosition.y += 20;
				f_rawSample = Mathf.PerlinNoise(v2_samplePosition.x,v2_samplePosition.y);
				//Apply Altitude Dampener
				if(b_dampenDetail[0]) f_rawSample = AltitudeDampen(f_rawSample);
				switch(s_blendTypes[0]) {
					case "add":
						f_sampleSum=Mathf.Clamp(f_sampleSum+(f_rawSample*f_blendStrengths[1]),0,1);
						break;
					case "subtract":
						f_sampleSum=Mathf.Clamp(f_sampleSum-(f_rawSample*f_blendStrengths[1]),0,1);
						break;
					case "average":
						f_sampleSum=Mathf.Clamp((f_sampleSum+Mathf.Lerp(f_sampleSum,f_rawSample,f_blendStrengths[1]))/2f,0,1);
						break;
				}
				//Create blocks
				i_heightmap[i,j] = (int)(f_sampleSum*i_height);
				Instantiate(p_terrainCell,new Vector3(i,i_heightmap[i,j],j),Quaternion.identity);
			}
		}
	}
	float AltitudeDampen(float f_rawSample){
		float f_altDampenHeight = 1f-f_altDampenStart;
				
		if(f_rawSample < f_altDampenStart) f_rawSample = f_rawSample*(1-f_altDampenStrength);
		//else f_rawSample = (f_altDampenStart*(1-f_altDampenStrength))+f_rawSample*(f_altDampenStrength*(Mathf.Clamp((f_rawSample-f_altDampenStart)/f_altDampenHeight,0,1)));
		else f_rawSample = Mathf.Lerp(f_rawSample*(1-f_altDampenStrength),f_rawSample,(f_rawSample-f_altDampenStart)/f_altDampenHeight);
		return f_rawSample;
	}
}

