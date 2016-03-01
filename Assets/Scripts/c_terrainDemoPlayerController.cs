using UnityEngine;
using System.Collections;

public class c_terrainDemoPlayerController : MonoBehaviour {
	public float f_downSpeed,f_upSpeed,f_horizontalSpeed,f_rotateSpeed;
	private float f_origDownSpeed;
	public c_terraingen_r3 c_controller;
	public GameObject go_nearestBlock;
	public Vector2 v2_gridPos, v2_prevPos;
	public float f_rotation, f_curRotation, f_prevRotation;
	// Use this for initialization
	void Start () {
	f_origDownSpeed = f_downSpeed;
	v2_gridPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
	}
	
	// Update is called once per frame
	void Update () {
		f_curRotation = transform.rotation.y;
		if(f_curRotation != f_prevRotation) f_rotation += Mathf.Abs(f_curRotation-f_prevRotation);
		v2_gridPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
		if(v2_gridPos != v2_prevPos) {
			for(int i = 0; i < c_controller.i_xzRes; i++) {
				for(int j = 0; j < c_controller.i_xzRes; j++) {
					if(c_controller.go_localBlocks[i,j] != null) {
						if(c_controller.go_localBlocks[i,j].transform.position.x == v2_gridPos.x &&
						c_controller.go_localBlocks[i,j].transform.position.z == v2_gridPos.y) { //y = z in Vector2
							go_nearestBlock = c_controller.go_localBlocks[i,j];
						}
					}
				}
			}
		v2_prevPos = v2_gridPos;
		}
	if(go_nearestBlock != null) {
		if(transform.position.y < go_nearestBlock.transform.position.y+8) {
			f_downSpeed = Mathf.Lerp(f_downSpeed,0,transform.position.y-(go_nearestBlock.transform.position.y+6));
			if(transform.position.y < go_nearestBlock.transform.position.y+6)transform.position = new Vector3(transform.position.x,go_nearestBlock.transform.position.y+6,transform.position.z);
		}
		else f_downSpeed = f_origDownSpeed;
	}
	if(Input.GetKey(KeyCode.Space)) transform.Translate(Vector3.up*Time.deltaTime*f_upSpeed);
	else if(Input.GetKey(KeyCode.LeftShift)) transform.Translate(-Vector3.up*Time.deltaTime*f_downSpeed*4);
	else transform.Translate(-Vector3.up*Time.deltaTime*f_downSpeed);
	if(Input.GetKey(KeyCode.W)) transform.Translate(new Vector3(0,0,1*Time.deltaTime*f_horizontalSpeed));
	else if(Input.GetKey(KeyCode.S)) transform.Translate(new Vector3(0,0,(-1*Time.deltaTime*f_horizontalSpeed*0.5f)));
	if(Input.GetKey(KeyCode.A)) transform.Rotate(new Vector3(0,-1*Time.deltaTime*f_rotateSpeed,0));
	else if(Input.GetKey(KeyCode.D)) transform.Rotate(new Vector3(0,1*Time.deltaTime*f_rotateSpeed,0));
	
	f_prevRotation = f_curRotation;
	}
}
