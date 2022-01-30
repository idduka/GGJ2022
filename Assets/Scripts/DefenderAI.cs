using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenderAI : MonoBehaviour
{
    [SerializeField]
    private Defender player;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private PowerUpController PowerUpController;

    [SerializeField]
    [Tooltip("The game state object.")]
    private GameState _gameState;

    private List<string> validDifficulties = new List<string> { "easy", "medium", "hard" };
    private string difficulty;

    private List<float> enemyAngles;
    private Vector2 playerPlanetPosition;

    private float angleStep;
    private float angleError;
    private WaitForSeconds calcFiringAnglesStep;
    private WaitForSeconds pauseBetweenEnemiesStep;

    private int turretsPlaced = 0;
    private int maxTurrets = 6;
    private System.DateTime timeOfLastTurret;
    private System.DateTime timeOfLastNuke;
    private System.DateTime timeOfLastSmoke;
    private System.DateTime timeOfLastEmp;
    private System.DateTime timeOfLastTrifire;

    // Start is called before the first frame update
    void Start()
    {
        timeOfLastTurret = System.DateTime.Now;
        timeOfLastNuke = System.DateTime.Now;
        timeOfLastSmoke = System.DateTime.Now;
        timeOfLastEmp = System.DateTime.Now;
        timeOfLastTrifire = System.DateTime.Now;

        enemyAngles = new List<float>();
        playerPlanetPosition = player.HomePlanet.transform.position;

        StartCoroutine(CalcFiringAngles());
        StartCoroutine(CalcUsePowerups());
    }

    public void SetDifficulty(string difficulty)
    {
        if (validDifficulties.Contains(difficulty))
        {
            this.difficulty = difficulty;
        }

        if (this.difficulty == "easy")
        {
            angleStep = 30;
            angleError = 10f;
            calcFiringAnglesStep = new WaitForSeconds(0.5f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.3f);
        }
        else if (this.difficulty == "medium")
        {
            angleStep = 20;
            angleError = 10f;
            calcFiringAnglesStep = new WaitForSeconds(0.25f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.2f);
        }
        else if (this.difficulty == "hard")
        {
            angleStep = 10;
            angleError = 5f;
            calcFiringAnglesStep = new WaitForSeconds(0.1f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    IEnumerator CalcFiringAngles()
    {
        while (!_gameState.IsGameOver)
        {
            yield return calcFiringAnglesStep;

            float playerAngle = AngleBetweenVector2(player.transform.position, playerPlanetPosition);

            if (enemySpawner.AliveEnemies != null && enemySpawner.AliveEnemies.Count > 0)
            {
                enemyAngles = new List<float>();

                foreach (var ea in enemySpawner.AliveEnemies.Where(x => !x.IsPhasing))
                {                    
                    float enemyAngle = AngleBetweenVector2(ea.transform.position, playerPlanetPosition) + Random.Range(0, (player._hasSmokeScreenShowing ? angleError + 10f : angleError));
                    enemyAngles.Add(enemyAngle);
                }
            }

            if (enemyAngles.Count > 0)
            {
                foreach (float ea in enemyAngles.OrderBy(x => x))
                {
                    float angleToTurn = (playerAngle - ea);

                    if (Mathf.Abs(angleToTurn) > 180)
                    {
                        float mult = (angleToTurn < 0) ? 1 : -1;
                        angleToTurn = (360 - Mathf.Abs(angleToTurn)) * mult;
                    }

                    if (Mathf.Abs(angleToTurn) < angleStep)
                    {
                        float mult = (angleToTurn < 0) ? -1 : 1;
                        for (int i = 1; i <= Mathf.Abs(angleToTurn); i++)
                        {
                            transform.RotateAround(playerPlanetPosition, Vector3.back, mult);
                            yield return null;
                        }
                    }
                    else
                    {
                        float toTurn = angleToTurn / angleStep;
                        for (int i = 1; i <= angleStep; i++)
                        {
                            transform.RotateAround(playerPlanetPosition, Vector3.back, toTurn);
                            yield return null;
                        }
                    }

                    playerAngle = AngleBetweenVector2(player.transform.position, playerPlanetPosition);
                    if (player._trifireMode)
                    {
                        player.TriFire();
                    }
                    else
                    {
                        player.Fire();
                    }

                    if (_gameState.IsGameOver)
                    {
                        yield break;
                    }
                    else
                    {
                        yield return pauseBetweenEnemiesStep;
                    }
                }

                enemyAngles = new List<float>();
            }
        }
    }

    IEnumerator CalcUsePowerups()
    {
        while (!_gameState.IsGameOver)
        {
            yield return calcFiringAnglesStep;

            if ((player.HomePlanet._hitPoints < 30
                || (player.HomePlanet._hitPoints < 75 && (Random.Range(0, 1f) < 0.05f)))
                && player.HomePlanet.CoinCount > PowerUpController.HealthCost)
            {
                player.HomePlanet.CoinCount -= PowerUpController.HealthCost;
                player.HomePlanet.HealDamage();
            }

            if (player.EnemySpawner.AliveEnemies != null
                && player.EnemySpawner.AliveEnemies.Count > 10
                && turretsPlaced < maxTurrets
                && player.HomePlanet.CoinCount > PowerUpController.TurretCost
                && (System.DateTime.Now - timeOfLastTurret).TotalSeconds > 15)
            {
                if (Random.Range(0, 1f) < 0.75f)
                {
                    float angle = 0;
                    float addAngle = 360 / maxTurrets;

                    while (player.PlayerTurrets.Any(x => x.SpawnAngle == angle) && angle <= 360)
                    {
                        angle += addAngle;
                    }

                    float playerAngle = AngleBetweenVector2(player.transform.position, playerPlanetPosition);
                    transform.RotateAround(playerPlanetPosition, Vector3.back, playerAngle);
                    transform.RotateAround(playerPlanetPosition, Vector3.back, angle);

                    player.HomePlanet.CoinCount -= PowerUpController.TurretCost;
                    player.PlaceTurret(angle);

                    turretsPlaced++;
                    timeOfLastTurret = System.DateTime.Now;

                    yield return null;
                }
            }

            if (player.EnemySpawner.AliveEnemies != null
                && (player.EnemySpawner.AliveEnemies.Count > 30
                || player.HomePlanet._hitPoints < 75 && player.EnemySpawner.AliveEnemies.Count > 20
                || player.HomePlanet._hitPoints < 50 && player.EnemySpawner.AliveEnemies.Count > 10)
                && player.HomePlanet.CoinCount > PowerUpController.NukeCost
                && (System.DateTime.Now - timeOfLastNuke).TotalSeconds > 30)
            {
                if (Random.Range(0, 1f) < 0.75f)
                {
                    player.HomePlanet.CoinCount -= PowerUpController.NukeCost;
                    StartCoroutine(player.Nuke());
                    timeOfLastNuke = System.DateTime.Now;

                    yield return null;
                }
            }

            if ((player?.EnemySpawner?._otherEnemySpawner?.AliveEnemies?.Count ?? 0) > 20
                && player.HomePlanet.CoinCount > PowerUpController.CloudCost
                && (System.DateTime.Now - timeOfLastSmoke).TotalSeconds > 30)
            {
                if (Random.Range(0, 1f) < 0.3f)
                {
                    player.HomePlanet.CoinCount -= PowerUpController.CloudCost;
                    player.SmokeScreen.Deploy();
                    timeOfLastSmoke = System.DateTime.Now;

                    yield return null;
                }
            }

            if (player.EnemySpawner.AliveEnemies != null
                && player.EnemySpawner.AliveEnemies.Count > 10
                && player.HomePlanet.CoinCount > PowerUpController.TriShotCost
                && (System.DateTime.Now - timeOfLastTrifire).TotalSeconds > 30)
            {
                if (Random.Range(0, 1f) < 0.3f)
                {
                    player.HomePlanet.CoinCount -= PowerUpController.TriShotCost;
                    StartCoroutine(player.EnterTrifireMode());
                    timeOfLastTrifire = System.DateTime.Now;

                    yield return null;
                }
            }

            if (player.EnemySpawner.AliveEnemies != null
                && player.EnemySpawner.AliveEnemies.Count > 10
                && player.EnemySpawner.AliveEnemies.ToDictionary(k => k, v => Vector2.Distance(transform.position, v.transform.position)).OrderBy(d => d.Value).FirstOrDefault().Value < 1
                && player.HomePlanet.CoinCount > PowerUpController.EmpCost
                && (System.DateTime.Now - timeOfLastEmp).TotalSeconds > 30)
            {
                if (Random.Range(0, 1f) < 0.3f)
                {
                    player.HomePlanet.CoinCount -= PowerUpController.EmpCost;
                    StartCoroutine(player.EnemySpawner.EnterEMPState(5f));
                    timeOfLastEmp = System.DateTime.Now;

                    yield return null;
                }
            }

            player.PlayerTurrets = player.PlayerTurrets.Where(x => x.GetTurretAmmo() > 0).ToList();
            turretsPlaced = player.PlayerTurrets.Count;
        }
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

}
