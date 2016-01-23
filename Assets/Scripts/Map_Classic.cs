using UnityEngine;
using System.Collections;

public class Map_Classic : MonoBehaviour {

	public GameObject grassCrossPrefab;

	public float mapAreaPosX, 
				 mapAreaNegX, 
				 mapAreaPosZ, 
				 mapAreaNegZ;


	// Use this for initialization
	void Start () {
		mapAreaPosX = 5.5f;
		mapAreaNegX = -5.5f;
		mapAreaPosZ = 6.5f;
		mapAreaNegZ = -6.5f;
		GameFinisher gf = GetComponent<GameFinisher> ();
		gf.mapAreaPosX = mapAreaPosX;
		gf.mapAreaNegX = mapAreaNegX;
		gf.mapAreaPosZ = mapAreaPosZ;
		gf.mapAreaNegZ = mapAreaNegZ;

		randomPlacingOfGrassClums (60);
	}

	//generieren statt modellieren
	void randomPlacingOfGrassClums(int amount){

		for(int i = 0; i < amount; i++){
			GameObject parent = GameObject.Find ("Decoration");
			GameObject g = GameObject.Instantiate (grassCrossPrefab);
			g.transform.parent = parent.transform;

			Vector3 gPos = generatePosition();
			g.transform.position = gPos;
			g.transform.rotation = Quaternion.identity;
			g.transform.Rotate (new Vector3 (0, Random.value * Random.Range (0, 359), 0));

			//jedes dritte büschel soll standardmaße haben
			if (Random.value > 0.333f) {
				float scaleX = g.transform.localScale.x + Random.Range (-0.2f, 0.2f);
				float scaleY = g.transform.localScale.y + Random.Range (-0.1f, 0.0f);
				float scaleZ = g.transform.localScale.z + Random.Range (-0.2f, 0.2f);
				g.transform.localScale = new Vector3 (scaleX, scaleY, scaleZ);
			}

			//y-position korrigieren
			Vector3 p = g.transform.position;
			p.y = g.transform.localScale.y / 2;
			g.transform.position = p;
				
		}
	}

	Vector3 generatePosition(){

		return new Vector3 ( Random.Range (mapAreaNegX, mapAreaPosX),
							 0,
							 Random.Range (mapAreaNegZ, mapAreaPosZ));
	}


	// Update is called once per frame
	void Update () {
	
	}
}
