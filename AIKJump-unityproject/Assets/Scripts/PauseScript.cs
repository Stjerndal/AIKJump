using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {
	public Button musicButton;
	public Image soundImg;
	public Image musicImg;

	public Sprite soundEnabled;
	public Sprite soundDisabled;
	public Sprite musicEnabled;
	public Sprite musicDisabled;

	Enemy[] enemies;

	public void init(bool sound, bool music) {
		SetSound(sound);
		SetMusic(music);
	}

	public void pause() {
		if(enemies == null)
			enemies = FindObjectsOfType<Enemy>();
//		Debug.Log("Enemies Length: " + enemies.Length);
		if(GameController.current.audioManager.soundEnabled) {
			for(int i = 0; i < enemies.Length; i++) {
				enemies[i].audioWings.Stop();
			}
		}
	}

	public void unPause() {
		if(GameController.current.audioManager.soundEnabled) {
			for(int i = 0; i < enemies.Length; i++) {
				enemies[i].audioWings.Play();
			}
		}
	}

	public void SetSound(bool enabled) {
//		anim.SetBool("SoundOn", enabled);
		soundImg.sprite = enabled ? soundEnabled : soundDisabled;
		musicButton.interactable = enabled;

//		if(enemies != null) {
//			for(int i = 0; i < enemies.Length; i++) {
//				if(!enabled) {
//					enemies[i].audioWings.Stop();
//				} else {
//					enemies[i].audioWings.Play();
//				}
//			}
//		}
	}

	public void SetMusic(bool enabled) {
		musicImg.sprite = enabled ? musicEnabled : musicDisabled;
//		anim.SetBool("MusicOn", enabled);
	} 


}
