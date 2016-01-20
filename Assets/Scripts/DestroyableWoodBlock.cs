using UnityEngine;
using System.Collections;
//für listen
using System.Collections.Generic;

public class DestroyableWoodBlock : MonoBehaviour {

	public GameObject shrapnelPrefab;
	Vector3 blockPosition;

	// Use this for initialization
	void Start () {
		blockPosition = transform.position;
	}


	//oberflächenpositionen für schrapnelle auf den Blöcken ermitteln
	//auch die rotation wird behandelt
	void generateSurfacePosition(GameObject obj){
		Vector3 pos = blockPosition;
		float xmod = Random.Range (-0.4f, 0.4f);
		float ymod = Random.Range (-0.4f, 0.4f); 
		float zmod = Random.Range (-0.4f, 0.4f);

		//Blockseite wählen: 1,2 ist +-X,  3,4 +-Z,  5,6 +-Y  
		int side = Mathf.RoundToInt(Random.Range (0.51f, 6.49f));

		switch (side){
		case 1:
			xmod = 0.45f;
			obj.transform.Rotate ( new Vector3(0, 0, 90.0f) );
			obj.transform.Rotate ( new Vector3(Random.Range (-45, 45), 0, 0) );
			break;
		case 2: 
			xmod = -0.45f;
			obj.transform.Rotate ( new Vector3(0, 0, -90.0f) );
			obj.transform.Rotate ( new Vector3(Random.Range (-45, 45), 0, 0) );
			break;
		case 3: 
			zmod = 0.45f;
			//z,x,y ist die rotationsreihenfolge
			obj.transform.Rotate ( new Vector3(0, 90.0f, 90.0f) );
			obj.transform.Rotate ( new Vector3(0, 0, Random.Range (-45, 45)) );
			break;
		case 4: 
			zmod = -0.45f;
			//z,x,y ist die rotationsreihenfolge
			obj.transform.Rotate ( new Vector3(0, -90.0f, -90.0f) );
			obj.transform.Rotate ( new Vector3(0, 0, Random.Range (-45, 45)) );
			break;
		case 5: 
			ymod = 0.45f;
			obj.transform.Rotate ( new Vector3(0, Random.Range (-45, 45), 0) );
			break;
		case 6: 
			//soll nicht durch den Boden gehen
			//wird leicht über position gesetzt -> besserer Explosionseffekt
			ymod = 0.1f;
			obj.transform.Rotate ( new Vector3(-90.0f, 0, 0) );
			obj.transform.Rotate ( new Vector3(0, Random.Range (-45, 45), 0) );
			break;
		default:
			break;
		}

		obj.transform.position = new Vector3 (pos.x+xmod, pos.y+ymod, pos.z+zmod);
		//für mehr verhaltensvielfalt
		obj.transform.Rotate ( new Vector3(Random.Range (-5, 5), Random.Range (-5, 5), Random.Range (-5, 5)) );

	}

	public void destroy(Vector3 bombPosition, float explosionRadius, float explosionPower, float explosionUpModifier){
		Destroy(gameObject);

		List<GameObject> shrapnels = new List<GameObject>();

		while(shrapnels.Count < 11){
			shrapnels.Add (GameObject.Instantiate (shrapnelPrefab));
		}

		foreach(GameObject shrap in shrapnels){
			shrap.transform.rotation = Quaternion.identity;
			generateSurfacePosition (shrap);
			Rigidbody rid = shrap.GetComponent<Rigidbody> ();
			rid.AddExplosionForce(explosionPower, bombPosition, explosionRadius+0.5f, explosionUpModifier, ForceMode.Force);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
