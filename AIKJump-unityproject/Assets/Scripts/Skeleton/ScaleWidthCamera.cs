using UnityEngine;

[ExecuteInEditMode]
public class ScaleWidthCamera : MonoBehaviour {

	public int targetWidth = 640;
	public float pixelsToUnits = 100;

	void Update() {

		int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);

		camera.orthographicSize = height / pixelsToUnits / 2;
	}
}
