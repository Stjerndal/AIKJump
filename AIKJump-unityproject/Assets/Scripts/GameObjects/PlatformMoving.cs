using UnityEngine;
using System.Collections;

public class PlatformMoving : Platform {

	public float platformSpeed = 3f;
	public int startDir = 1; //right: 1, left: -1

	void Start () {
		base.Start();
		rigidbody2D.velocity = Vector2.right * startDir * platformSpeed;
	}

	void FixedUpdate() {
		//TODO Fix so that the edge instead of the center is out of screen
		if(transform.position.x <= myMinHori) {
			transform.position = new Vector2(myMinHori, transform.position.y);
			rigidbody2D.velocity = Vector2.right * platformSpeed;
		}
		
		if(transform.position.x >= myMaxHori) {
			transform.position = new Vector2(myMaxHori, transform.position.y);
			rigidbody2D.velocity = Vector2.right * -platformSpeed;
		}
	}

	public void SetSpeed(float speed) {
		platformSpeed = speed;
		rigidbody2D.velocity = rigidbody2D.velocity.x > 0 ? Vector2.right * platformSpeed : Vector2.right * -platformSpeed;
	}
	
}
