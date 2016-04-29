using UnityEngine;
using System.Collections;

public class PowerUpImage : MonoBehaviour {
	public InvController script;
	public UnityEngine.UI.Image i_image;
	public Sprite[] s_item;
	public 
	void Start () {
		s_item[0] = null;
		i_image = transform.GetComponent<UnityEngine.UI.Image>();
	}
	void FixedUpdate () {
		 i_image.sprite = s_item[script.heldItem];
		 if(script.heldItem == 0) i_image.color = new Color(0,0,0,0);
		 else i_image.color = new Color(1,1,1,1);
	}
}
