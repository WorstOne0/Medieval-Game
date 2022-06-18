using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public Wave[] waves;
    public Melee enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemaingToSpawn, enemiesRemaingAlive;
    float nextSpawnTime;

    void Start() {
        NextWave();
    }

    void Update() {
        if (enemiesRemaingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemaingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Melee spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath() {
        enemiesRemaingAlive--;

        if (enemiesRemaingAlive == 0) {
            NextWave();
        }
    }

    void NextWave() {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length) {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemaingToSpawn = currentWave.enemyCount;
            enemiesRemaingAlive = enemiesRemaingToSpawn;
        }
    }

    [System.Serializable]
    public class Wave {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
