using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordButton : MonoBehaviour
{
    public bool backspace;

    Button _btn;
    TMP_Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(() =>
        {
            Events.OnPasswordButtonPress?.Invoke(backspace ? "delete" : _text.text);
        });
    }
}
