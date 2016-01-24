using UnityEngine;
using System.Collections;

public class Bomberman : MonoBehaviour {

	Animator anim;
	Vector3 moveDirection;
	public float speed = 3.5f;
	public float gravity = 9.81F;
	float yaw, pitch;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Bewegung vor und zurück mit Animation
		anim.SetFloat ("Speed", Input.GetAxis ("Vertical")); 

		// Starten der Animation zum Seitwärtslaufen
		anim.SetFloat ("SideSpeed", Input.GetAxis ("Horizontal")); 

		// Starten der Animation zum Bombenwurf
		anim.SetBool ("Throw", Input.GetButton ("Jump")); 

		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
		}

		// Rotation der Spielfigur
		float camHorizontal = Input.GetAxis ("Mouse X");
		// Schwellenwerte für Controller-Input
		if (camHorizontal >= 0.5 || camHorizontal <= -0.5) {
			gameObject.transform.RotateAround (gameObject.transform.position, Vector3.up, camHorizontal * 5); 
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
