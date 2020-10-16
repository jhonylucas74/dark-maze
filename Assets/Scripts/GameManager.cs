using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int _difficulty = 5;

    private void Start()
    {
        Events.OnTriggerStartGame?.Invoke(_difficulty);
    }

    public void OnMazeGenerated(Vector3 position)
    {
        Events.OnMazeGenerated?.Invoke(position);
        FadeManager.Instance.Fade(false, 0.5f, () => Events.OnGameStart?.Invoke());
    }

    public void TriggerNextLevel()
    {
        _difficulty++;
        ReloadScene();
    }

    public void ReplayLevel()
    {
        ReloadScene();
    }

    void ReloadScene()
    {
        Events.OnGameEnd?.Invoke();
        FadeManager.Instance.Fade(true, 1f, () =>
        {
            SceneManager.LoadSceneAsync("MainScene").completed += asyncOp =>
            {
                Events.OnTriggerStartGame?.Invoke(_difficulty);
            };
        });
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    TriggerNextLevel();
    }
}