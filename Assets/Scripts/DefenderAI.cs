using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderAI : MonoBehaviour
{
    [SerializeField]
    private Planet playerPlanet;

    [SerializeField]
    private Defender player;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private int lastAliveEnemiesCalculation;
    private List<float> enemyAngles;
    private Vector2 playerPlanetPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastAliveEnemiesCalculation = 0;
        enemyAngles = new List<float>();
        playerPlanetPosition = playerPlanet.transform.position;
        StartCoroutine(Ommok());
    }

    void Update()
    {

    }

    // Update is called once per frame
    IEnumerator Ommok()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float playerAngle = AngleBetweenVector2(player.transform.position, playerPlanetPosition);

            if (enemySpawner.AliveEnemies != null && enemySpawner.AliveEnemies.Count > 0 && enemySpawner.AliveEnemies.Count != lastAliveEnemiesCalculation)
            {
                enemyAngles = new List<float>();
                lastAliveEnemiesCalculation = enemySpawner.AliveEnemies.Count;

                foreach (var ea in enemySpawner.AliveEnemies)
                {
                    float enemyAngle = AngleBetweenVector2(ea.transform.position, playerPlanetPosition);
                    enemyAngles.Add(enemyAngle);
                }
            }

            if (enemyAngles.Count > 0)
            {
                foreach (float ea in enemyAngles)
                {
                    float angleToTurn = (playerAngle - ea);

                    while (angleToTurn > 10)
                    {
                        float actualTurnAngle = Mathf.Max(10, angleToTurn);
                        transform.RotateAround(playerPlanetPosition, Vector3.back, actualTurnAngle);
                        angleToTurn = angleToTurn - actualTurnAngle;
                        yield return new WaitForSeconds(1f);
                    }

                    transform.RotateAround(playerPlanetPosition, Vector3.back, angleToTurn);

                    playerAngle = AngleBetweenVector2(player.transform.position, playerPlanetPosition);
                    player.Fire();
                }

                enemyAngles = new List<float>();
            }

            lastAliveEnemiesCalculation = enemySpawner.AliveEnemies.Count;
        }
    }


    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

}
