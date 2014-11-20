using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float xSpeed = 88f;

//	float springForce = 2000f;
//	float jumpForce = 1500f;
//	float enemyForce = 1700f;

//	float jumpSpeedY = 29f; //grav 30
	float jumpSpeedY = 45f;
	float springSpeedY = 20f;
	float enemySpeedY = 20f;

	public Animator charAnim;

	public static int PLAYER_STATE_JUMP = 0;
	public static int PLAYER_STATE_FALL = 1;
	public static int PLAYER_STATE_HIT = 2;

	public Transform bombStartLeft;
	public Transform bombStartRight;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

//	bool isGameOver = false;
	public int state;

	public Vector2 velocity;
	
	public bool facingRight = true;

	float h; // to hold accelerometer data


	public float width = 1.7f;
	float myLeftBorder;
	float myRightBorder;

	private Animator springAnim;

	// Use this for initialization
	void Start () {
		state = PLAYER_STATE_FALL;
		velocity = new Vector2();
		springSpeedY = 1.5f * jumpSpeedY;
		enemySpeedY = 1.3f * jumpSpeedY;

		myLeftBorder = GameController.worldHorizMin - width/2;
		myRightBorder = GameController.worldHorizMax + width/2;
	}

	void FixedUpdate() {
		if(state != PLAYER_STATE_HIT) {
			velocity = rigidbody2D.velocity; 
			#if UNITY_EDITOR
			h = Input.GetAxis("Horizontal");
			velocity.x = h * xSpeed * 0.5f;
			#elif UNITY_ANDROID || UNITY_IPHONE
			h = Input.acceleration.x;
			velocity.x = h * xSpeed;
			#endif
			rigidbody2D.velocity = velocity;
	//		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
	//		charAnim.SetBool("Ground", grounded);

			charAnim.SetFloat("vSpeed", rigidbody2D.velocity.y);

			if(transform.position.x < myLeftBorder)
				transform.position = new Vector2(myRightBorder, transform.position.y);
			if(transform.position.x > myRightBorder)
				transform.position = new Vector2(myLeftBorder, transform.position.y);

			if(h > 0.05f && !facingRight) {
				Flip();
			} else if(h < -0.05f&& facingRight) {
				Flip();
			}
		}
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	// Update is called once per frame
//	void Update () {
//
//		if(!isGameOver && grounded && Input.anyKeyDown) {
//			charAnim.SetBool ("Ground", false);
//			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
//			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
////			rigidbody2D.AddForce(Vector2.up * 1000);
//		}
//	}

	public void CollideEnemy(Enemy enemyScript) {
	//		Debug.Log("Collided with enemy");
		if(!enemyScript.isDead && state != PLAYER_STATE_HIT) {
			GameController.current.playerHit();
			rigidbody2D.velocity = Vector2.zero;
			gameOver();
		}
	}

	public void landOnEnemy(Enemy enemyScript) {
		if(state != PLAYER_STATE_HIT) {
			GameController.current.jumpOnEnemy();
	//		Debug.Log("Landed on enemy");
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, enemySpeedY);
	//		rigidbody2D.AddForce(new Vector2(0f, enemyForce));
			charAnim.SetTrigger("Jump");
			enemyScript.Die();
		}
	}

	public void landOnPlatform(Platform platform) {
		if(state != PLAYER_STATE_HIT) {
			platform.Jump();
			GameController.current.jumpOnPlatform();
			charAnim.SetTrigger("Jump");
	//		Debug.Log("Landed on platform");
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeedY);
	//		rigidbody2D.AddForce(new Vector2(0f, jumpForce));
	//		charAnim.SetTrigger("Jump");
		}
	}

	public void landOnGround() {
		if(state != PLAYER_STATE_HIT) {
			GameController.current.jumpOnPlatform();
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeedY);
			charAnim.SetTrigger("Jump");
		}
	}

	public void landOnSpring(Animator springAnim) {
			if(state != PLAYER_STATE_HIT) {
			GameController.current.jumpOnSpring();
	//		Debug.Log("Landed on spring");
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, springSpeedY);
	//		rigidbody2D.AddForce(new Vector2(0f, springForce));
			charAnim.SetTrigger("Jump");
			springAnim.SetTrigger("Spring");
		}
	}

	public void landOnSpringDelayed(Animator springAnim) {
		this.springAnim = springAnim;
		Invoke ("delayedLandOnSpring", 0.2f);
	}

	private void delayedLandOnSpring() {
		GameController.current.jumpOnSpring();
		//		Debug.Log("Landed on spring");
		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, springSpeedY);
		//		rigidbody2D.AddForce(new Vector2(0f, springForce));
		charAnim.SetTrigger("Jump");
		springAnim.SetTrigger("Spring");
	}

	public void CollectBomb(Transform bombTransform) {
//		Debug.Log("Bomb Collected!");
		GameController.current.collectBomb();
		bombTransform.position = new Vector3(0, -30f, 0);
		
	}

	public void gameOver() {
		state = PLAYER_STATE_HIT;
		charAnim.SetBool("Dead", true);
		Debug.Log("Player too far down!");

		GameController.current.GameOver();
	}

	public void throwBomb(bool toTheRight) {
		if(state != PLAYER_STATE_HIT) {
			GameController.current.throwBomb();
	//		Debug.Log ("Throw a bomb to the " + (toTheRight ? "to the right" : "to the left" ));
			charAnim.SetTrigger(toTheRight ? "ThrowRight" : "ThrowLeft");
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(state != PLAYER_STATE_HIT) {
	//		Debug.Log("Something hit the player");
	//		Transform otherParentTransform = other.transform.parent.transform;
			Transform otherParentTransform = other.transform;
			string otherTag = otherParentTransform.tag;
			if(otherTag == "BombStatic") {
				CollectBomb(otherParentTransform);
			}
	//		if((otherParentTransform.position.y < transform.position.y) && rigidbody2D.velocity.y < 0f) {
	//			if(otherTag == "Platform" || otherTag == "Ground") {
	//				landOnPlatform();
	//			} else if(otherTag == "Spring") {
	//				landOnSpring();
	//			} else if(otherTag == "Enemy") {
	//				landOnEnemy();
	//			} else if (otherTag == "GameOverCheck") {
	//				gameOver();
	//			}
	//		} 
			else if (otherTag == "Enemy") {
				CollideEnemy(other.GetComponent<Enemy>());
			}
		}
	}
}