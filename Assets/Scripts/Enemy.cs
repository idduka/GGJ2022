using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed at which the enemy flies towards the planet")]
    private float _speed;

    [SerializeField]
    [Tooltip("The sprite that is applied when the enemy respawns on the other player side.")]
    private Sprite _respawnSprite;

    [SerializeField]
    [Tooltip("The total time it would take to phase out or phase in.")]
    private float _totalPhaseDuration;

    [SerializeField]
    [Tooltip("The transition curve for phasing out and in.")]
    private AnimationCurve _phaseAnimation;

    private bool _isPhasing = false;

    private SpriteRenderer _spriteRenderer;
    
    public Planet TargetPlanet { get; set; }
    
    public int HitPoints { get; set; }

    public bool IsRespawn { get; set; }
    
    public EnemySpawner EnemySpawner { private get; set; }

    private void Start()
    {
        if(IsRespawn)
        {
            StartCoroutine(PhaseIn());
        }
    }

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
        if(_isPhasing)
        {
            return;
        }

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
            Destroy(projectile.gameObject);
            if (!IsRespawn)
            {
                StartCoroutine(PhaseOut());
            }
            else
            {
                Die();
            }
        }
    }

    public IEnumerator PhaseIn()
    {
        _isPhasing = true;
        _speed = 0.0f;
        var curPhaseDuration = 0.0f;
        while (curPhaseDuration <= _totalPhaseDuration)
        {
            var colorComponentValue = _phaseAnimation.Evaluate(curPhaseDuration / _totalPhaseDuration);
            var newSpriteColor = new Color(colorComponentValue, colorComponentValue, colorComponentValue, colorComponentValue);
            GetComponent<SpriteRenderer>().color = newSpriteColor;
            yield return null;
            curPhaseDuration += Time.deltaTime;
        }
        _speed = 0.01f;
        _isPhasing = false;
    }

    public IEnumerator PhaseOut()
    {
        _isPhasing = true;
        _speed = 0.0f;
        var curPhaseDuration = 0.0f;
        while (curPhaseDuration <= _totalPhaseDuration)
        {
            var colorComponentValue = 1.0f - (_phaseAnimation.Evaluate(curPhaseDuration / _totalPhaseDuration));
            Debug.Log($"CCV: {colorComponentValue}");
            var newSpriteColor = new Color(colorComponentValue, colorComponentValue, colorComponentValue, colorComponentValue);
            GetComponent<SpriteRenderer>().color = newSpriteColor;
            yield return null;
            curPhaseDuration += Time.deltaTime;
        }
        Debug.Log($"CCV: Switch!");
        EnemySpawner.SpawnEnemyAtOtherSide(transform.localPosition);
        Die();
    }

    private void Die()
    {
        Debug.Log($"Killed enemy position: {transform.localPosition.x}, {transform.localPosition.y}");
        EnemySpawner.AliveEnemies.Remove(this);
        Destroy(gameObject);
    }
}