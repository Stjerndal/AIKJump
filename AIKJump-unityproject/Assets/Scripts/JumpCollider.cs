using UnityEngine;
using System.Collections;

public class JumpCollider : MonoBehaviour {

	public Player player;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(player.state != Player.PLAYER_STATE_HIT) {
	//		Debug.Log("Something hit the player");
			//		Transform otherParentTransform = other.transform.parent.transform;
			Transform otherParentTransform = other.transform;
			string otherTag = otherParentTransform.tag;
			if(player.rigidbody2D.velocity.y < 8f) {

				if(player.rigidbody2D.velocity.y < 0f) {
					if(otherTag == "Platform") {
						player.landOnPlatform(other.GetComponent<Platform>());
					} else if(otherTag == "Ground") {
						player.landOnGround();
					} else if(otherTag == "Spring") {
						player.landOnSpring(other.GetComponent<Animator>());
					} else if(otherTag == "Enemy") {
						player.landOnEnemy(other.GetComponent<Enemy>());
					} else if (otherTag == "GameOverCheck") {
						player.gameOver();
					}
					return;
				}

				if(otherTag == "Spring") {
					player.landOnSpringDelayed(other.GetComponent<Animator>());
				}
			}  

		}
	}
}