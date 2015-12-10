using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public int lives; 

	// Use this for initialization
	void Start () {
		lives = 2; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnHit() {
		lives = lives - 1;
		if (lives == 1) {
			Destroy(GameObject.Find("Live2"));
		} else if (lives == 0) {
			Destroy (GameObject.Find ("Live1")); 
			GameObject player = GameObject.Find("Player"); 
			CameraMove cam = player.GetComponent<CameraMove>();
			Destroy(cam); 
			player.transform.Rotate(new Vector3(90, 0, 90));
		}
	}
}
