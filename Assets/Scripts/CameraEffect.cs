using UnityEngine;
using System.Collections;

public class CameraEffect : MonoBehaviour {
	public KartController_pat c_kartController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60,100,Mathf.Clamp((c_kartController.f_zVelocity-c_kartController.f_zMaxVelocity/2f)/(c_kartController.f_zMaxVelocity-c_kartController.f_zMaxVelocity/2f),0f,Mathf.Infinity));
	//transform.localPosition = new Vector3(0,2+Mathf.Lerp(0,8,c_kartController.f_zVelocity/c_kartController.f_zMaxVelocity),-10-Mathf.Lerp(0,20,c_kartController.f_zVelocity/c_kartController.f_zMaxVelocity));
	}
}
