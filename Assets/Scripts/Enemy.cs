using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("The speed at which the enemy flies towards the planet")]
    public float _speed;

    [SerializeField]
    [Tooltip("The sprite that is applied when the enemy respawns on the other player side.")]
    private Sprite _respawnSprite;

    [SerializeField]
    [Tooltip("The total time it would take to phase out or phase in.")]
    private float _totalPhaseDuration;

    [SerializeField]
    [Tooltip("The transition curve for phasing out and in.")]
    private AnimationCurve _phaseAnimation;

    [SerializeField]
    [Tooltip("The damage the enemy deals to the planet on contact.")]
    private int _damageToPlanet;

    public bool IsPhasing { get; private set; }

    public bool IsBeingAffectedByEMP
    {
        get
        {
            return _isBeingAffectedByEMP;
        }
        set
        {
            if (value == true)
            {
                SparksEffect.Play();
            }
            _isBeingAffectedByEMP = value;
        }
    }
    private bool _isBeingAffectedByEMP;

    private SpriteRenderer _spriteRenderer;

    public Planet TargetPlanet { get; set; }

    public int HitPoints { get; set; }

    public bool IsRespawn { get; set; }

    public EnemySpawner EnemySpawner { private get; set; }

    public GameObject GibPrefab;

    public GameObject ExplosionPrefab;

    public ParticleSystem SparksEffect;

    private void Start()
    {
        if (IsRespawn)
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
        if (!IsBeingAffectedByEMP)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPlanet.transform.position, _speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPhasing)
        {
            return;
        }

        var planet = other.GetComponent<Planet>();

        // Debug.Log($"HIT PLANET: {planet != null}");

        if (planet != null)
        {
            planet.ApplyDamage(_damageToPlanet);
            EnemySpawner.AliveEnemies.Remove(this);
            Destroy(gameObject);
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            return;
        }

        var projectile = other.GetComponent<Projectile>();

        if (projectile != null)
        {
            Destroy(projectile.gameObject);
            Die();
        }
    }

    public IEnumerator PhaseIn()
    {
        IsPhasing = true;
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
        IsPhasing = false;
    }

    public IEnumerator PhaseOut()
    {
        var originalSpeed = _speed;
        IsPhasing = true;
        _speed = 0.0f;
        var curPhaseDuration = 0.0f;
        while (curPhaseDuration <= _totalPhaseDuration)
        {
            var colorComponentValue = 1.0f - (_phaseAnimation.Evaluate(curPhaseDuration / _totalPhaseDuration));
            var newSpriteColor = new Color(colorComponentValue, colorComponentValue, colorComponentValue, colorComponentValue);
            GetComponent<SpriteRenderer>().color = newSpriteColor;
            yield return null;
            curPhaseDuration += Time.deltaTime;
        }
        EnemySpawner.SpawnEnemyAtOtherSide(transform.localPosition, originalSpeed);
        Destroy(gameObject);
    }

    public void Die()
    {
        // Debug.Log($"Killed enemy position: {transform.localPosition.x}, {transform.localPosition.y}");
        EnemySpawner.AliveEnemies.Remove(this);
        if (IsRespawn)
        {
            Instantiate(GibPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            TargetPlanet.CoinCount++;
            StartCoroutine(PhaseOut());
        }
    }
}