using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    static T _instance;
    public static T Instance { get => _instance; }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);

        Events.OnLeaveGame += OnLeaveGame;
    }

    private void OnDestroy()
    {
        Events.OnLeaveGame -= OnLeaveGame;
    }

    void OnLeaveGame()
    {
        Destroy(gameObject);
    }
}