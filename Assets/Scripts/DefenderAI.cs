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
                foreach (float ea in enemyAngles.OrderBy(x => x))
                {
                    float angleToTurn = (playerAngle - ea);

                    float mult = (angleToTurn < 0) ? -1 : 1;
                    for (int i = 1; i < Mathf.Abs(angleToTurn); i++)
                    {
                        transform.RotateAround(playerPlanetPosition, Vector3.back, mult);
                        yield return new WaitForSeconds(0.005f);
                    }

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
