using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public AudioManager audioManager;
	MusicAudio musicAudio;
	AdScript adScript;

	public Text curScoreField;
	public Text bombsTextField;
	public Text scoreField;
	public Text bestScoreField;

	public Animator gameOverTextAnim;
	public Animator gameOverScorePanelAnim;
//	public Animator gameOverBtnPanelAnim;
	public Animator gameOverMedalAnim;
	public Animator ldbBtnAnim;
	public Animator playBtnAnim;
	public Animator twitterBtnAnim;
	public Animator backBtnAnim;

	public GameObject bombPanel;

	public GameObject pauseBtn;
	public GameObject pausePanel;
	public PauseScript pauseScript;

	public StuffSpawner stuffSpawner;

	public static float worldHorizMax;
	public static float worldHorizMin;

	public static Vector2 hideVector;

	public static GameController current;

	public GameStages gameStages;
	public GameStage currentStage;
	public float platSpawnRate;		//how quickly platforms spawn
	public float enemySpawnRate;		//how quickly enemies spawn
	public float bombSpawnRate;		//how quickly bombs spawn
	
	public float movingChance; //Default: 25%
	public float springChance; //Default: 15%

	public int curDiff = -1;



	float lastModeChange = 0f;

	const float MAX_PLAT_RATE = 12.9f;

	const int GAME_STATE_PAUSED = -1;
	const int GAME_STATE_RUNNING = 0;
	const int GAME_STATE_OVER = 2;

	const int BOMB_POINTS = 1;
	const int TROPHY_POINTS = 50;
	const int BOMB_ENEMY_POINTS = 10;

	public Player playerScript;
	public Transform playerTransform;
	public Rigidbody2D playerBody;


	public float heightSoFar;
	public int score;
	public int numBombs;
	public int state;

	int bestScore;

	public GameObject bombThrownPrefab;

	public Transform BombAimY;

	public Animator bombPanelAnim;

	public GameObject adObject;

	float bombDelay = 0.1f;
	const float bombCoolDown = 0.5f;
	float timeSinceBomb = 3f;

	bool musicEnabled;
	bool soundEnabled;

	int lastScore = 0;
	int lastNumBombs = 0;

	void Awake () {
		//if we don't currently have a game control...
		if (current == null)
			//...set this one to be it...
			current = this;
		//...otherwise...
		else if(current != this)
			//...destroy this one because it is a duplicate
			Destroy (gameObject);

		worldHorizMax = Camera.main.ViewportToWorldPoint(new Vector3(1f,0f,0f)).x;
		worldHorizMin = Camera.main.ViewportToWorldPoint(new Vector3(0f,0f,0f)).x;
		hideVector = new Vector2(0f, -100f);
		heightSoFar = 0f;
		timeSinceBomb = bombCoolDown + 1f;
		currentStage = new GameStage();
	}

	void Start() {
		curScoreField.text = "0";
		bombsTextField.text = "0";

		lastModeChange = 0f;
		score = 0;
		numBombs = 10;

//		currentStage = new GameStage();
		currentStage = gameStages.newStage(0);

		bestScore = PlayerPrefs.GetInt("bestscore", 0);
		bestScoreField.text = "" + bestScore;

		adScript = GameObject.FindObjectOfType<AdScript>();
		audioManager = GameObject.FindObjectOfType<AudioManager>();
		musicAudio = GameObject.FindObjectOfType<MusicAudio>();
		musicAudio.SetMusicPlayingFromSound(audioManager.soundEnabled);
		audioManager.StopAll();
		adScript.myObject = adObject;
		adScript.showWhenReady = false;
		adScript.LoadAd();

		setUpGUI();
//		Application.CaptureScreenshot("Screenshot.png", 2);
	}

	public void setUpGUI() {
		adScript.HideAd();
		gameOverTextAnim.SetTrigger("Hide");
		gameOverScorePanelAnim.SetTrigger("Hide");
		ldbBtnAnim.SetTrigger("Hide");
		playBtnAnim.SetTrigger("Hide");
		twitterBtnAnim.SetTrigger("Hide");
		backBtnAnim.SetTrigger("Hide");
		pauseScript.init(audioManager.soundEnabled, musicAudio.musicEnabled);
		pausePanel.SetActive(false);
		showBombPanel(true);
		showPauseButton(true);
//		gameOverBtnPanelAnim.SetTrigger("Hide");
	}

	void FixedUpdate() {
		heightSoFar = Mathf.Max (playerTransform.position.y, heightSoFar);
	}
	

	void OnGUI() {
//		curScoreField.text = "" + ((int) heightSoFar);
		if(state != GAME_STATE_OVER) {
//			if(score > lastScore) {
				curScoreField.text = "" + (score + (int) heightSoFar/10);
	//			curScoreField.text = "" + score;
//				lastScore = score;
//			}

			if(numBombs != lastNumBombs) {
				bombsTextField.text = "" + numBombs;
				lastNumBombs = numBombs;
			}
		}
	}




	public void RestartGame() {
		audioManager.Click();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void backToMainMenu() {
		audioManager.Click();
		PlayerPrefs.SetInt("fromGame", 1);
		Application.LoadLevel(0);
	}
	
	public void showLeaderboards() {
		audioManager.Click();
//		if(!Social.Active.localUser.authenticated) {
//			Social.localUser.Authenticate((bool success) => {
//				// handle success or failure
//				if(success) {
//					((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardID);
//				}
//			});
//		} else {
//			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardID);
//		}
	}

	public void twitterButton() {
		string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
		string TWEET_LANGUAGE = "en"; 
		string twitterText = "I just scored " + score
				+ " in #GnagetJump! Can you beat me? ANDROID: "
				+ "https://play.google.com/store/apps/details?id=se.stjerndal.wcsprint" +
				" #AIK";
		
		Application.OpenURL(TWITTER_ADDRESS +
		                    "?text=" + WWW.EscapeURL(twitterText) +
		                    "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
	}

	public void PauseGame() {
		audioManager.Click();
		state = GAME_STATE_PAUSED;

//		if(Time.timeScale > 0) {
			Time.timeScale = 0;
//		} else {
//			Time.timeScale = 1;
//		}
		pausePanel.SetActive(true);
		pauseScript.pause();
		pauseBtn.SetActive(false);
		adScript.ShowAd();
	}

	public void ResumeGame() {
		audioManager.Click();
		adScript.HideAd();
		state = GAME_STATE_RUNNING;
		pauseScript.unPause();
		pausePanel.SetActive(false);
		pauseBtn.SetActive(true);
		Time.timeScale = 1;
	}

	public void toggleSoundEnabled() {
		if(!audioManager.soundEnabled)
			audioManager.ClickForced();
		audioManager.ToggleSoundEnabled();
		musicAudio.SetMusicPlayingFromSound(audioManager.soundEnabled);
		pauseScript.SetSound(audioManager.soundEnabled);
	}

	public void toggleMusicEnabled() {
		audioManager.Click();
//		musicAudio.SetMusicPlaying(audioManager.soundEnabled);
		musicAudio.ToggleMusicEnabled();
		pauseScript.SetMusic(musicAudio.musicEnabled);
	}



	public void GameOver() {
		state = GAME_STATE_OVER;
		audioManager.Die();
		stuffSpawner.StopSpawn ();
		
		Debug.Log("GameOver!!");
		
		if((score + (int) heightSoFar/10) > bestScore) {
			//TODO POPUP NEW HIGHSCORE!!!
			bestScore = (score + (int) heightSoFar);
			PlayerPrefs.SetInt("bestscore", bestScore);
			Invoke("showNewBestScore", 2f);
			//			if (Social.localUser.authenticated) {
			//				Social.ReportScore(bestScore, leaderboardID, (bool success) => {});
			//			}
		}
		
		scoreField.text = "" + (score + (int) heightSoFar/10);
		bestScoreField.text = "" + bestScore;
		
		//show the game over stuff
		showPauseButton(false);
		showBombPanel(false);
		Invoke("showGameOverScorePanel", 0.6f);
		Invoke("hideCurScoreField", 0.6f);
		Invoke("showGameOverText", 0.6f);
		showPanelButtons(1f);
		//		Invoke("showGameOverBtnPanel", 1.6f);
		//		Invoke("showNewBestScore", 2f);
		
		
		musicAudio.Pause();
		adScript.ShowAd();
	}

	#region Game Events
	void throwBomb(Vector2 targetScreenLoc) {
		if(targetScreenLoc.y/Screen.height > 0.85f) {
			return;
		}
//		Debug.Log("TouchPos: " + targetScreenLoc);
		if(numBombs <= 0) {
			audioManager.Nope();
			bombPanelAnim.SetTrigger("OutOfAmmoFlash");
			return;
			
		}
		
		if(timeSinceBomb < bombCoolDown) {
			return;
		}
		numBombs--;
		
		timeSinceBomb = 0;
		Vector3 targetLoc = Camera.main.ScreenToWorldPoint(targetScreenLoc);
		bool toTheRight = (targetLoc.x > playerTransform.position.x && playerScript.facingRight)
			|| (targetLoc.x <= playerTransform.position.x && !playerScript.facingRight);
		targetLoc.z = 0f;
		targetLoc.y = BombAimY.position.y;
		playerScript.throwBomb(toTheRight);
		StartCoroutine(throwBombDelayed(toTheRight, targetLoc));
	}
	
	IEnumerator throwBombDelayed(bool toTheRight, Vector3 targetLoc) {
		yield return new WaitForSeconds(bombDelay);
		//		Vector2 startPos = ;
		//		Rigidbody2D bombBody = (Rigidbody2D) Instantiate(bombThrownPrefab, toTheRight ? playerScript.bombStartRight.position : playerScript.bombStartLeft.position, Quaternion.identity);
		GameObject bomb = (GameObject) Instantiate(bombThrownPrefab, toTheRight ? playerScript.bombStartRight.position : playerScript.bombStartLeft.position, Quaternion.identity);
		//		Transform bombTransform = (Transform) Instantiate(bombThrownPrefab, toTheRight ? playerScript.bombStartRight.position : playerScript.bombStartLeft.position, Quaternion.identity);
		//		bombTransform.position = new Vector2 (0f, 0f);
		BombThrown bombScript = bomb.GetComponentInParent<BombThrown>();
		bombScript.Throw(targetLoc, playerBody.velocity);
		//		bombBody.velocity = (targetLoc - playerTransform.position) * 2f;
	}

	public void collectBomb() {
		audioManager.Collect();
		score += BOMB_POINTS;
		numBombs++;
	}

	public void collectTrophy() {
		score += TROPHY_POINTS;
	}

	public void bombEnemy() {
//		audioManager.EnemyDie();
		score += BOMB_ENEMY_POINTS;
	}

	public void jumpOnPlatform() {
		audioManager.JumpPlat();
	}

	public void jumpOnEnemy() {
		audioManager.JumpEnemy();
	}

	public void jumpOnSpring() {
		audioManager.JumpSpring();
	}

	public void playerHit() {
		audioManager.Hit();
	}

	public void throwBomb() {
		audioManager.ThrowBomb();
	}

	public void enemyDie() {
		audioManager.EnemyDie();
	}
	#endregion

	void Update() {
		if(state == GAME_STATE_RUNNING) {

			timeSinceBomb += Time.deltaTime;
			#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0)) throwBomb(Input.mousePosition);

			#elif UNITY_ANDROID || UNITY_IPHONE
			if(Input.touchCount > 0) {
				foreach(Touch touch in Input.touches) {
					if(touch.phase == TouchPhase.Began) {
						throwBomb(touch.position);
					}
					return;
				}
			}
			#endif

			if(heightSoFar > GameStages.DIFF4 && lastModeChange < heightSoFar - 100f) {
				currentStage = gameStages.newStage(curDiff);
				curDiff++;
				lastModeChange = heightSoFar;
			} else if (heightSoFar > GameStages.DIFF3 && curDiff < 3) {
				currentStage = gameStages.newStage(3);
				curDiff = 3;
			}  else if (heightSoFar > GameStages.DIFF2 && curDiff < 2) {
				currentStage = gameStages.newStage(2);
				curDiff = 2;
			}  else if (heightSoFar > GameStages.DIFF1 && curDiff < 1) {
				currentStage = gameStages.newStage(1);
				curDiff = 1;
			}  else if (heightSoFar > GameStages.DIFF0 && curDiff < 0) {
				currentStage = gameStages.newStage(0);
				curDiff = 0;
			}

	//		platSpawnRate = currentStage.platSpawnRate;
	//		enemySpawnRate = currentStage.enemySpawnRate;
	//		bombSpawnRate = currentStage.bombSpawnRate;
	//		movingChance = currentStage.movingChance;
	//		springChance = currentStage.springChance;

	//		if(lastModeChange < (heightSoFar - 40f)) {
	//			lastModeChange = heightSoFar;
	//			stuffSpawner.platformSpawnRate = Mathf.Clamp(stuffSpawner.platformSpawnRate * 1.01f, 10f, MAX_PLAT_RATE);
	//			stuffSpawner.enemySpawnRate = Mathf.Clamp(stuffSpawner.enemySpawnRate * 0.95f, 12, 40f);
	//			stuffSpawner.movingChance = Mathf.Clamp(stuffSpawner.movingChance * 1.1f, 0.20f, 1.0f);
	//		}
		}
	}

	#region ShowGUIstuff
	void showGameOverText() {
		gameOverTextAnim.SetTrigger("DropDown");
//		audioManager.ThrowBomb();
	}
	
	void showGameOverScorePanel() {
		gameOverScorePanelAnim.SetTrigger("RiseFromBot");
//		gameOverMedalAnim.SetInteger("MedalType", MedalType(score));
//		audioManager.ThrowBomb2();
	}
	
//	void showGameOverBtnPanel() {
//		gameOverBtnPanelAnim.SetTrigger("UnHide");
//	}
	
//	void showNewBestScore() {
//		newAnim.SetTrigger("SlideInFromRight");
//		audioManager.ThrowBomb();
//	}

	void showPanelButtons(float delay) {
		Invoke("showLdbButton", delay);
		Invoke("showPlayButton", delay+0.4f);
		Invoke("showTwitterButton", delay+0.8f);
		Invoke("showBackBtn", delay+1.5f);
	}

	void showLdbButton() {
		ldbBtnAnim.SetTrigger("SlideInFromRight");
		audioManager.ThrowBomb();
	}

	void showPlayButton() {
		playBtnAnim.SetTrigger("SlideInFromRight");
		audioManager.ThrowBomb();
	}

	void showTwitterButton() {
		twitterBtnAnim.SetTrigger("SlideInFromRight");
		audioManager.ThrowBomb();
	}

	void hideCurScoreField() {
		curScoreField.text = "";
	}

	void showPauseButton(bool show) {
		pauseBtn.SetActive(show);
	}

	void showBombPanel(bool show) {
		bombPanel.SetActive(show);
	}

	void showBackBtn() {
		backBtnAnim.SetTrigger("SlideInFromRight");
    }
	#endregion

	private int MedalType(int score) {
		if(score >= 50) {
			return 3; //GOLD
		} else if (score >= 30) {
			return 2; //SILVER
		} else if (score >= 15) {
			return 1; //BRONZE
		} else {
			return 0; //NONDE
		}
	}



}
