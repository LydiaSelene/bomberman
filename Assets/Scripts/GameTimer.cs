using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

	public Text timeText;
	float time;

	//wir aufgerufen, wenn eine Instanz geladen wird, läuft noch vor start(),
	//ignoriert, aber wohl Deaktivierung auf dem Objekt in Unity.
	void Awake(){

	}

	// Use this for initialization
	void Start () {
		time = 300;
		//die angegebene Funktion wird nach a sekunden aufgerufen und ab da alle b sekunden wieder
		InvokeRepeating("actualizeVisualCounter", 0, 1);
	}

	void actualizeVisualCounter() {

		if (time <= 0) {
			GameFinisher finisherScript = GetComponent<GameFinisher> ();
			if (finisherScript != null) {
				//beendet das invoke im script
				CancelInvoke("actualizeVisualCounter");
				finisherScript.beginFinish ();
			}

		}else {
			time -= 1;

			float minutes = time / 60 - (time / 60 % 1);
			float seconds = time % 60;

			if (seconds >= 10) {
				timeText.text = "" + minutes + ":" + seconds;
			} else {
				timeText.text = "" + minutes + ":0" + seconds;
			}

			//timeText.text = string.Format ("{0:00}:{1:00}", minutes, seconds);
		}

		if (time <= 30 && timeText.color != Color.red ) {
			timeText.color = Color.red;
		}
			
	}


	// Update is called once per frame
	void Update () {
		/*
		if (time <= 0) {
			//do
		} else {
			time -= 1*Time.deltaTime;

			float minutes = time / 60 - (time / 60 % 1);
			float seconds = time % 60 - (time % 60 % 1);

			timeText.text = ""+minutes+":"+seconds;
			//timeText.text = string.Format ("{0:00}:{1:00}", minutes, seconds);
		}
		*/
	}
}
