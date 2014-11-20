using UnityEngine;
using System.Collections;

public class BombThrown : MonoBehaviour {

	public GameObject explosion;
	public ParticleSystem[] effects;

	float speed = 52f;

	float ttl = 1.3f;
	float lived = 0f;

	bool thrown;

	float defaultGScale;


	// Use this for initialization
	void Awake () {
		thrown = false;
		defaultGScale = rigidbody2D.gravityScale;
		rigidbody2D.gravityScale = 0f;
	}

	public void Throw(Vector3 targetLoc, Vector3 playerVelocity) {
		rigidbody2D.gravityScale = defaultGScale;
		thrown = true;

//		Debug.Log("TargetLoc: " + targetLoc + ". MyPos: " + transform.position);

		Vector2 myVel = (targetLoc - transform.position).normalized;
//		myVel.x += myVel.x > 0 ? 1 : -1;
//		myVel.y += 1;


//		Vector2 myOrigin = new Vector2(2f,2f);
//		Vector2 myTarget = new Vector2(2f, 4f);
//		Vector2 myTarget2 = new Vector2(2f,7f);

//		Debug.Log(myTarget + "-" + myOrigin + " = " + (myTarget-myOrigin) + ", Norm: " + (myTarget-myOrigin).normalized);
//		Debug.Log(myTarget2 + "-" + myOrigin + " = " + (myTarget2-myOrigin) + ", Norm: " + (myTarget2-myOrigin).normalized);

//		Debug.Log("Vel+: " + myVel);
//		Debug.Log ("Speed: " + myVel * speed);
		myVel *= speed;
		myVel.y += (playerVelocity.y > 0f) ? (playerVelocity.y * 0.3f) : 0f;
		rigidbody2D.velocity = myVel;


//		Debug.Log ("Normalized1000: " + ((targetLoc - transform.position)* 1000f ).normalized);
//		Debug.Log ("Normalized: " + ((targetLoc - transform.position)).normalized);
//		rigidbody2D.velocity = ((targetLoc - transform.position) * 1000f).normalized * speed;
	}
	
	// Update is called once per frame
	void Update () {
		if(thrown) {
			lived += Time.deltaTime;
			if(ttl <= lived) {
				Explode();
				thrown = false;
				transform.position = GameController.hideVector;
				rigidbody2D.gravityScale = 0f;
			}
		}
	}

	void Explode() {
		Instantiate (explosion, transform.position, transform.rotation);
		foreach (var effect in effects) {
			effect.transform.parent = null;
			effect.Stop ();
			Destroy (effect.gameObject, 1.0f);
		}
		Destroy (gameObject);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.transform.tag == "Enemy") {
			Explode ();
			Vector2 forceVec = (other.transform.position - transform.position) * 5000f;
			//			other.transform.rigidbody2D.AddForce(forceVec);
			other.transform.GetComponent<Enemy>().Die(forceVec);
			GameController.current.bombEnemy();
			//			other.transform.rigidbody2D.velocity = new Vector2(0f,0f);
		}
	}
}
