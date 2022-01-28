using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    private enum PlanetState
    {
        Healthy,
        SlightlyDamaged,
        Damaged,
        VeryDamaged
    }

    [SerializeField]
    [Tooltip("The starting hitpoints of the planet")]
    private int _hitPoints;

    [SerializeField]
    [Tooltip("The sprite that is displayed when the planet is slightly damaged.")]
    private Sprite _planetSlightlyDamagedSprite;
    
    [SerializeField]
    [Tooltip("The sprite that is displayed when the planet is damaged.")]
    private Sprite _planetDamagedSprite;

    [SerializeField]
    [Tooltip("The sprite that is displayed when the planet is very damaged.")]
    private Sprite _planetVeryDamagedSprite;

    [SerializeField]
    [Tooltip("If the planet HP fall under this threshold, the slightly damaged sprite is displayed.")]
    private int _slightlyDamagedSpriteThreshold;
    
    [SerializeField]
    [Tooltip("If the planet HP fall under this threshold, the damaged sprite is displayed.")]
    private int _damagedSpriteThreshold;
    
    [SerializeField]
    [Tooltip("If the planet HP fall under this threshold, the very damaged sprite is displayed.")]
    private int _veryDamagedSpriteThreshold;

    [SerializeField]
    [Tooltip("The health bar that visualizes the planet's current health.")]
    private HealthBar _healthBar;

    [SerializeField]
    [Tooltip("The object that controls the victory.")]
    private VictoryController _victoryController;

    [SerializeField]
    [Tooltip("The text field displaying the number of coins.")]
    private TextMeshProUGUI _coinTextField;
    
    public float Radius = 1.0f;

    public int CoinCount { get; set; }
    
    private PlanetState _planetState = PlanetState.Healthy;
    //clintsc
    public void HealDamage(int damage)
    {
        _hitPoints += damage;

        _healthBar.SetValue(_hitPoints);
       


        SetSprite();
    }


    //
    public void ApplyDamage(int damage)
    {
        _hitPoints -= damage;

        _healthBar.SetValue(_hitPoints);
        AudioSource planetDamage = GetComponent<AudioSource>();
        planetDamage.Play(0);

        if (_hitPoints <= 0)
        {
            _victoryController.SetDefeat();
            return;
        }
        
        SetSprite();
    }

    private void Start()
    {
        _healthBar.SetMaxValue(_hitPoints);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            // Control and update planet properties while in Editor mode...
            transform.localScale = new Vector3(Radius, Radius, 1.0f);
        }
    }

    private void FixedUpdate()
    {
        _coinTextField.text = CoinCount.ToString();
    }

    private void SetSprite()
    {
        if (_hitPoints < _veryDamagedSpriteThreshold)
        {
            if (_planetState != PlanetState.VeryDamaged)
            {
                _planetState = PlanetState.VeryDamaged;
                GetComponent<SpriteRenderer>().sprite = _planetVeryDamagedSprite;    
            }
            
            return;
        }

        if (_hitPoints < _damagedSpriteThreshold)
        {
            if (_planetState != PlanetState.Damaged)
            {
                _planetState = PlanetState.Damaged;
                GetComponent<SpriteRenderer>().sprite = _planetDamagedSprite;
            }

            return;
        }
        
        if (_hitPoints < _slightlyDamagedSpriteThreshold)
        {
            if (_planetState != PlanetState.SlightlyDamaged)
            {
                _planetState = PlanetState.SlightlyDamaged;
                GetComponent<SpriteRenderer>().sprite = _planetSlightlyDamagedSprite;
            }

            return;
        }
    }
}
