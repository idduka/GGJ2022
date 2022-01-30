using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public EnemySpawner _otherEnemySpawner;

    [SerializeField]
    [Tooltip("The number of seconds that shall be waited before the enemy respawns on the other side.")]
    private float _respawnDelay;

    [SerializeField]
    [Tooltip("The phase effect game object.")]
    private GameObject _phaseEffectPrefab;

    [SerializeField]
    [Tooltip("The game state object.")]
    private GameState _gameState;
    
    private Vector2 _topLeftCorner;
    public bool IsInEMPMode;
    public AudioClip EMPSound;
    public AudioSource PowerUPSSound;
    public List<Enemy> AliveEnemies { get; set; } = new List<Enemy>();

    private void Start()
    {
        var halfSize = _playerZone.rect.size / 2;
        _topLeftCorner = new Vector2(-halfSize.x, halfSize.y);
    }

    public void SpawnEnemy(Vector2 spawnPoint, float speedChange)
    {
        var createdEnemy = Instantiate(_enemyPrefab, _playerZone);
        createdEnemy.transform.localPosition = _topLeftCorner + spawnPoint;
        var enemyComponent = createdEnemy.GetComponent<Enemy>();
        enemyComponent.TargetPlanet = _targetPlanet;
        enemyComponent.EnemySpawner = this;
        enemyComponent._speed += speedChange;
        AliveEnemies.Add(enemyComponent);
    }

    public void SpawnEnemyAtOtherSide(Vector2 relativePosition, float speed)
    {
        StartCoroutine(_otherEnemySpawner.RespawnEnemy(relativePosition, speed));
    }
    
    public IEnumerator RespawnEnemy(Vector2 relativePosition, float speed)
    {
        if (_gameState.IsGameOver)
        {
            yield break;
        }
        
        var phaseEffect = Instantiate(_phaseEffectPrefab, _playerZone);
        phaseEffect.transform.localPosition = relativePosition;
        
        yield return new WaitForSeconds(_respawnDelay);
        
        Destroy(phaseEffect);
        
        if (_gameState.IsGameOver)
        {
            yield break;
        }
        
        var createdEnemy = Instantiate(_enemyPrefab, _playerZone);
        createdEnemy.transform.localPosition = relativePosition;
        var enemyComponent = createdEnemy.GetComponent<Enemy>();
        enemyComponent.TargetPlanet = _targetPlanet;
        enemyComponent.EnemySpawner = this;
        enemyComponent.IsRespawn = true;
        enemyComponent._speed = speed;
        enemyComponent.ChangeSprite();
        AliveEnemies.Add(enemyComponent);
    }

    public IEnumerator EnterEMPState(float seconds)
    {
        if (!IsInEMPMode)
        {
            PowerUPSSound.clip = EMPSound;
            PowerUPSSound.Play();
            IsInEMPMode = true;
            AliveEnemies = AliveEnemies.Select(x => { x.IsBeingAffectedByEMP = true; return x; }).ToList();
            yield return new WaitForSeconds(seconds);
            AliveEnemies = AliveEnemies.Select(x => { x.IsBeingAffectedByEMP = false; return x; }).ToList();
            IsInEMPMode = false;
        }        
    }
}
