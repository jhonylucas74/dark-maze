using DG.Tweening;
using TMPro;
using UnityEngine;

public class DoorPassword : MonoBehaviour
{
    const int PASSWORD_SIZE = 4;

    AudioSource _audioSrc;
    CanvasGroup _canvasGroup;
    [SerializeField] TMP_Text _passwordText;
    [SerializeField] AudioClip _correctSfx;
    [SerializeField] AudioClip _wrongSfx;

    string _generatedPassword;
    string _password = string.Empty;

    private void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
        _canvasGroup = GetComponent<CanvasGroup>();

        for (int i = 0; i < PASSWORD_SIZE; i++)
            _generatedPassword += Random.Range(0, 9);

        Events.OnDoorTouching += OnDoorTouching;
        Events.OnPasswordButtonPress += OnButtonPress;
    }

    private void Start()
    {
        Events.OnPasswordGenerated?.Invoke(_generatedPassword);
    }

    private void OnDestroy()
    {
        _canvasGroup.DOKill();
        Events.OnDoorTouching -= OnDoorTouching;
        Events.OnPasswordButtonPress -= OnButtonPress;
    }

    void OnDoorTouching(bool toggle)
    {
        _canvasGroup.interactable = toggle;
        _canvasGroup.blocksRaycasts = toggle;
        _canvasGroup.DOFade(toggle ? 1f : 0f, 0.5f).Play();
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
            if (_password == _generatedPassword)
            {
                _canvasGroup.interactable = false;

                _audioSrc.clip = _correctSfx;
                _audioSrc.Play();

                GameManager.Instance.TriggerNextLevel();
            }
            else
            {
                _audioSrc.clip = _wrongSfx;
                _audioSrc.Play();
            }
        }
    }
}