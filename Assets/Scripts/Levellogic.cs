using UnityEngine;
using System.Collections;

public class Levellogic : MonoBehaviour {

	int destroyedRigids = 0;


	// Use this for initialization
	void Start () {
	
		//die angegebene Funktion wird nach 60s aufgerufen und ab da alle 60 sekunden wieder
		InvokeRepeating("deleteRigidsBelowGround", 30, 30);
	}

	void deleteRigidsBelowGround(){
		Rigidbody[] rigids = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
		foreach (Rigidbody rigid in rigids) {
			if(rigid.transform.position.y < 0){
				Destroy(rigid.gameObject);
				destroyedRigids += 1;
			}
		}
		Debug.Log (destroyedRigids+" Rigid-Objekte schon gelöscht.");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
