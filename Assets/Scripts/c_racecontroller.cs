using UnityEngine;
using System.Collections;

public class c_racecontroller : MonoBehaviour {
	public float f_countdown;
	public bool b_start = false;
	// Use this for initialization
	void Start () {
	f_countdown = 4f;
	}
	
	// Update is called once per frame
	void Update () {
	if(f_countdown > 0) f_countdown+=-Time.smoothDeltaTime;
	else{
		f_countdown = 0;
		b_start = true;
	}
	}
}
