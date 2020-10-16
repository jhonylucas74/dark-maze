using UnityEngine;
using UnityEngine.UI;

public class ButtonReloadLevel : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ReplayLevel());
    }
}
