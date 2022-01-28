using System.Collections;
using System.Collections.Generic;
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

            if (Input.GetButtonDown(turretAxisName) && HomePlanet.CoinCount >= 15)
            {
                PlaceTurret();
                HomePlanet.CoinCount -= 15;
            }
            if (Input.GetButtonDown(smokeAxisName) && HomePlanet.CoinCount >= 15)
            {
                SmokeScreen.Deploy();
                HomePlanet.CoinCount -= 15;
            }
            if (Input.GetButtonDown(medkitName) && HomePlanet.CoinCount >= 15)
            {
                if (HomePlanet._hitPoints < 99)
                {
                    HomePlanet.CoinCount -= 15;
                    HomePlanet.HealDamage(2);
                }
            }
            if (Input.GetButtonDown(trifireAxisName) && HomePlanet.CoinCount >= 15)
            {
            }
            if (Input.GetButtonDown(empAxisName) && HomePlanet.CoinCount >= 15)
            {
            }
            if (Input.GetButtonDown(nukeAxisName) && HomePlanet.CoinCount >= 15)
            {
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
                Fire();
            }
        }
    }

    public void Fire()
    {
        AudioSource firesound = GetComponent <AudioSource>();
        firesound.Play(0);
        // start position for projectile is:
        // defender position + vector towards defender sprite center.
        var projectile = Instantiate(ProjectilePrefab,
            transform.position + (transform.rotation * Vector3.up * GetComponent<SpriteRenderer>().bounds.size.y / 2),
            transform.rotation);
    }

    private void PlaceTurret()
    {
        var turret = Instantiate(TurretPrefab,
            transform.position + (transform.rotation * Vector3.up * 2),
            transform.rotation).GetComponent<Turret>();

        turret.EnemySpawner = EnemySpawner;
    }
}
