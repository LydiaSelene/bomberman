using UnityEngine;
using System.Collections;

public class Bomberman : MonoBehaviour {

	Animator anim;
	Vector3 moveDirection;
	public float speed = 3.5f;
	public float gravity = 9.81F;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Bewegung vor und zurück mit Animation
		float move = Input.GetAxis ("Vertical");
		anim.SetFloat ("Speed", move); 

		float sideMove = Input.GetAxis ("Horizontal");
		anim.SetFloat ("SideSpeed", sideMove); 

		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
		}

		// Rotation der Spielfigur
		float rotation = Input.GetAxis ("Mouse X");
		gameObject.transform.RotateAround (gameObject.transform.position, Vector3.up, rotation); 

		// Für eventuell zum Bomben werfen
		bool throwing = Input.GetButton ("Jump");
		anim.SetBool ("Throw", throwing); 

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
