using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundToggler : MonoBehaviour {

	public Button soundButton;
	public Button musicButton;
	public Image soundImg;
	public Image musicImg;
	
	public Sprite soundEnabled;
	public Sprite soundDisabled;
	public Sprite musicEnabled;
	public Sprite musicDisabled;

	
	public void init(bool sound, bool music) {
		SetSound(sound);
		SetMusic(music);
	}

	public void SetSound(bool enabled) {
		//		anim.SetBool("SoundOn", enabled);
		soundImg.sprite = enabled ? soundEnabled : soundDisabled;
		musicButton.interactable = enabled;
	}
	
	public void SetMusic(bool enabled) {
		musicImg.sprite = enabled ? musicEnabled : musicDisabled;
		//		anim.SetBool("MusicOn", enabled);
	}

	public void SetBtnsInteractable(bool interactable) {
		soundButton.interactable = interactable;
		musicButton.interactable = interactable;
	}

}
