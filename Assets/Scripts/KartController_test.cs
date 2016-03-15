using UnityEngine;
using System.Collections;

public class KartController_test : MonoBehaviour {
    public Rigidbody rb_frontWheels,rb_backWheels;
    public float f_torque, f_speed;
    public Vector3 v3_rotation;
    public float xRotationLimit = 20f;
    public float yRotationLimit = 20f;
    public float zRotationLimit = 20f;
	// Use this for initialization
	void Start () {
	rb_frontWheels.maxAngularVelocity = 100f;
    rb_backWheels.maxAngularVelocity = 100f;
	}
	
	// Update is called once per frame

 
void Update () {
 v3_rotation = transform.eulerAngles;
if(transform.rotation.eulerAngles.x > xRotationLimit && transform.rotation.eulerAngles.x < 360-xRotationLimit){
if(transform.rotation.eulerAngles.x > xRotationLimit) transform.rotation = Quaternion.Euler(xRotationLimit,transform.rotation.y,transform.rotation.z);
else  transform.rotation = Quaternion.Euler(360-xRotationLimit,transform.rotation.y,transform.rotation.z);
}
 
if(transform.rotation.eulerAngles.y > yRotationLimit && transform.rotation.eulerAngles.y < 360-yRotationLimit){
if(transform.rotation.eulerAngles.y > yRotationLimit) transform.rotation = Quaternion.Euler(transform.rotation.x,yRotationLimit,transform.rotation.z);
else  transform.rotation = Quaternion.Euler(transform.rotation.x,360-yRotationLimit,transform.rotation.z);
}
 
if(transform.rotation.eulerAngles.z > zRotationLimit && transform.rotation.eulerAngles.z < 360-zRotationLimit){
if(transform.rotation.eulerAngles.z > zRotationLimit) transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,zRotationLimit);
else  transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,360-zRotationLimit);
}
        
	    if(Input.GetKey(KeyCode.W)) {
        //rb_frontWheels.AddTorque(new Vector3(1,0,0)*f_torque);
        //rb_backWheels.AddTorque(new Vector3(1,0,0)*f_torque);
        transform.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,1)*f_speed);
        }
	}
}
