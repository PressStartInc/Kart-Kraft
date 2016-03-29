using UnityEngine;
using System.Collections;

public class c_kartAngleCalc : MonoBehaviour {
	public c_terraingen_r4 c_terraingen;
	public KartController_pat1 c_kartcontroller;
	public Transform t_box;
	public float f_kartLength, f_kartWidth,f_lossyKartLength,f_lossyKartWidth;
	public Vector2 v2_frontLeft, v2_frontRight, v2_backLeft, v2_backRight;
	private float f_frontLeftHeight, f_frontRightHeight, f_backLeftHeight, f_backRightHeight;
	public float f_xLeftheight,f_xRightheight,f_zFrontheight,f_zBackheight;
	public float f_xLeftRads,f_xRightRads,f_zFrontRads,f_zBackRads;
	public float f_xAngle,f_zAngle;
	public RaycastHit hit;
	public LayerMask lm_ignorelayer;

	// Use this for initialization
	void Start () {
		f_kartLength = t_box.localScale.z;
		f_kartWidth = t_box.localScale.x;
		f_lossyKartLength = t_box.lossyScale.z;
		f_lossyKartWidth = t_box.lossyScale.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	if(c_kartcontroller.state == KartController_pat1.KartState.grounded ) {
		v2_frontLeft = new Vector2(transform.TransformPoint(new Vector3(-f_kartWidth*0.5f,c_terraingen.i_yRes,f_kartLength*0.5f)).x,transform.TransformPoint(new Vector3(-f_kartWidth*0.5f,c_terraingen.i_yRes,f_kartLength*0.5f)).z);
		v2_frontRight = new Vector2(transform.TransformPoint(new Vector3(f_kartWidth*0.5f,c_terraingen.i_yRes,f_kartLength*0.5f)).x,transform.TransformPoint(new Vector3(f_kartWidth*0.5f,c_terraingen.i_yRes,f_kartLength*0.5f)).z);
		v2_backLeft = new Vector2(transform.TransformPoint(new Vector3(-f_kartWidth*0.5f,c_terraingen.i_yRes,-f_kartLength*0.5f)).x,transform.TransformPoint(new Vector3(-f_kartWidth*0.5f,c_terraingen.i_yRes,-f_kartLength*0.5f)).z);
		v2_backRight = new Vector2(transform.TransformPoint(new Vector3(f_kartWidth*0.5f,c_terraingen.i_yRes,-f_kartLength*0.5f)).x,transform.TransformPoint(new Vector3(f_kartWidth*0.5f,c_terraingen.i_yRes,-f_kartLength*0.5f)).z);

		//Debug.DrawRay(transform.TransformPoint(new Vector3(-f_kartWidth*0.5f,c_terraingen.i_yRes,f_kartLength*0.5f)),-Vector3.up*c_terraingen.i_yRes,Color.red);
		Debug.DrawRay(new Vector3(v2_frontLeft.x,c_terraingen.i_yRes,v2_frontLeft.y),-Vector3.up*c_terraingen.i_yRes,Color.black);
		Debug.DrawRay(transform.TransformPoint(new Vector3(f_kartWidth*0.5f,c_terraingen.i_yRes,f_kartLength*0.5f)),-Vector3.up*c_terraingen.i_yRes,Color.blue);
		Debug.DrawRay(transform.TransformPoint(new Vector3(-f_kartWidth*0.5f,c_terraingen.i_yRes,-f_kartLength*0.5f)),-Vector3.up*c_terraingen.i_yRes,Color.green);
		Debug.DrawRay(transform.TransformPoint(new Vector3(f_kartWidth*0.5f,c_terraingen.i_yRes,-f_kartLength*0.5f)),-Vector3.up*c_terraingen.i_yRes,Color.yellow);
		
		if(Physics.Raycast(new Vector3(v2_frontLeft.x,c_terraingen.i_yRes,v2_frontLeft.y),-Vector3.up,out hit, lm_ignorelayer)) {
//			print(hit.transform.name);
			
			if(hit.transform.name == "p_track(Old)" || hit.transform.name == "p_track(New)")
				f_frontLeftHeight = c_terraingen.SampleTerrain(v2_frontLeft,c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
			else if(hit.transform.name == "p_terrain")
				f_frontLeftHeight = c_terraingen.SampleTerrain(v2_frontLeft,c_terraingen.f_blendAmount)*c_terraingen.i_yRes;
			//*/
			//f_frontLeftHeight = hit.point.y;
			}
			
		if(Physics.Raycast(new Vector3(v2_frontRight.x,c_terraingen.i_yRes,v2_frontRight.y),-Vector3.up,out hit, lm_ignorelayer)) {
			
			if(hit.transform.name == "p_track(Old)" || hit.transform.name == "p_track(New)")
				f_frontRightHeight = c_terraingen.SampleTerrain(v2_frontRight,c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
			else if(hit.transform.name == "p_terrain")
				f_frontRightHeight = c_terraingen.SampleTerrain(v2_frontRight,c_terraingen.f_blendAmount)*c_terraingen.i_yRes;
			//	*/
			//f_frontRightHeight = hit.point.y;
			}
		if(Physics.Raycast(new Vector3(v2_backLeft.x,c_terraingen.i_yRes,v2_backLeft.y),-Vector3.up,out hit, lm_ignorelayer)) {
			if(hit.transform.name == "p_track(Old)" || hit.transform.name == "p_track(New)")
				f_backLeftHeight = c_terraingen.SampleTerrain(v2_backLeft,c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
			else if(hit.transform.name == "p_terrain")
				f_backLeftHeight = c_terraingen.SampleTerrain(v2_backLeft,c_terraingen.f_blendAmount)*c_terraingen.i_yRes;
			//	*/
			//f_backLeftHeight = hit.point.y;				
			}
		if(Physics.Raycast(new Vector3(v2_backRight.x,c_terraingen.i_yRes,v2_backRight.y),-Vector3.up,out hit, lm_ignorelayer)) {
			
			if(hit.transform.name == "p_track(Old)" || hit.transform.name == "p_track(New)")
				f_backRightHeight = c_terraingen.SampleTerrain(v2_backRight,c_terraingen.f_trackRoughness)*c_terraingen.i_yRes;
			else if(hit.transform.name == "p_terrain")
				f_backRightHeight = c_terraingen.SampleTerrain(v2_backRight,c_terraingen.f_blendAmount)*c_terraingen.i_yRes;
				
			//f_backRightHeight = hit.point.y;
			}
		f_xLeftheight = f_frontLeftHeight-f_backLeftHeight;			
		f_xRightheight = f_frontRightHeight-f_backRightHeight;
		f_zFrontheight = f_frontLeftHeight-f_frontRightHeight;
		f_zBackheight = f_backLeftHeight-f_backRightHeight;
		f_zFrontRads = Mathf.Atan2(f_zFrontheight,f_lossyKartWidth);
		f_zBackRads = Mathf.Atan2(f_zBackheight,f_lossyKartWidth);
		f_xLeftRads = Mathf.Atan2(f_xLeftheight,f_lossyKartLength);
		f_xRightRads = Mathf.Atan2(f_xRightheight,f_lossyKartLength);
		f_zAngle = -(ToDegrees(f_zFrontRads)+ToDegrees(f_zBackRads))/2f;
		f_xAngle = -(ToDegrees(f_xLeftRads)+ToDegrees(f_xRightRads))/2f;
		//print(Mathf.Atan2(f_backLeftHeight-f_frontLeftHeight,f_kartLength));
		t_box.rotation = Quaternion.Lerp(t_box.rotation,Quaternion.Euler(f_xAngle,transform.eulerAngles.y,f_zAngle),Time.deltaTime*10f);
		}
		else {

		}
	}
	float ToDegrees(float f_rads) {
		return (f_rads * Mathf.Rad2Deg);
	}
}
