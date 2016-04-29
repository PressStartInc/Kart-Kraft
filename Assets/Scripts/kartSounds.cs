using UnityEngine;
using System.Collections;

public class kartSounds : MonoBehaviour {
	public KartController_pat1 csKartController;
	public string player;
	public float fMaxVelocity;
	public float fCurVelocity;
	public float fVelocityNormal;
	public float fEngineMinPitch,fEngineMaxPitch;
	public AudioClip acEngine, acDrift, acSpinout;
	public AudioSource asEngine, asDrift, asSpinout;
	// Use this for initialization
	void Start () {
	player = csKartController.s_player;
	asEngine.clip = acEngine;
	asEngine.loop = true;
	asDrift.clip = acDrift;
	asDrift.loop = true;
	asSpinout.clip = acSpinout;
	asSpinout.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
	fCurVelocity = csKartController.f_zVelocity;
	fMaxVelocity = csKartController.f_mMaxVelocity;
	fVelocityNormal = fCurVelocity/fMaxVelocity;
	asEngine.pitch = Mathf.Lerp(fEngineMinPitch,fEngineMaxPitch,fVelocityNormal);
	if(!asEngine.isPlaying)
		asEngine.Play();		
	if(!csKartController.b_amISpinningOutRightNow && csKartController.f_driftVelocity*csKartController.f_mVelocity != 0 && csKartController.state == KartController_pat1.KartState.grounded) {
		float f_normalizedDrift = Mathf.Abs(csKartController.f_driftVelocity/csKartController.f_maxDriftVelocity)*csKartController.f_mVelocity/csKartController.f_mMaxVelocity;
		asDrift.volume = f_normalizedDrift;
		if(!asDrift.isPlaying)
			
			asDrift.Play();
	}
	else if(asDrift.isPlaying)
		asDrift.Stop();
	if(csKartController.b_amISpinningOutRightNow) {
			asSpinout.volume = 1.0f;
			if(!asSpinout.isPlaying)
				asSpinout.Play();
		}
	else if(asSpinout.isPlaying)
		asSpinout.Stop();
	}
}
