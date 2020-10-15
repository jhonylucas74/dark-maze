using UnityEngine;
using UnityEngine.UI;

public class LampBar : MonoBehaviour
{
    [SerializeField] Gradient _barGradient;

    Slider _slider;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();

        Events.OnBatteryLampUpdate += OnBatteryLampUpdate;
    }

    private void OnDestroy()
    {
        Events.OnBatteryLampUpdate -= OnBatteryLampUpdate;
    }

    void OnBatteryLampUpdate(float percentage)
    {
        _slider.value = percentage;
        _slider.targetGraphic.color = _barGradient.Evaluate(percentage);
    }
}
