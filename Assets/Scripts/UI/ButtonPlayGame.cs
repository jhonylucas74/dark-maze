using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonPlayGame : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => FadeManager.Instance.Fade(true, 0f, () => SceneManager.LoadSceneAsync("MainScene")));
    }

    private void Start()
    {
        FadeManager.Instance.Fade(false);
    }
}
