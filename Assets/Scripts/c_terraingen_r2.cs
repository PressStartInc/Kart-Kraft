using UnityEngine;
using System.Collections;

public class c_terraingen : MonoBehaviour {
	public GameObject go_target;
	public Vector2 v2_prevPos, v2_curPos;
	// Use this for initialization
	void Start () {
	v2_curPos = new Vector2(Mathf.Floor(go_target.transform.position.x),Mathf.Floor(go_target.transform.position.y));
	v2_prevPos= v2_curPos;
	}
	
	// Update is called once per frame
	void Update () {
	v2_curPos = new Vector2(Mathf.Floor(go_target.transform.position.x),Mathf.Floor(go_target.transform.position.y));
	}
}
