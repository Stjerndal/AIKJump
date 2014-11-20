using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip clipJumpPlat;
	public AudioClip clipJumpEnemy;
	public AudioClip clipJumpSpring;
	public AudioClip clipThrowBomb;
	public AudioClip clipThrowBomb2;
	public AudioClip clipEnemyDie;
	public AudioClip clipCollect;
	public AudioClip clipHit;
	public AudioClip clipDie;
	public AudioClip clipGlassBreak;

	public AudioClip clipNope;
	public AudioClip clipClick;


	private AudioSource audioJumpPlat;
	private AudioSource audioJumpEnemy;
	private AudioSource audioJumpSpring;
	private AudioSource audioEnemyDie;
	private AudioSource audioThrowBomb;
	private AudioSource audioThrowBomb2;
	private AudioSource audioCollect;
	private AudioSource audioHit;
	private AudioSource audioDie;
	private AudioSource audioGlassBreak;

	private AudioSource audioNope;
	private AudioSource audioClick;

	public bool soundEnabled = true;

	private static AudioManager instance = null;
	public static AudioManager Instance {
		get { return instance; }
	}

	AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) {
		AudioSource newAudio = (AudioSource) gameObject.AddComponent("AudioSource");
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(transform.gameObject);


		//Add the necessary AudioSources
		audioJumpPlat = AddAudio(clipJumpPlat, false, false, 0.3f);
		audioJumpEnemy = AddAudio(clipJumpEnemy, false, false, 0.6f);
		audioJumpSpring = AddAudio(clipJumpSpring, false, false, 0.8f);
		audioEnemyDie = AddAudio(clipEnemyDie, false, false, 0.2f);
		audioThrowBomb = AddAudio(clipThrowBomb, false, false, 0.9f);
		audioThrowBomb2 = AddAudio(clipThrowBomb2, false, false, 0.9f);
		audioCollect = AddAudio(clipCollect, false, false, 0.4f);
		audioHit = AddAudio(clipHit, false, false, 0.9f);
		audioDie = AddAudio(clipDie, false, false, 0.3f);
		audioGlassBreak = AddAudio(clipGlassBreak, false, false, 0.45f);

		audioNope = AddAudio(clipNope, false, false, 1f);
		audioClick = AddAudio(clipClick, false, false, 0.8f);
//		audioDie.pitch = 1.23f;
	}

	public void JumpPlat() {
		if(soundEnabled) {
			audioJumpPlat.Play();
		}
	}

	public void JumpEnemy() {
		if(soundEnabled) {
			audioJumpEnemy.Play();
		}
	}

	public void JumpSpring() {
		if(soundEnabled) {
			audioJumpSpring.Play();
		}
	}

	public void EnemyDie() {
		if(soundEnabled) {
			audioEnemyDie.Play();
		}
	}

	public void ThrowBomb(){
		if(soundEnabled) {
			audioThrowBomb.Play();
		}
	}

	public void ThrowBomb2(){
		if(soundEnabled) {
			audioThrowBomb2.Play();
		}
	}

	public void GlassBreak(){
		if(soundEnabled) {
			audioGlassBreak.Play();
		}
	}

	public void Collect() {
		if(soundEnabled) {
			audioCollect.Play();
		}
	}

	public void Hit() {
		if(soundEnabled) {
			audioHit.Play();
		}
	}

	public void Die() {
		StopAll();
		if(soundEnabled && !audioDie.isPlaying) {
			audioDie.Play();
		}
	}

	public void Nope() {
		if(soundEnabled) {
			audioNope.Play();
		}
	}

	public void Click() {
		if(soundEnabled) {
			audioClick.Play();
		}
	}

	public void ClickForced() {
			audioClick.Play();
	}

	public void StopAll() {
		if(audioJumpPlat.isPlaying) audioJumpPlat.Stop();
		if(audioJumpEnemy.isPlaying) audioJumpEnemy.Stop();
		if(audioEnemyDie.isPlaying) audioEnemyDie.Stop();
		if(audioThrowBomb.isPlaying) audioThrowBomb.Stop();
		if(audioThrowBomb2.isPlaying) audioThrowBomb2.Stop();
		if(audioCollect.isPlaying) audioCollect.Stop();
		if(audioNope.isPlaying) audioNope.Stop();
		if(audioClick.isPlaying) audioClick.Stop();
		if(audioDie.isPlaying) audioDie.Stop();
	}

	public void SetSoundEnabled(bool enabled) {
		PlayerPrefs.SetInt("soundEnabled", (enabled ? 1 : 0));
		soundEnabled = enabled;
//		Debug.Log("Sound Enabled: " + soundEnabled);
		if(!soundEnabled)
			StopAll();
	}
	
	public void ToggleSoundEnabled() {
		soundEnabled = !soundEnabled;
		SetSoundEnabled(soundEnabled);
	}

}
