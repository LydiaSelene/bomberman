using UnityEngine;
using System.Collections;

public class visualExplosionSphere : MonoBehaviour {

	Transform thisTransform;
	public Renderer rend;

	// Use this for initialization
	void Start () {
	
		thisTransform = gameObject.transform;
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vec = thisTransform.localScale;
		Material mat = rend.material;
		Color col = mat.color;

		if (vec.x < 1.4) {
			vec.x += 0.07f;
			vec.y += 0.07f;
			vec.z += 0.07f;
			thisTransform.localScale = vec;
		}

		if (vec.x > 0.8 && col.a > 0) {
			col.a -= 0.08f;
			mat.SetColor ("_Color", col);

		}

		if(vec.x >= 1.4 && col.a <= 0) {
				Destroy(gameObject);
		}
	}


}

