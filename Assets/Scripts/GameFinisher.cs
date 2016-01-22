using UnityEngine;
using System.Collections;

public class GameFinisher : MonoBehaviour {

	public GameObject bombPrefab;
	float time;

	// Use this for initialization
	void Start () {
	
	}

	public void beginFinish(){
		//die angegebene Funktion wird nach a sekunden aufgerufen und ab da alle b sekunden wieder
		InvokeRepeating("timer", 1, 1);
		InvokeRepeating("generateFallingBombs", 1, 1);
		time = 0;
	}

	void timer(){
		time += 1;
	}

	void generateFallingBombs(){

		int max = 1;
		if (time < 10) {
			max = 4;
		} else if (time < 20) {
			max = 8;
		} else {
			max = 12;
		}

		int amount = Random.Range (1, max);
		for (int i = 0; i < amount; i++) {
			generateFallingBomb ();
		}

	}

	//funktioiert nicht, es fallen keine bomben
	IEnumerator MyCoroutine(){
		int amount = Random.Range (1, 10);
		for (int i = 0; i < amount; i++) {
			generateFallingBomb ();
			yield return new WaitForSeconds(0.1f);
		}
	}


	void generateFallingBomb(){
		float xPos = Random.Range(-5.4f, 5.4f);
		float zPos = Random.Range(-6.4f, 6.4f);
		GameObject bomb = GameObject.Instantiate(bombPrefab);
		bomb.transform.position = new Vector3 (xPos, 8, zPos);
		bomb.transform.rotation = Random.rotation;
		Rigidbody rid = bomb.GetComponent<Rigidbody> ();
		rid.AddTorque(new Vector3(Random.Range(-1,1), Random.Range(-1,1), Random.Range(-1,1)), ForceMode.Impulse);
		rid.AddForce(new Vector3(Random.Range(-5,5), Random.Range(-5,0), Random.Range(-5,5)), ForceMode.Impulse );
	}
		
	
	// Update is called once per frame
	void Update () {

	}
}
