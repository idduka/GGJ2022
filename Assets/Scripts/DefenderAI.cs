using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderAI : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawner.AliveEnemies != null && enemySpawner.AliveEnemies.Count > 0)
        {
            Debug.Log("Enemies: " + enemySpawner.AliveEnemies.Count);
            Vector3 enemyPos = enemySpawner.AliveEnemies[0].transform.position;
            Debug.Log("x: " + enemyPos.x + ", y:" + enemyPos.y);
        }
    }
}
