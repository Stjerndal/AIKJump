using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	bool musicEnabled;
	bool soundEnabled;
	AudioManager audioManager;
	MusicAudio musicAudio;
//	public Animator btnAnim;
	public Transform loadingImg;

	public AdScript adController;

	public SoundToggler soundToggler;
	public Animator settingsPanelAnim;



	void Start() {
		loadingImg.active = false;
		adController.LoadAndShow();
		audioManager = GameObject.FindObjectOfType<AudioManager>();
		musicAudio = GameObject.FindObjectOfType<MusicAudio>();
		audioManager.StopAll();
		soundToggler.init(audioManager.soundEnabled, musicAudio.musicEnabled);
		StartMusic();
		settingsPanelAnim.SetBool("Expanded", false);

	}

	public void playGame() {
//		btnAnim.SetTrigger("RiseToTop");
		audioManager.Click();
		loadingImg.active = true;
		Application.LoadLevel(1);
//		Invoke("startGame", 0.2f);

	}
//
//	void startGame() {
//		Application.LoadLevel(1);
//	}

	public void testScene() {
//		Application.LoadLevel(2);
	}

	public void SettingsClicked() {
		audioManager.Click();
		bool expanded = !settingsPanelAnim.GetBool("Expanded");
		settingsPanelAnim.SetBool("Expanded", expanded);
		soundToggler.SetBtnsInteractable(expanded);
		if(expanded)
			soundToggler.init(audioManager.soundEnabled, musicAudio.musicEnabled);
	}

	public void toggleSoundEnabled() {
		if(!audioManager.soundEnabled)
			audioManager.ClickForced();
		audioManager.ToggleSoundEnabled();
		musicAudio.SetMusicPlayingFromSound(audioManager.soundEnabled);
		soundToggler.SetSound(audioManager.soundEnabled);
	}
	
	public void toggleMusicEnabled() {
		audioManager.Click();
		//		musicAudio.SetMusicPlaying(audioManager.soundEnabled);
		musicAudio.ToggleMusicEnabled();
		soundToggler.SetMusic(musicAudio.musicEnabled);
	}


	public void StartMusic() {
//		musicEnabled = PlayerPrefs.GetInt("musicEnabled", 1) == 1;
//		musicAudio.SetMusicPlaying(musicEnabled);
		musicAudio.SetMusicPlayingFromSound(audioManager.soundEnabled);
	}
}
