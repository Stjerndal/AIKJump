/*/
* Script by Devin Curry
* Attach this script to a GUITexture or GUIText to fix its scale to the aspect ratio of the current screen
* Change the values of scaleOnRatio1 (units are in % of screenspace if the screen were square, 0f = 0%, 1f = 100%) to change the desired ratio of the GUI
* The ratio is width-based so scaleOnRatio1.x will stay the same, scaleOnRatio1.y will based on the screen ratio
*
* The most common aspect ratio for COMPUTERS is 16:9 followed by 16:10
* Possible aspect ratios for MOBILE are:
* 4:3
* 3:2
* 16:10
* 5:3
* 16:9
/*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIAspectRatioScale : MonoBehaviour 
{
	public Vector2 scaleOnRatio1 = new Vector2(0.1f, 0.1f);
	private RectTransform myTrans;
	private float widthHeightRatio;
	private float heightWidthRatio;
	
	void Start () 
	{
//		myTrans = transform;
		myTrans = GetComponent<RectTransform>();
		//find the aspect ratio
		widthHeightRatio = (float)Screen.width/Screen.height;

		heightWidthRatio = (float)Screen.height/Screen.width;
		
		//Apply the scale. We only calculate y since our aspect ratio is x (width) authoritative: width/height (x/y)
//		myTrans.localScale = new Vector3 (scaleOnRatio1.x, widthHeightRatio * scaleOnRatio1.y, 1);
		myTrans.localScale = new Vector3 (scaleOnRatio1.x * heightWidthRatio, scaleOnRatio1.y, 1);
	}
	/*
	void Update() {
		widthHeightRatio = (float)Screen.width/Screen.height;
		
		//Apply the scale. We only calculate y since our aspect ratio is x (width) authoritative: width/height (x/y)
		myTrans.localScale = new Vector3 (scaleOnRatio1.x, widthHeightRatio * scaleOnRatio1.y, 1);
	}*/

}