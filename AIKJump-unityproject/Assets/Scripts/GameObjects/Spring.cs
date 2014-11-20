using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {

	public GameObject myChild;

	public void Restart() {
		myChild.SetActive(true);
	}

	public void Hide() {
		myChild.SetActive(false);
	}

}
