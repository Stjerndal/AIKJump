using UnityEngine;
using System.Collections;

public class GameStages : MonoBehaviour {

	public const float DEFAULT_PLAT_SPAWN_RATE = 10f;
	public const float DEFAULT_ENEMY_SPAWN_RATE = 50f;
	public const float DEFAULT_BOMB_SPAWN_RATE = 52f;
	public const float DEFAULT_MOVING_CHANCE = 0.2f;
	public const float DEFAULT_SPRING_CHANCE = 0.15f;
	public const float DEFAULT_PLAT_SPEED = 2f;
	public const float DEFAULT_GLASS_CHANCE = 0.2f;

	public const float MAX_PLAT_SPAWN_RATE = 12.75f;
//	public const float MIN_ENEMY_SPAWN_RATE = 7f;

	public const int NUM_DIFFICULTIES = 5;

	public static float[] PLATFORMS = {5f, 6f, 7f, 10f, MAX_PLAT_SPAWN_RATE};
//	public static float[] PLATFORMS = {10f, 10.8f, 11.5f, 12.2f, 12.75f};
//	public static float[] PLATFORMS = {13.1f, 13.1f, 13.1f, 13.1f, 13.1f};
	public static float[] MOVING = {0.1f, 0.2f, 0.25f, 0.45f, 0.6f};
	public static float[] ENEMIES = {60f, 55f, 50f, 40f, 30f};
	public static float[] BOMBS = {35f, 40f, 47f, 55f, 60f};
	public static float[] SPRINGS = {0.10f, 0.10f, 0.15f, 0.1f, 0.05f};
	public static float[] PLAT_SPEEDS = {3f, 5f, 7f, 9f, 12f};
	public static float[] GLASS = {0.17f, 0.2f, 0.25f, 0.33f, 0.7f};

	public const float DIFF0 = 100f;
	public const float DIFF1 = 200f;
	public const float DIFF2 = 300f;
	public const float DIFF3 = 400f;
	public const float DIFF4 = 500f;

	public float specialChance = 0.3f;

	public GameStage newStage(int difficulty) {
		GameStage gameStage = new GameStage();

		if(difficulty > 5 && Random.value < 0.3f) {
			if(Random.value < specialChance) {
				Debug.Log("All moving");
				gameStage.platSpawnRate = MAX_PLAT_SPAWN_RATE;
				gameStage.movingChance = 1.0f;
				gameStage.enemySpawnRate = ENEMIES[0];
				gameStage.bombSpawnRate = BOMBS[4];
				gameStage.springChance = 0f;
				gameStage.platSpeed = PLAT_SPEEDS[2];
				gameStage.glassChance = GLASS[3];
			}
			return gameStage;
		}


		int max = (difficulty + 2) < NUM_DIFFICULTIES ? difficulty+2 : NUM_DIFFICULTIES; //+2 for Max exclusive in randomint
		int min = (difficulty - 2) > 0 ? difficulty-2 : 0;

		gameStage.platSpawnRate = PLATFORMS[Random.Range(min, max)];
		gameStage.movingChance = MOVING[Random.Range(min, max)];
		gameStage.enemySpawnRate = ENEMIES[Random.Range(min, max)];
		gameStage.bombSpawnRate = BOMBS[Random.Range(min, max)];
		gameStage.springChance = SPRINGS[Random.Range(min, max)];
		gameStage.platSpeed = PLAT_SPEEDS[Random.Range(min, max)];
		gameStage.glassChance = GLASS[Random.Range(min, max)];

		Debug.Log("P: " + gameStage.platSpawnRate + ", M: " + gameStage.movingChance + ", E: " + gameStage.enemySpawnRate + 
		          ", B: " + gameStage.bombSpawnRate + ", S: " + gameStage.springChance + "\nMin: " + min + ", Max: " + max + 
		          ", Speed: " + gameStage.platSpeed + ", Glass: " + gameStage.glassChance);
		return gameStage;
	}


}
