using UnityEngine;

[ExecuteInEditMode]
public class PixelDensityCamera : MonoBehaviour {

	public float pixelsToUnits = 100;

	void Update () {

		camera.orthographicSize = Screen.height / pixelsToUnits / 2;
	}
}
