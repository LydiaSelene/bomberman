using UnityEngine;
using System.Collections;

public class PowerUpRandomizer : MonoBehaviour {

	public GameObject powerUpPrefab; 
	public int numberPowerUps; 

	// Use this for initialization
	void Start () {
		// Zufällig Verteilung der PowerUps in den Kisten
		for (int i = 0; i < numberPowerUps; i++) {
			int nr = (int) Mathf.Floor(Random.Range (2f, 75f));
			GameObject box = GameObject.Find ("DestroyableBox (" + nr + ")");
			GameObject coin = GameObject.Instantiate (powerUpPrefab);
			coin.transform.position = box.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
