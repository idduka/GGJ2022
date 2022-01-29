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

    // Start is called before the first frame update
    void Start()
    {
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
            angleStep = 180;
            angleError = 20f;
            calcFiringAnglesStep = new WaitForSeconds(1f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.3f);
        }
        else if (this.difficulty == "medium")
        {
            angleStep = 60;
            angleError = 15f;
            calcFiringAnglesStep = new WaitForSeconds(0.6f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.2f);
        }
        else if (this.difficulty == "hard")
        {
            angleStep = 30;
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
                    float enemyAngle = AngleBetweenVector2(ea.transform.position, playerPlanetPosition) + Random.Range(0, angleError);
                    enemyAngles.Add(enemyAngle);
                }
            }

            if (enemyAngles.Count > 0)
            {
                foreach (float ea in enemyAngles.OrderBy(x => x))
                {
                    float angleToTurn = (playerAngle - ea);

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
                    player.Fire();

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

            int money = player.HomePlanet.CoinCount;

            if (turretsPlaced < maxTurrets && money > player.TurretCost)
            {
                turretsPlaced++;

                float angle = 360 / maxTurrets * turretsPlaced;
                float playerAngle = AngleBetweenVector2(player.transform.position, playerPlanetPosition);
                transform.RotateAround(playerPlanetPosition, Vector3.back, playerAngle);
            }
        }
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

}
