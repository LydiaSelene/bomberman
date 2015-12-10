using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	GameObject destroyableBox;
	public int explosionRadius;
	public float timeToDetonation; 

	void Start () {
		explosionRadius = 1;
		timeToDetonation = 3.0f; 
	}

	void Update () {
		timeToDetonation = timeToDetonation - (Time.deltaTime % 60); 
		if (timeToDetonation > 0) {
			// Countdown wird heruntergezählt
		} else {
			Debug.Log("BOOOOOM");
			Destroy(gameObject);
		}
	}

	void OnDestroy() {
		// Identifizierung des Rasterfelds, auf dem die Bombe liegt
		float xField = Mathf.Round (transform.position.x);
		float zField = Mathf.Round (transform.position.z); 
		Debug.Log ("Bombe liegt auf Feld: (" + xField + " | " + zField + ")");

		// Überprüfung, ob Spieler von Bombe getroffen wurde. 
		GameObject player = GameObject.Find ("Player"); 
		float xPlayerField = Mathf.Round (player.transform.position.x);
		float zPlayerField = Mathf.Round (player.transform.position.z);

		if ((Mathf.Abs(xPlayerField - xField) <= explosionRadius && Mathf.Abs(zPlayerField - zField) == 0) ||
		    (Mathf.Abs(xPlayerField - xField) == 0 && Mathf.Abs(zPlayerField - zField) <= explosionRadius)) {
			Debug.Log ("Spieler im Explosionsbereich"); 
			PlayerStatus status = player.GetComponent<PlayerStatus>();
			status.OnHit();
		}
		
		// Ermitteln der Boxen im Explosionsradius
		GameObject destroyableBoxes = GameObject.Find ("DestroyableBoxes"); 
		foreach (Transform box in destroyableBoxes.GetComponentsInChildren<Transform>()) {
			if (box.name.Equals("DestroyableBoxes")) {continue;}
			if ((Mathf.Abs(box.position.x - xField) <= explosionRadius && Mathf.Abs(box.position.z - zField) == 0) ||
			    (Mathf.Abs(box.position.x - xField) == 0 && Mathf.Abs(box.position.z - zField) <= explosionRadius)) {
				// Explosion durchführen mit Zerstörung der zu zerstörenden Boxen
				Destroy(box.gameObject);

			}
		}
	}
}
