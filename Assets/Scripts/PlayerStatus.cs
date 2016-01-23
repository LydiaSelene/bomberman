using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	int lives; 
	//nur Ganzzahlen!
	float bombRadius;


	// Use this for initialization
	void Start () {
		lives = 2; 

		bombRadius = 1f;

	}

	public void OnHit() {
		lives = lives - 1;
		//TODO: zu statisch!
		if (lives == 1) {
			Destroy(GameObject.Find("Live2"));
		} else if (lives == 0) {
			Destroy (GameObject.Find ("Live1")); 
			GameObject player = GameObject.Find("Player"); 
			PlayerCameraMove cam = player.GetComponent<PlayerCameraMove>();
			Destroy(cam); 
			player.transform.Rotate(new Vector3(90, 0, 90));
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
