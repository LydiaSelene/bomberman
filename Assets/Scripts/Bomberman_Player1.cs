using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bomberman_Player1 : MonoBehaviour {

	Animator anim;
	Vector3 moveDirection;
	public float speed = 3.5f;
	public float gravity = 9.81F;
	public int throwTrust = 160, numberBombs = 0, maxBombs = 1;
	int lives;
	public GameObject BombPrefab;
	float detonationTime = 3.0f, timeBetweenBombThrows = 1.0f; 
	float bombRadius = 1.0f; 

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		lives = 2; 
	}
	
	// Update is called once per frame
	void Update () {
		// Start der Animation des Laufens
		anim.SetFloat ("Speed", Input.GetAxis ("Vertical_Player1")); 

		// Starten der Animation zum Seitwärtslaufen
		anim.SetFloat ("SideSpeed", Input.GetAxis ("Horizontal_Player1")); 

		// Starten der Animation zum Bombenwurf
		anim.SetFloat ("Throw", Input.GetAxis ("Jump_Player1")); 

		// Translokation der Spielfigur
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal_Player1"), 0, Input.GetAxis("Vertical_Player1"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
		}

		// Rotation der Spielfigur
		gameObject.transform.RotateAround (gameObject.transform.position, Vector3.up, Input.GetAxis ("Mouse X_Player1") * 5); 

		// Übernahme aller berechneten Werte und Ausführung der Bewegung
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

		// Bomben-Timer
		detonationTime -= (Time.deltaTime % 60); 
		if (detonationTime <= 0) {
			numberBombs = 0;
			detonationTime = 3.0f; 
		} 

		// Bomber werfen bei gedrücktem Triggern
		timeBetweenBombThrows -= (Time.deltaTime % 60);
		if (timeBetweenBombThrows <= 0 && Input.GetAxis ("Jump_Player1") >= 0.8) {
			timeBetweenBombThrows = 1.0f;
			if (numberBombs < maxBombs) {
				numberBombs++;
				detonationTime = 3.0f; 
				createGameObject ();
			}
		}
	}

	private void createGameObject(){
		//calculate spawn position
		Vector3 spawnPosition = gameObject.transform.position;
		//+ 1/4 player height (höhe auf Brust, statt Bauch)
		spawnPosition.y += gameObject.transform.lossyScale.y / 4;
		//+ x in forward dir, bomb before playr
		spawnPosition += gameObject.transform.forward.normalized/2;

		//ein prefab in der szene erzeugen
		GameObject bomb = (GameObject) GameObject.Instantiate(BombPrefab, spawnPosition, Quaternion.identity);
		bomb.transform.Rotate (	Random.value*Random.Range (-80, 80), 
			Random.value*Random.Range (-80, 80),
			Random.value*Random.Range (-80, 80));
		//werfen
		Rigidbody bombRB = bomb.GetComponent<Rigidbody> ();
		bombRB.AddForce(gameObject.transform.forward.normalized * throwTrust, ForceMode.Force);

		//bomb.SendMessage("setOwningPlayerStatus", GetComponent<PlayerStatus>(), SendMessageOptions.RequireReceiver);
		bomb.SendMessage("setBombRadius", bombRadius, SendMessageOptions.RequireReceiver);
	}

	public void setBombRadius(float radius){
		bombRadius = radius;
		if (bombRadius > 10) {
			bombRadius = 10;
		} else if (bombRadius < 1) {
			bombRadius = 1;
		}
	}

	public void setWinnerText() {
		GameObject winnerText = GameObject.Find ("WinnerText");
		Text content = winnerText.GetComponent<Text> (); 
		content.text = "Player 2 wins!";
		content.enabled = true;
	}

	public void OnHit() {
		lives = lives - 1;
		if (lives == 1) {
			Destroy(GameObject.Find("Live2"));
		} else if (lives == 0) {
			Destroy (GameObject.Find ("Live1")); 
			setWinnerText ();
		}
	}
}
