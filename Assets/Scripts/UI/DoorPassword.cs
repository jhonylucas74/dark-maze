using DG.Tweening;
using TMPro;
using UnityEngine;

public class DoorPassword : MonoBehaviour
{
    const int PASSWORD_SIZE = 4;

    CanvasGroup _canvasGroup;
    [SerializeField] TMP_Text _passwordText;
    [SerializeField] TMP_Text _paperText;

    string _generatedPassword;
    string _password = string.Empty;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        for (int i = 0; i < PASSWORD_SIZE; i++)
            _generatedPassword += Random.Range(0, 9);

        _paperText.text = _generatedPassword;

        Events.OnDoorTouching += OnDoorTouching;
        Events.OnPasswordButtonPress += OnButtonPress;
    }

    private void OnDestroy()
    {
        Events.OnDoorTouching -= OnDoorTouching;
        Events.OnPasswordButtonPress -= OnButtonPress;
    }

    void OnDoorTouching(bool toggle)
    {
        _canvasGroup.interactable = toggle;
        _canvasGroup.blocksRaycasts = toggle;
        _canvasGroup.DOFade(toggle ? 1f : 0f, 1f).Play();
    }

    void OnButtonPress(string key)
    {
        if (key == "delete")
        {
            key = string.Empty;
            if (_password.Length > 0)
            {
                _password = _password.Remove(_password.Length - 1);
                _passwordText.text = _password;
            }

            return;
        }

        if (_password.Length >= PASSWORD_SIZE)
            return;

        _password += key;
        _passwordText.text = _password;

        if(_password.Length >= PASSWORD_SIZE)
        {
            Debug.Log(_password == _generatedPassword ? "Correct" : "Wrong");
        }
    }
}