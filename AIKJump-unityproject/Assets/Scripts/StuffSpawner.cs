using UnityEngine;
using System.Collections;

public class StuffSpawner : MonoBehaviour {

	public GameObject platformPrefab;		//the platform game object
	public GameObject platformMovingPrefab;		//the platformMoving game object
	public GameObject springPrefab;		//the spring game object
	public GameObject enemyPrefab;		//the enemy game object
	public GameObject bombPrefab;		//the enemy game object

	int platformPoolSize = 20;		//how many platforms to keep on standby
	int springPoolSize = 10;		//how many platforms to keep on standby
	int enemyPoolSize = 6;		//how many platforms to keep on standby
	int bombPoolSize = 6;		//how many platforms to keep on standby

//	public float platformSpawnRate = 11f;		//how quickly platforms spawn
//	public float enemySpawnRate = 40f;		//how quickly platforms spawn
//	public float bombSpawnRate = 52f;		//how quickly platforms spawn
	float platformMin; //minimum x value of the platform position
	float platformMax; //maximum x value of the platform position
	public float startPosY = 0f;

//	public float movingChance = 0.25f; //Default: 1 in 4
//	public float springChance = 0.15f; //Default: 1 in 4
//
//	float springHeight = .8f;

	private float lastSpawnPlatform = 0f;
	private float lastSpawnBomb = -7f;
	private float lastSpawnEnemy = 0f;
	private float sinceSpawn = 0f;

	const float springEnemyDistance = 15f;

	GameObject[] platforms;					//collection of pooled platforms
	Platform myPlat;						//to get myMaxHori etc
	GameObject[] platformsMoving;			//collection of pooled moving platforms
	GameObject[] springs;					//collection of pooled springs
	GameObject[] enemies;					//collection of pooled enemies
	GameObject[] bombs;						//collection of pooled bombs
	int currentplatform = 0;				//index of the current platform in the collection
	int currentplatformMoving = 0;			//index of the current moving platform in the collection
	int currentSpring = 0;					//index of the current spring in the collection
	int currentEnemy = 0;					//index of the current enemy in the collection
	int currentBomb = 0;					//index of the current bomb in the collection


	float nextPlatHeight;

	bool spawning = true;



	
	
	void Start()
	{
		//initialize the platforms collection
		platforms = new GameObject[platformPoolSize];
		platformsMoving = new GameObject[platformPoolSize];
		springs = new GameObject[springPoolSize];
		enemies = new GameObject[enemyPoolSize];
		bombs = new GameObject[bombPoolSize];
		//loop through the collection and create the individual platforms
		for(int i = 0; i < platformPoolSize; i++)
		{
			platforms[i] = (GameObject)Instantiate(platformPrefab, new Vector3(0f, -20, 0f), Quaternion.identity);
			platformsMoving[i] = (GameObject)Instantiate(platformMovingPrefab, new Vector3(0f, -20 - 10f * i, 0f), Quaternion.identity);
		}

		for(int i = 0; i < enemyPoolSize; i++) {
			enemies[i] = (GameObject)Instantiate(enemyPrefab, new Vector3(0f, -25 - 10f * i, 0f), Quaternion.identity);
		} for(int i = 0; i < springPoolSize; i++) {
			springs[i] = (GameObject)Instantiate(springPrefab, new Vector3(0f, -20, 0f), Quaternion.identity);
		} for(int i = 0; i < bombPoolSize; i++) {
			bombs[i] = (GameObject)Instantiate(bombPrefab, new Vector3(0f, -20, 0f), Quaternion.identity);
		}
//		lastSpawnPlatform = startPosY;
//		lastSpawn = startPosY;
//		lastSpawn = startPosY;
		myPlat = platforms[0].GetComponent<Platform>();
		lastSpawnPlatform = transform.position.y-10f;
		nextPlatHeight = GameController.current.currentStage.getNextHeight(lastSpawnPlatform);
	}

	void Update() {
		if(spawning){
			if(GameController.current.currentStage.platSpawnRate <= (transform.position.y - lastSpawnPlatform)) {
//			if(nextPlatHeight <= transform.position.y) {
//				Debug.Log("Should spawn platform");
				SpawnPlatform();
			} else if(GameController.current.currentStage.enemySpawnRate <= (transform.position.y - lastSpawnEnemy)) {
				SpawnEnemy();
			} else if(GameController.current.currentStage.bombSpawnRate <= (transform.position.y - lastSpawnBomb)) {
				SpawnBomb();
			}

		}
	}

	void SpawnPlatform() {

		//To spawn a platform, get the current spawner position...
		Vector3 pos = transform.position;
		platformMin = myPlat.myMinHori;
		platformMax = myPlat.myMaxHori;

		if((transform.position.y - lastSpawnPlatform) > GameController.current.currentStage.MAX_PLAT_SPAWN_RATE) {
			pos.y = lastSpawnPlatform + GameController.current.currentStage.MAX_PLAT_SPAWN_RATE;

			Debug.Log("To large Diff: " + (transform.position.y - lastSpawnPlatform) + ", New Diff: " + (pos.y - lastSpawnPlatform));
		}

		lastSpawnPlatform = pos.y;
		//...set a random x position...
		pos.x = Random.Range(platformMin, platformMax);
		pos.z = 0;
		Transform nextPlatTransform;


//		Debug.Log("Random 1: " + Random.value);

		if(Random.value > GameController.current.currentStage.movingChance) { //Static platform
			//...then set the current platform to that position.
			nextPlatTransform = platforms[currentplatform].transform;
			myPlat = nextPlatTransform.GetComponent<Platform>();
//			nextPlatTransform.GetComponent<Platform>().mySpring = null;
//			nextPlatTransform.position = pos;
			//increase the value of currentplatform. If the new size is too big, set it back to zero
			if(++currentplatform >= platformPoolSize)
				currentplatform = 0;
		} else { //Moving platform
			nextPlatTransform = platformsMoving[currentplatformMoving].transform;
//			nextPlatTransform.GetComponent<Platform>().mySpring = null;
			myPlat = nextPlatTransform.GetComponent<Platform>();
			nextPlatTransform.GetComponent<PlatformMoving>().SetSpeed(GameController.current.currentStage.platSpeed);
//			nextPlatTransform.position = pos;
			if(++currentplatformMoving >= platformPoolSize)
				currentplatformMoving = 0;
		}

		myPlat.mySpring = null;
		nextPlatTransform.position = pos;

		if(Random.value < GameController.current.currentStage.glassChance) {
			myPlat.Restart(Platform.PLATFORM_MATERIAL_GLASS);
		} else {
			myPlat.Restart(Platform.PLATFORM_MATERIAL_WOOD);
		}


//		Debug.Log("Random 2: " + Random.value);
		if(Random.value < GameController.current.currentStage.springChance) { //Static platform
			//...then set the current platform to that position.
//			pos.y += springHeight;
//			springs[currentplatform].transform.position = pos;springs[currentplatform].transform.position = pos;
			myPlat.mySpring = springs[currentSpring].transform;
			myPlat.mySpring.GetComponent<Spring>().Restart();
			lastSpawnEnemy = transform.position.y + springEnemyDistance;
			//increase the value of currentplatform. If the new size is too big, set it back to zero
			if(++currentSpring >= springPoolSize) {
				currentSpring = 0;
			}
		}

//		nextPlatHeight = GameController.current.currentStage.getNextHeight(transform.position.y);

	}

	void SpawnEnemy() {
//		Debug.Log("Spawn enemy");
		//To spawn an enemy, get the current spawner position...
		Vector3 pos = transform.position;
		lastSpawnEnemy = pos.y;
		//...set a random x position...
		pos.x = Random.Range(platformMin, platformMax);
		pos.z = 0;

		//...then set the current enemy to that position.
		enemies[currentEnemy].transform.position = pos;
		enemies[currentEnemy].GetComponent<Enemy>().Reborn();
		
		//increase the value of currentenemy. If the new size is too big, set it back to zero
		if(++currentEnemy >= enemyPoolSize)
			currentEnemy = 0;
	}

	void SpawnBomb() {
		
		//To spawn a bomb, get the current spawner position...
		Vector3 pos = transform.position;
		lastSpawnBomb = pos.y;
		//...set a random x position...
		pos.x = Random.Range(platformMin, platformMax);
		pos.z = 0;
		
		//...then set the current bomb to that position.
		bombs[currentBomb].transform.position = pos;
		//increase the value of currentbomb. If the new size is too big, set it back to zero
		if(++currentBomb >=bombPoolSize)
			currentBomb = 0;
	}

	public void startSpawning() {
		spawning = true;
	}
	
	public void StopSpawn() {
		spawning = false;
	}

}
