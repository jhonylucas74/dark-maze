using UnityEngine;
using UnityEngine.UI;

public class LampBar : MonoBehaviour
{
    [SerializeField] Gradient _barGradient;

    Slider _slider;
    Image _Image;
    public Sprite SpriteLightOn;
    public Sprite SpriteLightOff;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _Image = GetComponentInChildren<Image>();
        Events.OnBatteryLampUpdate += OnBatteryLampUpdate;
        Events.OnLightOn += OnLightOn;
        Events.OnLightOff += OnLightOff;
    }

    private void OnDestroy()
    {
        Events.OnBatteryLampUpdate -= OnBatteryLampUpdate;
    }

    void OnLightOn () {
        _Image.sprite = SpriteLightOn;
    }

    void OnLightOff () {
        _Image.sprite = SpriteLightOff;
    }
    void OnBatteryLampUpdate(float percentage)
    {
        _slider.value = percentage;
        _slider.targetGraphic.color = _barGradient.Evaluate(percentage);
    }
}
