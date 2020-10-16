using TMPro;
using UnityEngine;

public class TextCurrentLevel : MonoBehaviour
{
    TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        Events.OnTriggerStartGame += OnTriggerStartGame;
    }

    private void OnDestroy()
    {
        Events.OnTriggerStartGame -= OnTriggerStartGame;
    }

    void OnTriggerStartGame(int difficulty)
    {
        _text.text = $"Level {Mathf.Abs(difficulty - 4)}";
    }
}
