using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.RotateAround (gameObject.transform.position, Vector3.up, Time.deltaTime *75);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Player")) {
			PlayerStatus status =  other.GetComponent<PlayerStatus>();
			status.powerUp ();
			Destroy (gameObject); 
		}

	}
}
