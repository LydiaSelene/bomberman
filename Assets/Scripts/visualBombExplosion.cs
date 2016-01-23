using UnityEngine;
using System.Collections;

public class visualBombExplosion : MonoBehaviour {

	public GameObject explosionSpherePrefab;

	// Use this for initialization
	void Start () {
		
	}

	/*
	void OnDestroy(){
		Vector3 pos = transform.position;
		createExplosionSphereAtPoint (new Vector3(pos.x,pos.y,pos.z));

		createExplosionSphereAtPoint (new Vector3(pos.x+1,pos.y,pos.z));
		createExplosionSphereAtPoint (new Vector3(pos.x-1,pos.y,pos.z));
		createExplosionSphereAtPoint (new Vector3(pos.x,pos.y,pos.z+1));
		createExplosionSphereAtPoint (new Vector3(pos.x,pos.y,pos.z-1));

	}*/

	public void createExplosionSphereAtPoint(Vector3 pos){
		/*
		//Rasterzellen-koordinaten, in der die Bombe liegt
		float xField = Mathf.Round (pos.x);
		float yField = Mathf.Floor (pos.y);
		float zField = Mathf.Round (pos.z);
		//damit die Mitte der 3D-Zelle zählt
		yField += 0.5f;

		//Die explosionSphere nur erzeugen, wenn die Explosion in dieser Zelle auch wirkt.
		if( calcExplosionCell(new Vector3(xField,yField,zField)) ){
			GameObject sphere = GameObject.Instantiate(explosionSpherePrefab);
			sphere.transform.position = new Vector3(xField,yField,zField);
			sphere.transform.rotation = Quaternion.identity;
		}*/

		GameObject sphere = GameObject.Instantiate(explosionSpherePrefab);
		sphere.transform.position = pos;
		sphere.transform.rotation = Quaternion.identity;
	}

	/* Berechnet eine Zelle des Spielfelds, welche Teil des Explosionsbereichs einer Bombe ist.
		 * pos has to be a rounded Position of the rastered Playfield (Grid) and in explosion area.
		 */
	bool calcExplosionCell (Vector3 pos) {
		Collider[] objectsInGridCell = Physics.OverlapBox(pos, new Vector3(0.5f, 0.5f, 0.5f));

		foreach(Collider obj in objectsInGridCell){
			Transform tf = obj.GetComponent<Transform>();
			//es muss getestet werden, ob das objekt in derselben rasterzelle wie die jetzt behandelte liegt
			if( !isInSameField(tf.position.x, tf.position.z, pos.x, pos.z) ){
				//objekt nicht in der Rasterzelle, überspringen
				continue;
			}
			//auf SolidBlock und LevelWall prüfen
			//TODO: zum prüfen von LevelWall muss diese anders behandelt werden (wegen objekt-mittelpunkten, die nicht zum grid passen)
			if(obj.tag.Equals("SolidBlock") || obj.tag.Equals("LevelBoundary") /*|| obj.tag.Equals("LevelWall")*/){
				return false;
			}
		}
		return true;
	}

	/* Prüft, ob ein Objekt in einer bestimmten Rasterzelle liegt.
	 * Die Koordinaten des Objekts müssen nicht auf eine Zelle gerundet sein,
	 * die der Zelle aber schon.
	 */
	//TODO: zählt nicht objekte die größer als ein feld sind, z.b. LevelWall -> verschobene positionen, manchmal zählts doch und falsch
	bool isInSameField(float xObj, float zObj, float xField, float zField){
		xObj = Mathf.Round (xObj);
		zObj = Mathf.Round (zObj);
		//Debug.Log ("Objpos: "+xObj+ " "+ zObj+" Fieldpos"+xField+" "+zField);
		if (xObj == xField && zObj == zField) {
			return true;
		} else {
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
