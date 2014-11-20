using UnityEngine;
using System.Collections;

public class GUITextScaler : MonoBehaviour {

	public int fontSizeOn800w = 32;
	private float sizeRatio;
	
	void Start () 
	{
		SetScale();
	}
	/*
	void Update() {
		SetScale ();
	}
	*/
	//call on an event that tells if the aspect ratio changed
	void SetScale()
	{
		//find the size ratio
		sizeRatio = (float)Screen.width/800;
		
		guiText.fontSize = (int) (fontSizeOn800w*sizeRatio);
	}
}
