using UnityEngine;
using System.Collections;

public class ExplosionSound : MonoBehaviour {

	public AudioSource explosionSound;

	void Awake() {
		if(GameController.current.audioManager.soundEnabled)
			explosionSound.Play();
	}
}
