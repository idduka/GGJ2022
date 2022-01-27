using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The zone in which the enemies shall be spawned.")]
    private RectTransform _playerZone;

    [SerializeField]
    [Tooltip("The prefab that shall be used to spawn the default enemy.")]
    private GameObject _enemyPrefab;

    [SerializeField]
    [Tooltip("The planet this enemy wants to attack.")]
    private Planet _targetPlanet;
    
    private Vector2 _topLeftCorner;

    public List<Enemy> AliveEnemies { get; set; } = new List<Enemy>();

    private void Start()
    {
        var halfSize = _playerZone.rect.size / 2;
        _topLeftCorner = new Vector2(-halfSize.x, halfSize.y);
    }

    public void SpawnEnemy(Vector2 spawnPoint)
    {
        var createdEnemy = Instantiate(_enemyPrefab, _playerZone);
        createdEnemy.transform.localPosition = _topLeftCorner + spawnPoint;
        var enemyComponent = createdEnemy.GetComponent<Enemy>();
        enemyComponent.TargetPlanet = _targetPlanet;
        AliveEnemies.Add(enemyComponent);
    }
    
    public void StartSpawn(List<Vector2> spawnPoints)
    {
        StartCoroutine(StartSpawnInternal(spawnPoints));
    }

    private IEnumerator StartSpawnInternal(List<Vector2> spawnPoints)
    {
        yield return null;
        
        foreach (var spawnPoint in spawnPoints)
        {
            /*var actualSpawnPoint =
                new Vector2(_playerZone.position.x + spawnPoint.x, _playerZone.position.y - spawnPoint.y);*/

            var position = CalculateActualSpawnPosition(spawnPoint);
            
            Debug.Log($"ACTUAL point: {spawnPoint.x}, {spawnPoint.y}");
            
            Debug.Log($"SPAWN TIME: {DateTime.Now:hh:mm:ss}");
            var createdEnemy = Instantiate(_enemyPrefab, _playerZone);
            createdEnemy.transform.localPosition = position;
            
            yield return new WaitForSeconds(0.3f);
        }
    }

    private Vector2 CalculateActualSpawnPosition(Vector2 baseSpawnPosition)
    {
        var halfSize = _playerZone.rect.size / 2;
        var topLeftCorner = new Vector2(-halfSize.x, halfSize.y);
        
        return topLeftCorner + baseSpawnPosition;
    }
}
