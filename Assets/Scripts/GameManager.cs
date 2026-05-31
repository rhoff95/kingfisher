using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
internal struct EnemySpawn
{
    public GameObject prefab;
    public int weight;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // 1:8 small ship
    // 1:6 jet
    // 1:25 Big ship
    // 1:30 Ace
    // Else plane
    [SerializeField] private List<EnemySpawn> enemyTypes;

    [Range(0.5f, 10f)] public float spawnInterval;
    [Range(0, 100)] public int maxEnemies;

    private int _totalWeights;
    private bool _gameRunning = true;
    private List<Enemy> _enemies = new();
    private Camera _camera;

    private void Awake()
    {
        Instance = this;
        
        _camera = Camera.main;
    }

    private void Start()
    {
        _totalWeights = enemyTypes.Sum(et => et.weight);
        StartCoroutine(SpawnEnemiesRoutine());
    }


    private IEnumerator SpawnEnemiesRoutine()
    {
        while (_gameRunning)
        {
            if (_enemies.Count < maxEnemies)
            {
                var prefab = GetEnemyPrefab();

                var direction = Random.value < 0.5f ? 1f : -1f;
                var cameraSize = _camera.orthographicSize * _camera.aspect;
                var positionX = _camera.transform.position.x + (cameraSize * 1.3f + 10f) * direction;
                var positionY = Random.Range(0f, 30f);
                var position = new Vector3(positionX, positionY, 0f);

                var go = Instantiate(prefab, position, Quaternion.identity, transform);
                var enemy = go.GetComponent<Enemy>();
                _enemies.Add(enemy);
            }

            yield return new WaitForSeconds(Mathf.Max(0.5f, spawnInterval));
        }
    }

    private GameObject GetEnemyPrefab()
    {
        return enemyTypes.First().prefab;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
}