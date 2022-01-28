using System;
using TMPro;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The enemy spawner for this player.")]
    private EnemySpawner _enemySpawner;

    [SerializeField]
    [Tooltip("The victory controller of the other player.")]
    private VictoryController _otherVictoryController;

    [SerializeField]
    [Tooltip("The game result text for the player.")]
    private TextMeshProUGUI _gameResultText;
    
    [SerializeField]
    [Tooltip("The game state object.")]
    private GameState _gameState;
    
    public void SetVictory()
    {
        _gameState.IsGameOver = true;
        _gameResultText.enabled = true;
        _gameResultText.text = "Victory";
        
        foreach (var enemy in _enemySpawner.AliveEnemies)
        {
            Destroy(enemy.gameObject);
        }
        
        _enemySpawner.AliveEnemies.Clear();
    }

    public void SetDefeat()
    {
        _gameResultText.enabled = true;
        _gameResultText.text = "Defeat";

        _otherVictoryController.SetVictory();
        
        foreach (var enemy in _enemySpawner.AliveEnemies)
        {
            Destroy(enemy.gameObject);
        }
        
        _enemySpawner.AliveEnemies.Clear();
    }

    private void Start()
    {
        _gameState.IsGameOver = false;
        _gameResultText.enabled = false;
    }
}