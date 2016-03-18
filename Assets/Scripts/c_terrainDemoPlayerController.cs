using UnityEngine;
using System.Collections;

public class c_terrainDemoPlayerController : MonoBehaviour {
	public float f_downSpeed,f_upSpeed,f_horizontalSpeed,f_rotateSpeed;
	private float f_origDownSpeed;
	public c_terraingen_r4 c_controller;
	public GameObject go_nearestBlock;
	public Vector2 v2_gridPos, v2_prevPos;
	public float f_rotation, f_curRotation, f_prevRotation;
    public float f_turnDecisionCount, f_turnDuration, f_counter, f_turnCounter,f_turnStrength, f_startCounter;
    public int i_turnDirection;
	// Use this for initialization
	void Start () {
    f_turnDecisionCount = Random.Range(3f,5f);
    f_counter = 0.0f;
    f_turnCounter = 0.0f;
    f_turnDuration = (float)Random.Range(0f,2f);
	f_origDownSpeed = f_downSpeed;
    f_startCounter = 0f;
	v2_gridPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
	}
	
	// Update is called once per frame
	void Update () {
        f_counter+=Time.deltaTime;
		f_curRotation = transform.rotation.y;
		if(f_curRotation != f_prevRotation) f_rotation += Mathf.Abs(f_curRotation-f_prevRotation);
		v2_gridPos = new Vector2(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.z));
		if(v2_gridPos != v2_prevPos) {
			for(int i = 0; i < c_controller.i_xzRes; i++) {
				for(int j = 0; j < c_controller.i_xzRes; j++) {
                    for(int k = 0; k < c_controller.i_xzRes; k++)
						if(c_controller.go_localBlocks[k,i,j] != null) {
							if(c_controller.go_localBlocks[k,i,j].transform.position.x == v2_gridPos.x &&
							c_controller.go_localBlocks[k,i,j].transform.position.z == v2_gridPos.y) { //y = z in Vector2
								go_nearestBlock = c_controller.go_localBlocks[k,i,j];
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
    	transform.Translate(-Vector3.up*Time.deltaTime*f_downSpeed);
        if(f_counter > f_turnDecisionCount) {
            f_counter = 0;
            f_turnCounter = 0;
            i_turnDirection = Random.Range(0,2);
            f_turnDuration = Random.Range(0f,2f);
            f_turnStrength = Random.Range(0f,1f);
            f_turnDecisionCount = Random.Range(3f,5f);
        }
	if(f_startCounter > 2f) transform.Translate(new Vector3(0,0,1*Time.deltaTime*f_horizontalSpeed));
    else f_startCounter+= Time.deltaTime;
	if(f_turnDuration > 0) {
        if(i_turnDirection == 0) transform.Rotate(new Vector3(0,-1*Time.deltaTime*f_rotateSpeed*f_turnStrength,0));
	    if(i_turnDirection == 1) transform.Rotate(new Vector3(0,1*Time.deltaTime*f_rotateSpeed*f_turnStrength,0));
        f_turnDuration += -Time.deltaTime;
        }
	f_prevRotation = f_curRotation;
	}
}
