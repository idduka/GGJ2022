using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenderAI : MonoBehaviour
{
    [SerializeField]
    private Planet playerPlanet;

    [SerializeField]
    private Defender player;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private List<string> validDifficulties = new List<string> { "easy", "medium", "hard" };
    private string difficulty;

    private List<float> enemyAngles;
    private Vector2 playerPlanetPosition;

    private float angleError;
    private WaitForSeconds calcFiringAnglesStep;
    private WaitForSeconds pauseBetweenEnemiesStep;

    // Start is called before the first frame update
    void Start()
    {
        difficulty = "hard";
        enemyAngles = new List<float>();
        playerPlanetPosition = playerPlanet.transform.position;        
    }

    public void SetDifficulty(string difficulty)
    {
        if (validDifficulties.Contains(difficulty))
        {
            this.difficulty = difficulty;
        }

        if (this.difficulty == "easy")
        {
            angleError = 20f;
            calcFiringAnglesStep = new WaitForSeconds(1f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.5f);
        }
        else if (this.difficulty == "medium")
        {
            angleError = 15f;
            calcFiringAnglesStep = new WaitForSeconds(0.6f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.2f);
        }
        else if (this.difficulty == "hard")
        {
            angleError = 5f;
            calcFiringAnglesStep = new WaitForSeconds(0.1f);
            pauseBetweenEnemiesStep = new WaitForSeconds(0.1f);
        }

        StartCoroutine(CalcFiringAngles());
    }

    // Update is called once per frame
    IEnumerator CalcFiringAngles()
    {
        while (true)
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

                    if (Mathf.Abs(angleToTurn) < 45)
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
                        float toTurn = angleToTurn / 45;
                        for (int i = 1; i <= 45; i++)
                        {
                            transform.RotateAround(playerPlanetPosition, Vector3.back, toTurn);
                            yield return null;
                        }
                    }

                    playerAngle = AngleBetweenVector2(player.transform.position, playerPlanetPosition);
                    player.Fire();

                    yield return pauseBetweenEnemiesStep;
                }

                enemyAngles = new List<float>();
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
