using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float enemySpeed = 5f;
	public Vector2 landedOnForce = Vector2.up * -3;
	public int startDir = 1; //right: 1, left: -1
	public Animator enemyAnim;
	public bool isDead;
	public BoxCollider2D myCollider;
	public AudioSource audioWings;

	float myMinHori;
	float myMaxHori;



	Vector2 defaultLscale;
	// Use this for initialization
	void Start () {
		rigidbody2D.velocity = Vector2.right * startDir * enemySpeed;
		defaultLscale = transform.localScale;
		isDead = false;
		BoxCollider2D box = GetComponent<BoxCollider2D>();
		myMinHori = GameController.worldHorizMin + box.size.x*2f;
		myMaxHori = GameController.worldHorizMax - box.size.x*2f;
		myCollider.isTrigger = false;
	}

	void FixedUpdate() {
		//TODO Fix so that the edge instead of the center is out of screen
		if(transform.position.x <= myMinHori) {
			transform.position = new Vector2(myMinHori, transform.position.y);
			rigidbody2D.velocity = Vector2.right * enemySpeed;
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
		}

		if(transform.position.x >= myMaxHori) {
			transform.position = new Vector2(myMaxHori, transform.position.y);
			rigidbody2D.velocity = Vector2.right * -enemySpeed;
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
		}
	}

	public void Die(Vector2 forceVec) {
		GameController.current.enemyDie();
		audioWings.Stop();
		isDead = true;
		rigidbody2D.angularVelocity = 600f;
		enemyAnim.SetBool("Dead", true);
		rigidbody2D.gravityScale = 1.5f;
		rigidbody2D.AddForce(forceVec * 0.2f);
		myCollider.isTrigger = true;
	}

	/* Death by landing on it */
	public void Die() {
		Die (landedOnForce);
	}

	public void Reborn() {
		enemyAnim.SetBool("Dead", false);
		isDead = false;
		rigidbody2D.gravityScale = 0f;
		transform.localScale = defaultLscale;
		rigidbody2D.velocity = Vector2.right * startDir * enemySpeed;
		rigidbody2D.angularVelocity = 0f;
		transform.rotation = Quaternion.identity;
		myCollider.isTrigger = false;
		if(GameController.current.audioManager.soundEnabled)
			audioWings.Play();
	}
}
