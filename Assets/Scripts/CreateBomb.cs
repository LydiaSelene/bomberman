﻿//#version 1.1
using UnityEngine;
using System.Collections;

public class CreateBomb : MonoBehaviour {

	public GameObject BombPrefab;
	public GameObject bomb;
	public int throwTrust = 160;

	// Use this for initialization
	void Start () {
	
	}

	//IEnumerator nochmal nachlesen
	private void createGameObject(){

		//create Bomb before Player
		GameObject player = GameObject.Find ("Player");

		//calculate spawn position
		Vector3 spawnPosition = player.transform.position;
		//+ 1/4 player height (höhe auf Brust, statt Bauch)
		spawnPosition.y += player.transform.lossyScale.y / 4;
		//+ x in forward dir, bomb before playr
		spawnPosition += player.transform.forward.normalized/2;

		//ein prefab in der szene erzeugen
		bomb = GameObject.Instantiate(BombPrefab);
		bomb.transform.position = spawnPosition;
		bomb.transform.rotation = Quaternion.identity;
		bomb.transform.Rotate ((Random.value*80)-(Random.value*80),
		                       (Random.value*80)-(Random.value*80),
		                       (Random.value*80)-(Random.value*80));
		//werfen
		Rigidbody bombRB = bomb.GetComponent<Rigidbody> ();
		bombRB.AddForce(player.transform.forward.normalized * throwTrust, ForceMode.Force);
	}
	
	// Update is called once per frame
	void Update(){
		//GetKeyDown löst nur 1x aus, wenn gedrückt
		if (Input.GetKeyDown(KeyCode.Space)){
			createGameObject();
		}
	}
}
