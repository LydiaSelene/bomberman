using UnityEngine;
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

	// Use this for initialization
	void Start () {
		lives = 2; 
		bombRadius = 1f;

		if(gameObject.name.Equals("Bomberman_Player1")){
			live1="Live1";
			live2 = "Live2";
		}else if(gameObject.name.Equals("Bomberman_Player2")){
			live1="Live3";
			live2 = "Live4";
		}

	}

	public void OnHit(string cause) {
		if (cause.Equals ("SimpleAI")) {
			AudioSource.PlayClipAtPoint(biteDamageSound, transform.position);
		}else if(cause.Equals ("Bomb")){
			AudioSource.PlayClipAtPoint(bombDamageSound, transform.position);
		}
		lives = lives - 1;
		//TODO: zu statisch!
		if (lives == 1) {
			Destroy(GameObject.Find(live1));
		} else if (lives == 0) {
			AudioSource.PlayClipAtPoint(dyingSound, transform.position);
			Destroy (GameObject.Find (live2)); 
			Destroy(gameObject); 
			gameObject.transform.Rotate(new Vector3(90, 0, 90));
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
	
	// Update is called once per frame
	void Update () {
		
	}


}
