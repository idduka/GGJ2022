using System.Collections;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Defender : MonoBehaviour
{
    public bool IsPlayer1 = true;
    public Planet HomePlanet;
    public float RadialSpeed = 180;
    public Projectile ProjectilePrefab;
    public GameState GameState;
    public EnemySpawner EnemySpawner;
    public Turret TurretPrefab;
    public ParticleSystem DirtTrail;
    public SmokeScreen SmokeScreen;
    public ParticleSystem NukeEffect;

    public int TurretCost { get { return 15; } }
    public int SmokeCost { get { return 15; } }
    public int HealCost { get { return 15; } }
    public int TrifireCost { get { return 15; } }
    public int EMPCost { get { return 15; } }
    public int NukeCost { get { return 15; } }

    private bool _trifireMode = false;
    
    // Start is called before the first frame update
    void Start()
    {
        DirtTrail.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            // Control and update defender properties while in Editor mode...
            if (HomePlanet != null)
            {
                transform.localPosition = new Vector3(0.0f, HomePlanet.Radius, 0.0f);
            }
        }
        else
        {
            if (GameState.IsGameOver)
            {
                return;
            }
            
            var horizontalAxisName = IsPlayer1 ? "P1Horizontal" : "P2Horizontal";
            var fireAxisName = IsPlayer1 ? "P1Fire" : "P2Fire";
            var turretAxisName = IsPlayer1 ? "P1Turret" : "P2Turret";
            var smokeAxisName = IsPlayer1 ? "P1Smoke" : "P2Smoke";
            var medkitName = IsPlayer1 ? "P1Medkit" : "P2Medkit";
            var trifireAxisName = IsPlayer1 ? "P1Trifire" : "P2Trifire";
            var empAxisName = IsPlayer1 ? "P1Emp" : "P2Emp";
            var nukeAxisName = IsPlayer1 ? "P1Nuke" : "P2Nuke";

            if (Input.GetButtonDown(turretAxisName) && HomePlanet.CoinCount >= TurretCost)
            {
                PlaceTurret();
                HomePlanet.CoinCount -= TurretCost;
            }
            if (Input.GetButtonDown(smokeAxisName) && HomePlanet.CoinCount >= SmokeCost)
            {
                SmokeScreen.Deploy();
                HomePlanet.CoinCount -= SmokeCost;
            }
            if (Input.GetButtonDown(medkitName) && HomePlanet.CoinCount >= HealCost)
            {
                if (HomePlanet._hitPoints < 99)
                {
                    HomePlanet.CoinCount -= HealCost;
                    HomePlanet.HealDamage();
                }
            }
            if (Input.GetButtonDown(trifireAxisName) && HomePlanet.CoinCount >= TrifireCost)
            {
                if (!_trifireMode)
                {
                    HomePlanet.CoinCount -= TrifireCost;
                    StartCoroutine(EnterTrifireMode());
                }
            }
            if (Input.GetButtonDown(empAxisName) && HomePlanet.CoinCount >= EMPCost)
            {
                if (!EnemySpawner.IsInEMPMode)
                {
                    HomePlanet.CoinCount -= EMPCost;
                    StartCoroutine(EnemySpawner.EnterEMPState(5f));
                }
            }
            if (Input.GetButtonDown(nukeAxisName) && HomePlanet.CoinCount >= NukeCost)
            {
                StartCoroutine(Nuke());
                HomePlanet.CoinCount -= NukeCost;
            }

            var horizontalAxisValue = Input.GetAxis(horizontalAxisName);
            if (horizontalAxisValue != 0)
            {
                DirtTrail.enableEmission = true;
                var rotation = RadialSpeed * Time.deltaTime * horizontalAxisValue;
                transform.RotateAround(HomePlanet.transform.position, Vector3.back, rotation);
            }
            else
            {
                DirtTrail.enableEmission = false;
            }
            if (Input.GetButtonDown(fireAxisName))
            {
                if (_trifireMode)
                {
                    TriFire();
                }
                else
                {
                    Fire();
                }
            }
        }
    }

    public void Fire()
    {
        AudioSource firesound = GetComponent <AudioSource>();
        firesound.Play(0);
        // start position for projectile is:
        // defender position + vector towards defender sprite center.
        Instantiate(ProjectilePrefab,
            transform.position + (transform.rotation * Vector3.up * GetComponent<SpriteRenderer>().bounds.size.y / 2),
            transform.rotation);
    }

    public void TriFire()
    {
        AudioSource firesound = GetComponent<AudioSource>();
        firesound.Play(0);

        // start position for projectile is:
        // defender position + vector towards defender sprite center.
        Instantiate(ProjectilePrefab,
            transform.position + (transform.rotation * Vector3.up * GetComponent<SpriteRenderer>().bounds.size.y / 2),
            transform.rotation * Quaternion.AngleAxis(20, Vector3.back));
        Instantiate(ProjectilePrefab,
            transform.position + (transform.rotation * Vector3.up * GetComponent<SpriteRenderer>().bounds.size.y / 2),
            transform.rotation);
        Instantiate(ProjectilePrefab,
            transform.position + (transform.rotation * Vector3.up * GetComponent<SpriteRenderer>().bounds.size.y / 2),
            transform.rotation * Quaternion.AngleAxis(-20, Vector3.back));
    }

    public void PlaceTurret()
    {
        var turret = Instantiate(TurretPrefab,
            transform.position + (transform.rotation * Vector3.up * 2),
            transform.rotation).GetComponent<Turret>();

        turret.EnemySpawner = EnemySpawner;
    }

    private IEnumerator EnterTrifireMode()
    {
        _trifireMode = true;
        yield return new WaitForSeconds(10.0f);
        _trifireMode = false;
    }

    public IEnumerator Nuke()
    {
        NukeEffect.Play();
        yield return new WaitForSeconds(2.0f);
        while(EnemySpawner.AliveEnemies.Count != 0)
        {
            EnemySpawner.AliveEnemies.First().Die();
        }
    }
}
