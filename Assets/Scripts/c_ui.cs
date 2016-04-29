using UnityEngine;
using System.Collections;

public class c_ui : MonoBehaviour {
	public int i_numPlayers;
	public KartController_pat1[] c_kartControllerArray;
	public c_terraingen_r6 c_terrainGen;
	public c_waypoint_r1 c_waypointGen;
	public InvController[] c_invControllerArray;
	public UnityEngine.UI.Text[] t_playerPlacement;
	public UnityEngine.UI.Text[] t_playerSpeed;
	public UnityEngine.UI.Text[] t_playerPowerups;
	public UnityEngine.UI.Text[] t_playerTime;
	public UnityEngine.UI.Text[] t_playerDistance;
	public UnityEngine.UI.Text[] t_playerMaxDistance;
	public UnityEngine.UI.Text t_winner;

	// Use this for initialization
	void Start () {
		c_kartControllerArray = new KartController_pat1[i_numPlayers];
		c_invControllerArray = new InvController[i_numPlayers];

		for(int i=0; i<i_numPlayers;i++){
			c_kartControllerArray[i]=c_terrainGen.go_focalPoint[i].GetComponent<KartController_pat1>();
			c_invControllerArray[i]=c_terrainGen.go_focalPoint[i].GetComponent<InvController>();
		}
	}

	// Update is called once per frame
	void Update () {
	for(int i = 0; i < i_numPlayers; i++) {
			t_playerPlacement[i].text = (c_terrainGen.i_placement[i]+1).ToString();
			t_playerSpeed[i].text = Mathf.Round(c_kartControllerArray[i].f_mVelocity*9f).ToString() + " mph";
			t_playerTime[i].text = Mathf.Round(Time.timeSinceLevelLoad).ToString() + " seconds";
			t_playerDistance[i].text = (Mathf.Round((c_terrainGen.i_waypoint[i]*c_waypointGen.i_waypointDistance*18f)/528f)/10f).ToString() +"/";
			t_playerMaxDistance[i].text = (Mathf.Round((c_waypointGen.i_maxWaypoints*c_waypointGen.i_waypointDistance*18f)/528f)/10f).ToString() + "mi";
			// t_playerPowerups[i].text = c_invControllerArray[i].heldItem.ToString();
		}	
	}
}
