using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpController : MonoBehaviour
{
    public SpriteRenderer TurretSpriteRenderer;
    
    public SpriteRenderer HealthSpriteRenderer;
    
    public SpriteRenderer TriShotSpriteRenderer;
    
    public SpriteRenderer NukeSpriteRenderer;
    
    public SpriteRenderer EmpSpriteRenderer;
    
    public SpriteRenderer CloudSpriteRenderer;

    public Planet PlayerPlanet;

    public int TurretCost;
    
    public int HealthCost;
    
    public int TriShotCost;
    
    public int NukeCost;
    
    public int EmpCost;
    
    public int CloudCost;

    public GameState _gameState;

    public Text TurretCostText;
    public Text HealCostText;
    public Text TriShostCostText;
    public Text NukeCostText;
    public Text EmpCostText;
    public Text CloudCostText;
    
    private readonly Color _grey = new Color(0.3f, 0.3f, 0.3f);
    private readonly Color _originalColor = new Color(1, 1, 1);

    private void Start()
    {
        TurretCostText.text = TurretCost.ToString();
        HealCostText.text = HealthCost.ToString();
        TriShostCostText.text = TriShotCost.ToString();
        NukeCostText.text = NukeCost.ToString();
        EmpCostText.text = EmpCost.ToString();
        CloudCostText.text = CloudCost.ToString();
    }

    private void FixedUpdate()
    {
        if (_gameState.IsGameOver)
        {
            return;
        }

        if (TurretCost <= PlayerPlanet.CoinCount)
        {
            TurretSpriteRenderer.color = _originalColor;
        }
        else
        {
            TurretSpriteRenderer.color = _grey;
        }
        
        if (HealthCost <= PlayerPlanet.CoinCount)
        {
            HealthSpriteRenderer.color = _originalColor;
        }
        else
        {
            HealthSpriteRenderer.color = _grey;
        }
        
        if (TriShotCost <= PlayerPlanet.CoinCount)
        {
            TriShotSpriteRenderer.color = _originalColor;
        }
        else
        {
            TriShotSpriteRenderer.color = _grey;
        }
        
        if (NukeCost <= PlayerPlanet.CoinCount)
        {
            NukeSpriteRenderer.color = _originalColor;
        }
        else
        {
            NukeSpriteRenderer.color = _grey;
        }
        
        if (EmpCost <= PlayerPlanet.CoinCount)
        {
            EmpSpriteRenderer.color = _originalColor;
        }
        else
        {
            EmpSpriteRenderer.color = _grey;
        }
        
        if (CloudCost <= PlayerPlanet.CoinCount)
        {
            CloudSpriteRenderer.color = _originalColor;
        }
        else
        {
            CloudSpriteRenderer.color = _grey;
        }
    }
}
