using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPaper : MonoBehaviour
{
    public Gradient gradient;

    Image _image;
    TMP_Text _text;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TMP_Text>();

        Events.OnPasswordGenerated += OnPasswordGenerated;
        Events.OnPasswordFound += OnPasswordFound;
        Events.OnLampIntensityUpdate += OnLampIntensityUpdate;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnPasswordGenerated -= OnPasswordGenerated;
        Events.OnPasswordFound -= OnPasswordFound;
        Events.OnLampIntensityUpdate -= OnLampIntensityUpdate;
    }

    void OnPasswordGenerated(string password)
    {
        _text.text = password;
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