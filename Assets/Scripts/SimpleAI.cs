using UnityEngine;
using System.Collections;

public class SimpleAI : MonoBehaviour {

	int lives;
	bool died;
	Renderer renderer;
	float damageIntervall;
	float damageTimer;

	public AudioClip dyingSound;

	private NavMeshAgent agent;
	//handlungsradius für bewegung
	float range;
	float minRange;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		range = 2f;
		minRange = 0.2f;
		agent.SetDestination (new Vector3(-3.2f,0f,5.7f));
		lives = 1;
		died = false;
		renderer = GetComponent<Renderer>();

		damageIntervall = 2.0f;
		damageTimer = 0f;
	}

	//Die Funktion gibt einen zufälligen Punkt in einer Sphäre.
	bool RandomPoint(Vector3 center, float range, out Vector3 result) {
		for (int i = 0; i < 30; i++) {
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
				result = hit.position;
				if (Vector3.Distance (center, result) > minRange*2f) {
					return true;
				} else {
					//Debug.Log ("neue Distanz zu klein");
				}
			}
		}
		result = Vector3.zero;
		Debug.Log ("Kein Zielpunkt gefunden");
		return false;
	}

	public void OnHit() {
			lives = lives - 1;
			if (lives <= 0) {
				died = true;
				AudioSource.PlayClipAtPoint(dyingSound, transform.position);
			}
	}

	void dying(){
		Material material = renderer.material;
		Color color = material.color;
		if (color.a > 0f) {
			color.a -= 0.015f;
			material.SetColor ("_Color", color);
		} else {
			Destroy(gameObject);
		}
	}

	//z.b. um den Spieler zu verletzen
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Player")) {
			//funktioniert so nur für einen spieler. bei zweien zählt die zeit für beide
			if (damageTimer <= 0f) {
				PlayerStatus status =  other.GetComponent<PlayerStatus>();
				status.OnHit("SimpleAI");
				damageTimer = damageIntervall;
			}
		}
	}

	
	// Update is called once per frame
	void Update () {

		if(damageTimer > 0f){
			damageTimer -= Time.deltaTime;
		}

		if (died) {
			agent.destination = transform.position;
			dying ();
			
		}else if (Vector3.Distance (agent.destination, transform.position) <= minRange || agent.destination == null) {
			//Debug.Log ("distanz: "+ Vector3.Distance (agent.destination, transform.position));
			Vector3 point;
			if (RandomPoint (transform.position, range, out point)) {
				Debug.DrawRay (point, Vector3.up, Color.blue, 1.0f);
				agent.SetDestination (point);
				//Debug.Log ("position "+point+" als Ziel gesetzt");
			} else {
				Debug.Log ("Kein neues Ziel gesetzt");
			}
		} else {
			//Debug.Log("übergangen");
		}


			
	}
}
