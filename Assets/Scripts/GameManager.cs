using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;

    private void Awake()
    {
        Events.OnMazeGenerated += OnMazeGenerated;
    }

    private void OnDestroy()
    {
        Events.OnMazeGenerated -= OnMazeGenerated;
    }

    private void OnMazeGenerated(Vector3 position)
    {
        _playerTransform.position = position;
    }
}