using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How many enemies shall spawn in the initial wave?")]
    private int _startingWaveSize;

    [SerializeField]
    [Tooltip("By much how shall the wave size increase each time (after the first wave)?")]
    private int _waveSizeIncrease;

    [SerializeField]
    [Tooltip("After how many seconds shall the next wave spawn be triggered?")]
    private int _waveSpawnDelay;

    [SerializeField]
    [Tooltip("A list of all enemy spawners in the game.")]
    private EnemySpawner[] _enemySpawners;

    [SerializeField]
    [Tooltip("How many spawn points should be selected initially (for the first wave)?")]
    private int _startingNumberOfSpawnPoints;

    [SerializeField]
    [Tooltip("How many spawn points should be added each wave (after the first wave)?")]
    private int _numberOfSpawnPointsIncrease;

    [SerializeField]
    [Tooltip("")]
    private RectTransform _playerAreaTransform;
    
    private int _waveCount = 1;
    
    private Coroutine _coroutine;
    
    private void Start()
    {
        _coroutine = StartCoroutine(SpawnWaves());
    }

    private void OnDestroy()
    {
        StopCoroutine(_coroutine);
    }

    private IEnumerator SpawnWaves()
    {
        var totalSpawnCount = _startingNumberOfSpawnPoints;
        var totalEnemyCount = _startingWaveSize;

        var maxRandomX = _playerAreaTransform.rect.width;
        var maxRandomY = _playerAreaTransform.rect.height;
        
        while (true)
        {
            Debug.Log("Waiting for next wave spawn.");
            
            yield return new WaitForSeconds(_waveSpawnDelay);
            
            if (_waveCount > 1)
            {
                totalSpawnCount += _numberOfSpawnPointsIncrease * _waveCount;
                totalEnemyCount += _waveSizeIncrease * _waveCount;
            }
            
            Debug.Log($"Wave spawn count: {totalSpawnCount}, wave enemy count: {totalEnemyCount}");

            var spawnPoints = new List<Vector2>();
            
            for (int counter = 0; counter < totalSpawnCount; counter++)
            {
                var spawnPoint =
                    GetSpawningEdgePosition(Random.Range(0, (int) maxRandomX), Random.Range(0, (int) maxRandomY), _playerAreaTransform);
                
                Debug.Log($"Spawn position {counter}: {spawnPoint.x}, {spawnPoint.y}");
                
                spawnPoints.Add(spawnPoint);
            }

            var randomSpawnList = GetSpawnOrder(spawnPoints, totalEnemyCount);

            foreach (var spawnPoint in randomSpawnList)
            {
                foreach (var enemySpawner in _enemySpawners)
                {
                    enemySpawner.SpawnEnemy(spawnPoint);
                }
                
                yield return new WaitForSeconds(0.3f);
            }

            _waveCount++;
        }
    }

    private Vector2 GetSpawningEdgePosition(int randomX, int randomY, RectTransform transform)
    {
        var maxX = _playerAreaTransform.rect.width;
        var maxY = _playerAreaTransform.rect.height;
        
        bool isXLeft = (maxX - randomX) > (0 + randomX);

        bool isYUp = (maxY - randomY) > (0 + randomY);

        var xDistance = isXLeft ? 0 + randomX : maxX - randomX;

        var yDistance = isYUp ? 0 + randomY : maxY - randomY;
        
        // if the random position is closer to X than to Y, align it to the X axis
        // otherwise align the spawn point to the Y axis
        if (xDistance < yDistance)
        {
            var xPosition = isXLeft ? 0 : transform.rect.width;
            return new Vector2(xPosition, -randomY);
        }
        else
        {
            var yPosition = isYUp ? 0 : transform.rect.height;
            return new Vector2(randomX, -yPosition);
        }
    }

    private List<Vector2> GetSpawnOrder(List<Vector2> spawnPoints, int enemyNumber)
    {
        var spawnPointOrderList = new List<Vector2>();
        
        for (int index = 0; index < enemyNumber; index++)
        {
            var randomSpawnPointIndex = Random.Range(0, spawnPoints.Count - 1);
            
            var spawnPoint = spawnPoints[randomSpawnPointIndex];
            spawnPointOrderList.Add(spawnPoint);
        }

        return spawnPointOrderList;
    }
}
