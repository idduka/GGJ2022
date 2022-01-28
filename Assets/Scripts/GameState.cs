using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
    public bool IsGameOver { get; set; }

    private void OnEnable()
    {
        IsGameOver = false;
    }
}