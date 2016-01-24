using UnityEngine;
using System.Collections;

public class Bomberman : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Bewegung vor und zurück mit Animation
		float move = Input.GetAxis ("Vertical");
		anim.SetFloat ("Speed", move); 

		// Rotation der Spielfigur
		float rotation = Input.GetAxis ("Mouse X");
		gameObject.transform.RotateAround (gameObject.transform.position, Vector3.up, rotation); 

		// Für eventuell zum Bomben werfen
		bool throwing = Input.GetButton ("Jump");
		anim.SetBool ("Throw", throwing); 

	}
}
