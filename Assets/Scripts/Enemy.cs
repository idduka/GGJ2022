using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed at which the enemy flies towards the planet")]
    private float _speed;

    [SerializeField]
    [Tooltip("The sprite that is applied when the enemy respawns on the other player side.")]
    private Sprite _respawnSprite;
    
    private SpriteRenderer _spriteRenderer;
    
    public Planet TargetPlanet { get; set; }
    
    public int HitPoints { get; set; }

    public bool IsRespawn { get; set; }
    
    public EnemySpawner EnemySpawner { private get; set; }

    public void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = _respawnSprite;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPlanet.transform.position, _speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var planet = other.GetComponent<Planet>();

        Debug.Log($"HIT PLANET: {planet != null}");
        
        if (planet != null)
        {
            EnemySpawner.AliveEnemies.Remove(this);
            Destroy(gameObject);
            return;
        }

        var projectile = other.GetComponent<Projectile>();

        if (projectile != null)
        {
            if (!IsRespawn)
            {
                EnemySpawner.SpawnEnemyAtOtherSide(transform.localPosition);
            }
            
            Debug.Log($"Killed enemy position: {transform.localPosition.x}, {transform.localPosition.y}");
            EnemySpawner.AliveEnemies.Remove(this);
            Destroy(projectile.gameObject);
            Destroy(gameObject);
        }
    }
}