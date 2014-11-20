using UnityEngine;

[ExecuteInEditMode]
public class IsometricSpriteRenderer : MonoBehaviour 
{
	void Update ()
	{
		renderer.sortingOrder = (int)(transform.position.y * -10);
	}
}
