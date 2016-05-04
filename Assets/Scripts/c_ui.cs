using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class c_ui : MonoBehaviour {
	public int i_numPlayers;
	public KartController_pat1[] c_kartController;
	public c_terraingen_r6 c_terrainGen;
	public c_waypoint_r1 c_waypointGen;
	public UnityEngine.UI.Image[] i_playerPlacement;
	public Sprite[] s_placementImages;
	public UnityEngine.UI.Text[] t_playerSpeed;
	public UnityEngine.UI.Text[] t_playerPowerups;
	public UnityEngine.UI.Text[] t_playerTime;
	public UnityEngine.UI.Text[] t_playerDistance;
	public UnityEngine.UI.Text[] t_playerMaxDistance;
	public InvController[] c_invControllerArray;

	public float f_zVelocity, f_mMaxVelocity, f_normalizedVelocity;
	public UnityEngine.UI.Text t_winner, t_paused;
	public GameObject pKartMapItem;
	public UnityEngine.UI.Image[] i_playerMapImage;
	public RectTransform[] t_playerTransform;
	public float f_iconStartPos,f_iconEndPos;
	public float[] f_playerMapPos;
	// Use this for initialization
	
	public UnityEngine.UI.Image[] i_SpeedometerNeedle;
	float f_countdown;
	public UnityEngine.UI.Image i_countdown;
	public AudioSource asTimer;
	public AudioSource asGo;
	public bool paused = false;

	void Start () {
		//t_paused.enabled = false;
		f_countdown = 6.0f;
		//i_numPlayers = c_terrainGen.go_focalPoint.Length;
		c_kartController = new KartController_pat1[c_terrainGen.go_focalPoint.Length];
		f_iconStartPos = Screen.width/20f;
		f_iconEndPos = Screen.width/20f*19f;
		f_playerMapPos = new float[c_terrainGen.go_focalPoint.Length];
		t_playerTransform = new RectTransform[c_terrainGen.go_focalPoint.Length];
		c_invControllerArray = new InvController[i_numPlayers];
	for(int i = 0; i < i_numPlayers; i++) {
		
		}
	for(int i = 0; i < c_terrainGen.go_focalPoint.Length; i++){
		c_kartController[i] = c_terrainGen.go_focalPoint[i].GetComponent<KartController_pat1>();
		i_playerMapImage[i].sprite = c_kartController[i].s_playerIcon;
		t_playerTransform[i] = i_playerMapImage[i].rectTransform;
		}
	}
	// Update is called once per frame
	void Update () {	
//Debug.Log(Input.GetButton("Pause"));
		if (Input.GetButtonDown("Pause") == true)
		{
			paused = !paused;
			if (paused)
			{
				Time.timeScale = 0;
				//t_paused.enabled = true;
			}
			else 
			{
				Time.timeScale = 1;
				//t_paused.enabled = false;
			}
		}	
	if ((Input.GetButton("p1Item") || Input.GetButton("p2Item") || Input.GetButton("p3Item") || Input.GetButton("p4Item"))
		 && paused)
	 SceneManager.LoadScene("MainMenu");
		
	f_countdown += -Time.deltaTime;
	if(Mathf.Floor(f_countdown) <= 2f && Mathf.Floor(f_countdown) >= 0f){
		i_countdown.color = new Color(1,1,0,1);
		if(i_countdown.sprite != s_placementImages[(int)Mathf.Floor(f_countdown)]){
			i_countdown.sprite = s_placementImages[(int)Mathf.Floor(f_countdown)];	
			asTimer.Play();
			}
		}
	if(Mathf.Floor(f_countdown) == -1f && i_countdown.gameObject.activeSelf) {
		for(int i = 0; i < c_terrainGen.go_focalPoint.Length; i++) {
			c_kartController[i].state = KartController_pat1.KartState.grounded;
		}
		i_countdown.gameObject.SetActive(false);
		asTimer.pitch = 2f;
		asTimer.Play();
	}

	for(int i = 0; i < i_numPlayers; i++) {
//			print("t:"+i);
			i_playerPlacement[i].sprite = s_placementImages[(c_terrainGen.i_placement[i])];
			t_playerSpeed[i].text = Mathf.Round(c_kartController[i].f_zVelocity*9f).ToString() + "";
			t_playerTime[i].text = Mathf.Round(Time.timeSinceLevelLoad).ToString() + "s";
			t_playerDistance[i].text = (Mathf.Round((c_terrainGen.i_waypoint[i]*c_waypointGen.i_waypointDistance*18f)/528f)/10f).ToString() +"/";
			t_playerMaxDistance[i].text = (Mathf.Round((c_waypointGen.i_maxWaypoints*c_waypointGen.i_waypointDistance*18f)/528f)/10f).ToString() + "mi";
			 i_SpeedometerNeedle[i].transform.rotation = Quaternion.identity;
			f_zVelocity = c_kartController[i].f_zVelocity;
			f_mMaxVelocity = c_kartController[i].f_mMaxVelocity;
			f_normalizedVelocity = f_zVelocity/f_mMaxVelocity;
//			print(f_normalizedVelocity);
			float f_needleDegree = 270+(80*Mathf.Clamp(f_normalizedVelocity,0,Mathf.Infinity));
			 i_SpeedometerNeedle[i].transform.Rotate(Vector3.fwd*f_needleDegree);
			 
			 c_invControllerArray[i]=c_terrainGen.go_focalPoint[i].GetComponent<InvController>();
		}
		for(int i = 0; i < c_terrainGen.go_focalPoint.Length; i++) {
			f_playerMapPos[i] = f_iconStartPos + (c_terrainGen.i_waypoint[i] / (float)c_waypointGen.i_maxWaypoints)*(f_iconEndPos-f_iconStartPos);
		t_playerTransform[i].transform.position = new Vector3(f_playerMapPos[i],t_playerTransform[i].transform.position.y,t_playerTransform[i].transform.position.z);
		}
	}
}
