using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed at which the enemy flies towards the planet")]
    private float _speed;
    
    public Planet TargetPlanet { get; set; }
    
    public int HitPoints { get; set; }

    public EnemySpawner EnemySpawner { private get; set; }
    
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
        }
    }
}