using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebGetter : MonoBehaviour {

	public string url = "http://textuploader.com/odoh/raw";
	public Text textField;

	IEnumerator Start() {
		WWW www = new WWW(url);
		yield return www;
		textField.text = www.text;
	}
}
