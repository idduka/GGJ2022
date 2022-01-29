using System.Collections;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The game state object.")]
    private GameState _gameState;

    [SerializeField]
    [Tooltip("The fire rate of the turret in seconds.")]
    private int _fireRate;

    [SerializeField]
    [Tooltip("The projectile prefab.")]
    private Projectile _projectilePrefab;

    public EnemySpawner EnemySpawner { private get; set; }

    private int _turretAmmo = 30;

    private void Start()
    {
        StartCoroutine(StartFiring());
    }

    private IEnumerator StartFiring()
    {
        while (!_gameState.IsGameOver)
        {
            if (!EnemySpawner.AliveEnemies.Any())
            {
                yield return null;
                continue;
            }

            Debug.Log("Enemy detected");

            var enemies = EnemySpawner.AliveEnemies;

            var closestEnemy =
                enemies.ToDictionary(k => k, v => Vector2.Distance(transform.position, v.transform.position)).OrderBy(d => d.Value).FirstOrDefault().Key;

            var targetPoint = closestEnemy.transform.position - transform.position;

            var rotZ = Mathf.Atan2(targetPoint.y, targetPoint.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

            Fire();
            _turretAmmo -= 1;
            
            if (_turretAmmo == 0)
            {
                DestroyTurret();
            }

            yield return new WaitForSeconds(_fireRate);
        }
    }

    void DestroyTurret()
    {
        Destroy(gameObject);
    }

    private void Fire()
    {
        AudioSource firesound = GetComponent<AudioSource>();
        firesound.Play(0);
        // start position for projectile is:
        // defender position + vector towards defender sprite center.
        Instantiate(_projectilePrefab,
            transform.position + (transform.rotation * Vector3.up * GetComponent<SpriteRenderer>().bounds.size.y / 2),
            transform.rotation);
    }
}