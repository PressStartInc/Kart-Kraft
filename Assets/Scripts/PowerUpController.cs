using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

    private InvController invController;
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
        Destroy(gameObject);
    }

}
