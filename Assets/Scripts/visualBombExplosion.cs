using UnityEngine;
using System.Collections;

public class visualBombExplosion : MonoBehaviour {

	public GameObject explosionSpherePrefab;

	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy(){
		Vector3 pos = transform.position;
		createExplosionSphereAtPoint (new Vector3(pos.x,pos.y,pos.z));

		createExplosionSphereAtPoint (new Vector3(pos.x+1,pos.y,pos.z));
		createExplosionSphereAtPoint (new Vector3(pos.x-1,pos.y,pos.z));
		createExplosionSphereAtPoint (new Vector3(pos.x,pos.y,pos.z+1));
		createExplosionSphereAtPoint (new Vector3(pos.x,pos.y,pos.z-1));

	}

	public void createExplosionSphereAtPoint(Vector3 pos){
		
		//Rasterzellen-koordinaten, in der die Bombe liegt
		float xField = Mathf.Round (pos.x);
		float yField = Mathf.Floor (pos.y);
		float zField = Mathf.Round (pos.z);
		//damit die Mitte der 3D-Zelle zählt
		yField += 0.5f;

		GameObject sphere = GameObject.Instantiate(explosionSpherePrefab);
		sphere.transform.position = new Vector3(xField,yField,zField);
		sphere.transform.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
