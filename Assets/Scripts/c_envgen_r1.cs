using UnityEngine;
using System.Collections;

public class c_envgen_r1 : MonoBehaviour {
    public Material m_terrain1,m_terrain2;
    public Material m_track;
    public Material m_skyGradient;
    public GameObject go_directionalLight;
    public Color c_terrainColor1, c_terrainColor2, c_trackColor1, c_trackColor2, c_skyColor, c_groundColor;
    public float f_sunSize, f_atmosphere, f_exposure, f_skyIntensity;
    
	// Use this for initialization
	void Start () {
    go_directionalLight.transform.Rotate(Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f));
    if(go_directionalLight.transform.eulerAngles.x > 180) go_directionalLight.GetComponent<Light>().intensity = 0f;
    int i_numColorComponents = Random.Range(0,2)+1;
    print(i_numColorComponents);
    int[] i_mainColorComponents = new int[i_numColorComponents];
        for(int i = 0; i < i_numColorComponents; i++) {
             bool b_unique = true;
             do {
                 i_mainColorComponents[i] = Random.Range(0,3);
                 for(int j = 0; j < i-1; j++) {
                     if(i_mainColorComponents[i] == i_mainColorComponents[j]) 
                        b_unique = false;
                 }
             }while(!b_unique);
             switch(i_mainColorComponents[i]) {
                 case 0:
                    c_skyColor = new Color(1,c_skyColor.g,c_skyColor.b,1);
                    break;
                 case 1:
                    c_skyColor = new Color(c_skyColor.r,1,c_skyColor.b,1);
                    break;
                 case 2:
                    c_skyColor = new Color(c_skyColor.r,c_skyColor.g,1,1);
                    break;                                
             }
        }  
        f_skyIntensity = Random.Range(0f,1f);
        c_skyColor = new Color((c_skyColor.r == 1f) ? c_skyColor.r : Random.Range(0.0f,0.75f)*f_skyIntensity,
                                    (c_skyColor.g == 1f) ? c_skyColor.g : Random.Range(0.0f,0.75f)*f_skyIntensity,
                                    (c_skyColor.b == 1f) ? c_skyColor.b : Random.Range(0.0f,0.75f)*f_skyIntensity);
                                    
        f_sunSize = Random.Range(0f,1f);
        f_exposure = Random.Range(0f,8f);
        f_atmosphere = Random.Range(0f,5f);
        RenderSettings.skybox.SetColor("_SkyTint", c_skyColor);
        RenderSettings.skybox.SetFloat("_Exposure",f_exposure);
        RenderSettings.skybox.SetFloat("_AtmosphereThickness",f_atmosphere);
        RenderSettings.skybox.SetFloat("_SunSize",f_sunSize);
        //RenderSettings.fogColor = c_terrainColor1;
        //RenderSettings.fogDensity = Random.Range(0f,f_atmosphere)*0.1f;
        
        
        c_terrainColor1 = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
        c_terrainColor2 = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
        c_groundColor = c_terrainColor1;
        RenderSettings.skybox.SetColor("_GroundColor", c_groundColor);
        
        //temporarily assigning albedo and emission color
        m_terrain1.SetColor("_Color",c_terrainColor1);
        m_terrain1.SetColor("_EmissionColor",c_terrainColor2);
	    
        c_trackColor1 = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
        c_trackColor2 = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
        
        m_track.SetColor("_Color",c_trackColor1);
        m_track.SetColor("_EmissionCoor",c_trackColor2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
