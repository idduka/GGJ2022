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
    [Tooltip("The game state object.")]
    private GameState _gameState;

    [SerializeField]
    [Tooltip("The sprite for a player defeat.")]
    private Sprite _defeatSprite;

    [SerializeField]
    [Tooltip("The sprite for a player victory.")]
    private Sprite _victorySprite;

    [SerializeField]
    [Tooltip("The sprite renderer for the game result.")]
    private SpriteRenderer _gameResultSpriteRenderer;

    public void SetVictory()
    {
        _gameState.IsGameOver = true;
        _gameResultSpriteRenderer.enabled = true;
        _gameResultSpriteRenderer.sprite = _victorySprite;

        foreach (var enemy in _enemySpawner.AliveEnemies)
        {
            Destroy(enemy.gameObject);
        }

        _enemySpawner.AliveEnemies.Clear();
    }

    public void SetDefeat()
    {
        _gameResultSpriteRenderer.enabled = true;
        _gameResultSpriteRenderer.sprite = _defeatSprite;

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
        _gameResultSpriteRenderer.enabled = false;
    }
}