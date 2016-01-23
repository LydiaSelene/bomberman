using UnityEngine;
using System.Collections;


public class BombExplosion : MonoBehaviour {
	
	public float explosionRadius;
	public float timeToDetonation; 
	//for rigid physics
	public float explosionPower;
	public float explosionUpModifier;
	//Rasterzellen-koordinaten, in der die Bombe liegt
	float xField, yField, zField;
	public AudioClip explosionSound;
	//PlayerStatus owningPlayerStatus;


	void Start () {
		/*
		explosionRadius = 1f;
		timeToDetonation = 3.0f; 
		//je radiuspunkt um 5 erhöhen ?
		explosionPower = 150f;
		//je radiuspunkt um 0.33 erhöhen ?
		explosionUpModifier = 0.66f;
		*/

		timeToDetonation = 3.0f; 

		/*
		explosionRadius = owningPlayerStatus.getBombRadius();
		explosionPower = 145f + (explosionRadius * 5f);
		explosionUpModifier = 0.33f + (explosionRadius * 0.33f);
		*/
	}

	//muss z.b. das ThrowBomb-Script des Spielers bei der Instanziierung aufrufen und jenen Script übergeben.
	public void setOwningPlayerStatus(PlayerStatus script){
		//owningPlayerStatus = script;
	}

	public void setBombRadius(float radius){
		explosionRadius = radius;
		explosionPower = 145f + (radius * 5f);
		explosionUpModifier = 0.33f + (radius * 0.33f);
	}

	public void setBombTimer(float time){
		timeToDetonation = time;
	}

	void Update () {
		timeToDetonation = timeToDetonation - (Time.deltaTime % 60); 
		if (timeToDetonation > 0) {
			// Countdown wird heruntergezählt
		} else {
			//Debug.Log("BOOOOOM");
			Destroy(gameObject);
		}
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

	/* Berechnet eine Zelle des Spielfelds, welche Teil des Explosionsbereichs einer Bombe ist.
	* pos has to be a rounded Position of the rastered Playfield (Grid) and in explosion area.
	*/
	bool calcExplosionCell (Vector3 pos) {

		Collider[] objectsInGridCell = Physics.OverlapBox(pos, new Vector3(0.5f, 0.5f, 0.5f));

		//objekte durchgehen
		foreach(Collider obj in objectsInGridCell){
			// RasterZelle in der das Objekt liegt
			Transform tf = obj.GetComponent<Transform>();
			//es muss getestet werden, ob das objekt in derselben rasterzelle wie die jetzt behandelte liegt
			if( !isInSameField(tf.position.x, tf.position.z, pos.x, pos.z) ){
				//objekt nicht in der Rasterzelle, überspringen
				continue;
			}

			//zuerst auf SolidBlock und LevelWall prüfen
			//TODO: zum prüfen von LevelWall muss diese anders behandelt werden (wegen objekt-mittelpunkten, die nicht zum grid passen)
			if(obj.tag.Equals("SolidBlock") || obj.tag.Equals("LevelBoundary") /*|| obj.tag.Equals("LevelWall")*/){
				//der explosionsarm kann an dieser Stelle abgebrochen werden
				//hinter einem SolidBlock wirkt die Explosion nicht
				return false;

		    //auf zerstörbare Blöcke prüfen
			}else if(obj.tag.Equals("DestroyableBlock")){
				DestroyableWoodBlock script = obj.GetComponent<DestroyableWoodBlock>();
				script.destroy (transform.position,explosionRadius, explosionPower, explosionUpModifier);

		    //auf rigid-Bodies prüfen (erstmal allg. alle gleich behandeln)
			}else if(obj.attachedRigidbody != null){
				//explosionskraft auf das objekt wirken
				//radius+0.5 zu erreichen des zellenrandes
				obj.attachedRigidbody
					.AddExplosionForce(explosionPower, 
						               transform.position, explosionRadius+0.5f, explosionUpModifier, ForceMode.Force);
			
		    // Überprüfung, ob Spieler von Bombe getroffen wurde.
			//TODO: evtl auch Tag
			}else if(obj.name.Equals("Player")){
				PlayerStatus status =  obj.GetComponent<PlayerStatus>();
				status.OnHit();

			}else{
				Debug.Log ("Attention! I dont know what to do with this object: "+obj );
			}
		}
		return true;
	}
	
	void OnDestroy() {
		
		//explosionssound abspielen: muss statisch erfolgen, da bereits das gameobject gelöscht wird
		//TODO: evtl. andere, objektbezogene lösung, um audiosource-einstellungen nutzen zu können
		//z.b. ein empty erzeugen, was sich nach DepthTextureMode abspielen löscht
		AudioSource.PlayClipAtPoint(explosionSound, transform.position);

		//Rasterzellen-koordinaten, in der die Bombe liegt
		xField = Mathf.Round (transform.position.x);
		yField = Mathf.Floor (transform.position.y);
		zField = Mathf.Round (transform.position.z);
		//damit die Mitte der 3D-Zelle zählt
		yField += 0.5f;

		//zum prüfen, ob ein SolidBlock o.ä eine Weiterberechnung eines Explosionsarms erübrigt
		bool continuePosX = true;
		bool continueNegX = true;
		bool continuePosZ = true;
		bool continueNegZ = true;
		for (int i=0; i <= explosionRadius; i++) {
			//bombenposition
			Vector3 gridPosition = new Vector3 (xField, yField, zField);
			//Debug.Log ("gridPosition: "+gridPosition);

			//in +-x und +-z relativ zur bombe prüfen
			//x positiv (Explosionsarm)
			if(continuePosX){
				Vector3 cellPosX = new Vector3 (gridPosition.x + i, gridPosition.y, gridPosition.z);
				//false, wenn ein Explosionshindernis da ist, der Arm soll dann beendet werden
				continuePosX = calcExplosionCell (cellPosX);
				if(i==0){
					//bei i=0 würde 4x die Zelle der Bombe geprüft werden, überspringen
					continue;
				}
			}
			//x negativ (Explosionsarm)
			if(continueNegX){
				Vector3 cellNegX = new Vector3 (gridPosition.x - i, gridPosition.y, gridPosition.z);
				continueNegX = calcExplosionCell (cellNegX);
			}
			//z positiv (Explosionsarm)
			if(continuePosZ){
				Vector3 cellPosZ = new Vector3 (gridPosition.x, gridPosition.y, gridPosition.z + i);
				continuePosZ = calcExplosionCell (cellPosZ);
			}
			//z negativ (Explosionsarm)
			if(continueNegZ){
				Vector3 cellNegZ = new Vector3 (gridPosition.x, gridPosition.y, gridPosition.z - i);
				continueNegZ = calcExplosionCell (cellNegZ);
			}
		}

	}
}
