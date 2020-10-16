using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int _difficulty = 5;

    protected override void Awake()
    {
        base.Awake();

        Events.OnMazeGenerated += OnMazeGenerated;
    }

    private void Start()
    {
        Events.OnTriggerStartGame?.Invoke(_difficulty);
    }

    private void OnDestroy()
    {
        Events.OnMazeGenerated -= OnMazeGenerated;
    }

    private void OnMazeGenerated(Vector3 position)
    {

    }
}