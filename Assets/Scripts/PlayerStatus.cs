using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	int lives; 
	//nur Ganzzahlen!
	float bombRadius;
	public AudioClip dyingSound;
	public AudioClip biteDamageSound;
	public AudioClip bombDamageSound;
	string live1;
	string live2;
	string winningText;
	float quitTime;
	bool quitGame;
	GameObject spFlag;

	// Use this for initialization
	void Start () {
		lives = 2; 
		bombRadius = 1f;
		quitTime = 5.0f; 
		quitGame = false; 
		spFlag = GameObject.Find ("SinglePlayerFlag");

		if(gameObject.name.Equals("Bomberman_Player1")){
			live1="Live1";
			live2 = "Live2";

			if (spFlag == null) {
				winningText = "Player 2 wins";
			} else {
				winningText = "You are blobed!";
			}
		}else if(gameObject.name.Equals("Bomberman_Player2")){
			live1="Live3";
			live2 = "Live4";
			winningText = "Player 1 wins";
		}

	}

	public void powerUp() {
		bombRadius++; 
	}

	public void OnHit(string cause) {
		if (cause.Equals ("SimpleAI")) {
			AudioSource.PlayClipAtPoint(biteDamageSound, transform.position);
		}else if(cause.Equals ("Bomb")){
			AudioSource.PlayClipAtPoint(bombDamageSound, transform.position);
		}
		lives = lives - 1;
		if (lives == 1) {
			Destroy(GameObject.Find(live1));
		} else if (lives == 0) {
			AudioSource.PlayClipAtPoint(dyingSound, transform.position);
			Destroy (GameObject.Find (live2)); 
			//gameObject.transform.Rotate(new Vector3(90, 0, 90));
			quitGame = true; 
			GameObject winnerText = GameObject.Find ("WinnerText");
			Text text = winnerText.GetComponent<Text> ();
			text.text = winningText;
			text.enabled = true; 
		}
	}

	//BombExplosion.cs soll sich z.B. die aktuellen Bombeneigenschaften holen, die Items ja ändern können.
	public float getBombRadius(){
		return bombRadius;
	}

	//Z.b. Items ändern evtl. den Radius.
	public void setBombRadius(float radius){
		bombRadius = radius;
		if (bombRadius > 10) {
			bombRadius = 10;
		} else if (bombRadius < 1) {
			bombRadius = 1;
		}
	}

	void spWin() {
		GameObject winnerText = GameObject.Find ("WinnerText");
		Text text = winnerText.GetComponent<Text> ();
		text.text = "You win!";
		text.enabled = true; 
	}
	
	// Update is called once per frame
	void Update () {
		if (spFlag != null && GameObject.FindWithTag ("Enemy_AI") == null) {
			quitGame = true; 
			spWin (); 
		}

		if (quitGame) {
			quitTime -= (Time.deltaTime % 60); 
			if (quitTime <= 0) {
				SceneManager.LoadScene ("Menu"); 
			} 
		}
	}
}
