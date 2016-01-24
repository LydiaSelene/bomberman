using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour {

	public void startSinglePlayer() {
		Debug.Log ("Starting Singleplayer Game...");
		SceneManager.LoadScene ("Level"); 
	}

	public void startMultiPlayer() {
		Debug.Log ("Starting MultiPlayer-Match ..."); 
		SceneManager.LoadScene ("MPLevel");
	}

	public void QuitGame() {
		Application.Quit (); 
	}
}
