using UnityEngine;
using System.Collections;


public class BombExplosion : MonoBehaviour {
	
	GameObject destroyableBox;
	public float explosionRadius;
	public float timeToDetonation; 
	//for rigid physics
	public float explosionPower;
	public float explosionUpModifier;
	//Rasterzellen-koordinaten, in der die Bombe liegt
	float xField, yField, zField;
	public AudioClip explosionSound;
	AudioSource audio;
	
	void Start () {
		explosionRadius = 1f;
		timeToDetonation = 3.0f; 
		//je radiuspunkt um 5 erhöhen ?
		explosionPower = 150f;
		//je radiuspunkt um 0.33 erhöhen ?
		explosionUpModifier = 0.66f;

		//audio = GetComponent<AudioSource>();
		//audio.Play();

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

	/* Testet, ob ein Objekt im Explosionsradius in XZ liegt.
	 * xzObj: grid xz-position of the object
	  xzField: grid xz-position of the bomb*/
	bool isInRadius(float xObj, float zObj, float xField, float zField){
		if ((Mathf.Abs (xObj - xField) <= explosionRadius && Mathf.Abs (zObj - zField) == 0) ||
		    (Mathf.Abs (xObj - xField) == 0 && Mathf.Abs (zObj - zField) <= explosionRadius)) {
			return true;
		} else {
			return false;
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
		//der radius als umkreisradius, dann werden aber teile außerhalb der quadr. rasterzelle geprüft ->
		//die gerundete gridposition des objekts muss mit der des explosionsteils abgeglichen werden
		Collider[] objectsInGridCell = Physics.OverlapBox(pos, new Vector3(0.5f, 1.0f, 0.5f));
		//Debug.Log ("objectsInGridCell: "+objectsInGridCell.Length);
		//objekte durchgehen
		foreach(Collider obj in objectsInGridCell){
			// RasterZelle in der das Objekt liegt
			Transform tf = obj.GetComponent<Transform>();
			//es muss getestet werden, ob das objekt in derselben rasterzelle wie die jetzt behandelte liegt
			if( !isInSameField(tf.position.x, tf.position.z, pos.x, pos.z) ){
				//Debug.Log (obj+" ist nicht in der Rasterzelle");
				//objekt nicht in der Rasterzelle, überspringen
				continue;
			}
			//Debug.Log ("Obj in der Rasterzelle: "+obj);

			//zuerst auf SolidBlock und LevelWall prüfen
			//TODO: zum prüfen von LevelWall muss diese anders behandelt werden (wegen objekt-mittelpunkten, die nicht zum grid passen)
			if(obj.tag.Equals("SolidBlock") || obj.tag.Equals("LevelBoundary") /*|| obj.tag.Equals("LevelWall")*/){
				//der explosionsarm kann an dieser Stelle abgebrochen werden
				//hinter einem SolidBlock wirkt die Explosion nicht
				//SolidBlock nimmt eine ganze Zelle ein
				//Debug.Log ("SolidBlock/LevelWall, Arm abbrechen");
				return false;

		    //auf zerstörbare Blöcke prüfen
			}else if(obj.tag.Equals("DestroyableBlock")){
				//zerstörung des Blocks
				//Debug.Log ("DestroyableBlock, zerstören");
				DestroyableWoodBlock script = obj.GetComponent<DestroyableWoodBlock>();
				script.destroy (transform.position,explosionRadius, explosionPower, explosionUpModifier);

		    //auf rigid-Bodies prüfen (erstmal allg. alle gleich behandeln)
			}else if(obj.attachedRigidbody != null){
				//Rigidbody rb = hit.GetComponent<Rigidbody>();
				//explosionskraft auf das objekt wirken
				//radius+0.5 wegen erreichen des zellenrandes
				//Debug.Log ("RigidBody, Kraft rauf");
				obj.attachedRigidbody
					.AddExplosionForce(explosionPower+((explosionRadius*explosionPower/10)-explosionPower/10), 
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
		
		//explosion abspielen: muss statisch erfolgen, da bereits das gameobject gelöscht wird
		//TODO: evtl. andere, objektbezogene lösung, um audiosource-einstellungen nutzen zu können
		//z.b. ein empty erzeugen, was sich nach DepthTextureMode abspielen löscht
		AudioSource.PlayClipAtPoint(explosionSound, transform.position);
		//audio = GetComponent<AudioSource>();
		//audio.PlayOneShot (explosionSound, 1.0f);

		//Rasterzellen-koordinaten, in der die Bombe liegt
		xField = Mathf.Round (transform.position.x);
		yField = Mathf.Round (transform.position.y);
		zField = Mathf.Round (transform.position.z);

		/*
		notiz: bei bombenexplosion jeden richtungsarm zellenweise lang prüfen,
		wenn solidblock, arm abbrechen, dahinter ist nichts mehr betroffen
		-> übersichtlichere struktur ?
		-> collisions ohne solidblock -> if tag  box -> destroy -> if rigid -> force -> if player -> life
		 * */
		//zum prüfen, ob ein SolidBlock eine Weiterberechnung eines Explosionsarms erübrigt
		//hinter einem SolidBlock wirkt eine Explosion nicht
		bool continuePosX = true;
		bool continueNegX = true;
		bool continuePosZ = true;
		bool continueNegZ = true;
		for (int i=0; i <= explosionRadius; i++) {
			//bombenposition
			Vector3 gridPosition = new Vector3 (xField, yField, zField);
			Debug.Log ("gridPosition: "+gridPosition);

			//in +-x und +-z relativ zur bombe prüfen
			//x positiv (Explosionsarm)
			if(continuePosX){
				//Debug.Log ("jetzt X positiv mit i="+i+"-------------------------------------------------");
				Vector3 cellPosX = new Vector3 (gridPosition.x + i, gridPosition.y, gridPosition.z);
				//die methode liefert nur false, wenn ein SolidBlock vorkommt, der Arm soll dann beendet werden
				continuePosX = calcExplosionCell (cellPosX);
				if(i==0){
					//bei i=0 würde 4x die Zelle der Bombe geprüft werden, überspringen
					continue;
				}
			}else{
				//Debug.Log ("X positiv wurde zuvor abgebrochen");
			}
			//x negativ (Explosionsarm)
			if(continueNegX){
				//Debug.Log ("jetzt X negativ mit i="+i+"-------------------------------------------------");
				Vector3 cellNegX = new Vector3 (gridPosition.x - i, gridPosition.y, gridPosition.z);
				continueNegX = calcExplosionCell (cellNegX);
			}else{
				//Debug.Log ("X negativ wurde zuvor abgebrochen");
			}
			//z positiv (Explosionsarm)
			if(continuePosZ){
				//Debug.Log ("jetzt Z positiv mit i="+i+"-------------------------------------------------");
				Vector3 cellPosZ = new Vector3 (gridPosition.x, gridPosition.y, gridPosition.z + i);
				continuePosZ = calcExplosionCell (cellPosZ);
			}else{
				//Debug.Log ("Z positiv wurde zuvor abgebrochen");
			}
			//z negativ (Explosionsarm)
			if(continueNegZ){
				//Debug.Log ("jetzt Z negativ mit i="+i+"-------------------------------------------------");
				Vector3 cellNegZ = new Vector3 (gridPosition.x, gridPosition.y, gridPosition.z - i);
				continueNegZ = calcExplosionCell (cellNegZ);
			}else{
				//Debug.Log ("Z negativ wurde zuvor abgebrochen");
			}
		}

	}
}
