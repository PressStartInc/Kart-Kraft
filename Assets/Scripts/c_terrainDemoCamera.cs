using UnityEngine;
using System.Collections;

public class c_camera : MonoBehaviour {
	public GameObject go_player;
	public float f_distance;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	transform.LookAt(go_player.transform.position);
	f_distance = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(Mathf.Pow(go_player.transform.position.x-transform.position.x,2)+Mathf.Pow(go_player.transform.position.y-transform.position.y,2),2)+Mathf.Pow(go_player.transform.position.z-transform.position.z,2)));
	if(f_distance > 5) transform.Translate(Vector3.fwd*Time.deltaTime*f_distance*2f);
	if(transform.position.y < go_player.transform.position.y+2) transform.Translate(Vector3.up*Time.deltaTime);
	}
}
