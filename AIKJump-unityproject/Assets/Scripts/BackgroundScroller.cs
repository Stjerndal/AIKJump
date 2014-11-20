using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {
	float xSpeed = -0.01f;			//Speed of the scrolling
	
	void Update ()
	{
		//Keep looping between 0 and 1
//		float x = Mathf.Repeat (Time.time * xSpeed, 1);
		float y = Mathf.Repeat (Camera.main.transform.position.y/100f, 1); //START AT 0.43
		//Create the offset
//		Vector2 offset = new Vector2 (0, Camera.main.transform.position.y/100f);
		Vector2 offset = new Vector2 (0, y);
		//Apply the offset to the material
		renderer.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}
}