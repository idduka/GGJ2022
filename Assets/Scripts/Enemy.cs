using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed at which the enemy flies towards the planet")]
    private float _speed;
    
    public Planet TargetPlanet { get; set; }
    
    public int HitPoints { get; set; }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPlanet.transform.position, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var planet = other.GetComponent<Planet>();

        if (planet != null)
        {
            Destroy(this);
        }
    }
}