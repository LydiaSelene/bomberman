using UnityEngine;
using System.Collections;

public class PlayerCameraMove : MonoBehaviour {

	public float speed = 3.5F;
	public float jumpSpeed = 0.0F;
	//TODO: wasn das fürn Gravitations-Wert ? kraft 20 fail und gewicht 20 auch fail
	public float gravity = 9.81F;
	private Vector3 moveDirection = Vector3.zero;

	//rotation um upvector
	private float yaw = 0.0f;
	//rotation um right vector
	private float pitch = 0.0f;
	//Mausgeschwindigkeit
	public float speedH = 3.0f;
	public float speedV = 3.0f;

	void Update() {
		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");
		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;

			//Blickrichtung soll nicht zu springen führen
			moveDirection.y = 0;

			//TODO: Blickrichtung soll WSAD-Geschwindigkeit nicht ändern
			//lösung: prüfung ob forward +-x, +-z hat ?

			//bei leertaste, definierbar ?
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
