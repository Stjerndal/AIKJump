using UnityEngine;
using System.Collections;

public class GameStage {

	public float platSpawnRate;		//how quickly platforms spawn
	public float enemySpawnRate;		//how quickly enemies spawn
	public float bombSpawnRate;		//how quickly bombs spawn
	
	public float movingChance; //Default: 25%
	public float springChance; //Default: 15%

	public float platSpeed;

	public float glassChance;

	public float MAX_PLAT_SPAWN_RATE;
	
//	public float platformMin; //minimum x value of the platform position
//	public float platformMax; //maximum x value of the platform position

	public GameStage(float platformSpawnRate, float enemySpawnRate, float bombSpawnRate,
	                 float movingChance, float springChance, float platSpeed, float glassChance, float maxPlatSpawnRate) {
		this.MAX_PLAT_SPAWN_RATE = maxPlatSpawnRate;
		this.platSpawnRate = platformSpawnRate;
		this.enemySpawnRate = enemySpawnRate;
		this.bombSpawnRate = bombSpawnRate;
		this.movingChance = movingChance;
		this.springChance = springChance;
		this.platSpeed = platSpeed;
		this.glassChance = glassChance;
	}

	public GameStage() {
//		this.platSpawnRate = GameStages.DEFAULT_PLAT_SPAWN_RATE;
		this.enemySpawnRate = GameStages.DEFAULT_ENEMY_SPAWN_RATE;
		this.bombSpawnRate = GameStages.DEFAULT_BOMB_SPAWN_RATE;
		this.movingChance = GameStages.DEFAULT_MOVING_CHANCE;
		this.springChance = GameStages.DEFAULT_SPRING_CHANCE;
		this.MAX_PLAT_SPAWN_RATE = GameStages.MAX_PLAT_SPAWN_RATE;
		this.platSpeed = GameStages.DEFAULT_PLAT_SPEED;
		this.glassChance = GameStages.DEFAULT_GLASS_CHANCE;
	}

	public float getNextHeight(float curHeight) {
		float rand = Random.Range(platSpawnRate, MAX_PLAT_SPAWN_RATE);
		Debug.Log("Min: " + platSpawnRate + ", Max: " + MAX_PLAT_SPAWN_RATE + "Rand: " + rand);
		return curHeight + rand;
	}

}
