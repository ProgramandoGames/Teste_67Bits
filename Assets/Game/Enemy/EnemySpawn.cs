using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public Enemy enemyPrefab;

    public List<Vector3> spawnPosition;
    public float delay = 5f;
    public int   maxEnemies = 8;

    private List<Enemy> _enemies = new List<Enemy>();
    private float _spawnTime = 0;

    private void Awake() {
        
    }

    void Update() {
        
        if(Time.time > _spawnTime && _enemies.Count < maxEnemies) {
            SpawnEnemy();
            _spawnTime = Time.time + delay;
        }

    }

    private void SpawnEnemy() {

        Enemy enemy = Instantiate(enemyPrefab,
                                  spawnPosition[Random.Range(0, spawnPosition.Count)],
                                  Quaternion.identity);
        enemy.Destroyed += OnEnemyDestroyed;
        _enemies.Add(enemy);

    }

    // This method is called when an enemy is destroyed 
    // inside the EnemyStacking script
    private void OnEnemyDestroyed(Enemy enemy) {
        _enemies.Remove(enemy);
    }

}
