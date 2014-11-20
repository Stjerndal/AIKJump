using UnityEngine;
using System.Collections;

public class CameraFollowY : MonoBehaviour {

//	public Transform target;		//target for the camera to follow
	float yOffset = 0f;			//how much y-axis space should be between the camera and target
	public bool isAlive = true;
//	public Transform Player;
	public float startHeight;
	float tmpHeight;

	void Awake() {
		startHeight = transform.position.y;
	}

	void Update() {
		tmpHeight = GameController.current.heightSoFar+yOffset;
//		if(startHeight < tmpHeight) {
		if(transform.position.y < tmpHeight) {
			transform.position = new Vector3 (transform.position.x, tmpHeight, transform.position.z);
		}
	}
}
