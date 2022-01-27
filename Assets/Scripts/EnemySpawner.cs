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

    [SerializeField]
    [Tooltip("The enemy spawner of the other player area.")]
    private EnemySpawner _otherEnemySpawner;

    [SerializeField]
    [Tooltip("The number of seconds that shall be waited before the enemy respawns on the other side.")]
    private float _respawnDelay;
    
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
        enemyComponent.EnemySpawner = this;
        AliveEnemies.Add(enemyComponent);
    }

    public void SpawnEnemyAtOtherSide(Vector2 relativePosition)
    {
        StartCoroutine(_otherEnemySpawner.RespawnEnemy(relativePosition));
    }
    
    public IEnumerator RespawnEnemy(Vector2 relativePosition)
    {
        yield return new WaitForSeconds(_respawnDelay);
        
        var createdEnemy = Instantiate(_enemyPrefab, _playerZone);
        createdEnemy.transform.localPosition = relativePosition;
        var enemyComponent = createdEnemy.GetComponent<Enemy>();
        enemyComponent.TargetPlanet = _targetPlanet;
        enemyComponent.EnemySpawner = this;
        enemyComponent.IsRespawn = true;
        enemyComponent.ChangeSprite();
        AliveEnemies.Add(enemyComponent);
    }

    private Vector2 CalculateActualSpawnPosition(Vector2 baseSpawnPosition)
    {
        var halfSize = _playerZone.rect.size / 2;
        var topLeftCorner = new Vector2(-halfSize.x, halfSize.y);
        
        return topLeftCorner + baseSpawnPosition;
    }
}
