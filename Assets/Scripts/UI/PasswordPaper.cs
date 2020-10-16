using UnityEngine;
using UnityEngine.UI;

public class PasswordPaper : MonoBehaviour
{
    public Gradient gradient;

    Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();

        Events.OnPasswordFound += OnPasswordFound;
        Events.OnLampIntensityUpdate += OnLampIntensityUpdate;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnPasswordFound -= OnPasswordFound;
        Events.OnLampIntensityUpdate -= OnLampIntensityUpdate;
    }

    void OnPasswordFound()
    {
        gameObject.SetActive(true);
    }

    void OnLampIntensityUpdate(float value)
    {
        _image.color = gradient.Evaluate(value);
    }
}