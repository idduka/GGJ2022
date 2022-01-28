using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The sprite that represents the actual health bar filling.")]
    private Image _healthBarFilling;
    
    private float _widthPerValue;

    public void SetMaxValue(int maxValue)
    {
        var maxWidth = _healthBarFilling.rectTransform.rect.width;

        _widthPerValue = maxWidth / maxValue;
    }
    
    public void SetValue(int newValue)
    {
        var newWidth = _widthPerValue * newValue;

        _healthBarFilling.rectTransform.sizeDelta = new Vector2(newWidth, _healthBarFilling.rectTransform.sizeDelta.y);
    }
}
