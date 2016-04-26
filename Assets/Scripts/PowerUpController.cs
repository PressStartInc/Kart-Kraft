using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

    public InvController invController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        invController = other.gameObject.transform.parent.GetComponent<InvController>();
        invController.getItem();
    }

}
