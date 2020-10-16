using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    [SerializeField] float _batteryDecayRatio = 0.2f;
    float _batteryLife = 5f;
    float _currentBattery;

    Transform _transform;
    CharacterController _characterController;
    Animator _animator;
    Light _lampLight;

    bool _playing;
    bool _lampOn;
    Coroutine _batteryCoroutine;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _lampLight = GetComponentInChildren<Light>();

        Events.OnGameStart += OnGameStart;
        Events.OnGameEnd += OnGameEnd;
        Events.OnTriggerStartGame += OnTriggerStartGame;
        Events.OnMazeGenerated += OnMazeGenerated;
    }

    private void OnDestroy()
    {
        Events.OnGameStart -= OnGameStart;
        Events.OnGameEnd -= OnGameEnd;
        Events.OnTriggerStartGame -= OnTriggerStartGame;
        Events.OnMazeGenerated -= OnMazeGenerated;
    }

    void Update()
    {
        if (!_playing)
            return;

        RaycastHit raycastHit;
        Ray ray = new Ray(_transform.position, _transform.forward);
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("AlwaysLit")))
            if(raycastHit.distance - 0.3f <= 0.1f)
                Events.OnDoorTouching?.Invoke(raycastHit.collider.tag == "Door");
            else
                Events.OnDoorTouching?.Invoke(false);
        else
                Events.OnDoorTouching?.Invoke(false);

        ToggleLamp(Input.GetKey(KeyCode.Space));

        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        float magnitude = Mathf.Abs(direction.magnitude);

        _animator.SetBool("isWalking", magnitude > 0f);

        _characterController.SimpleMove(direction * _speed);

        if (magnitude <= 0) return;

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, 
                                Quaternion.LookRotation(direction.normalized * 360f, Vector3.up), 300f * Time.deltaTime);
    }

    void OnGameStart()
    {
        _playing = true;
    }

    void OnGameEnd()
    {
        _playing = false;
    }

    void ToggleLamp(bool toggle)
    {
        if (toggle)
        {
            if (_currentBattery > 0f && _batteryCoroutine == null && !_lampOn)
                _batteryCoroutine = StartCoroutine(BatteryRoutine());
        }
        else
        {
            if (_batteryCoroutine != null && _lampOn)
            {
                _lampOn = false;
                 Events.OnLightOff?.Invoke();
                _lampLight.DOIntensity(0f, 0.5f)
                            .OnUpdate(() => Events.OnLampIntensityUpdate?.Invoke(_lampLight.intensity))
                            .OnComplete(() =>
                            {
                                StopCoroutine(_batteryCoroutine);
                                _batteryCoroutine = null;
                            }).Play();
            }
        }
    }

    IEnumerator BatteryRoutine()
    {
        _lampOn = true;
         Events.OnLightOn?.Invoke();
        _lampLight.DOIntensity(1f, 0.25f).OnUpdate(() => Events.OnLampIntensityUpdate?.Invoke(_lampLight.intensity)).SetEase(Ease.OutSine).Play();

        while(true)
        {
            _currentBattery -= _batteryDecayRatio * Time.deltaTime;
            Events.OnBatteryLampUpdate?.Invoke(_currentBattery / _batteryLife);

            if (_currentBattery <= 0f)
                break;

            yield return null;
        }

        ToggleLamp(false);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch(hit.collider.tag)
        {
            case "KeyCode":
                hit.collider.gameObject.SetActive(false);
                Events.OnPasswordFound?.Invoke();
            break;
            /*case "Door":
                Events.OnDoorTouching?.Invoke(true);
            break;*/
        }
    }

    void OnTriggerStartGame(int difficulty)
    {
        _batteryLife = difficulty;
        _currentBattery = _batteryLife;
    }

    private void OnMazeGenerated(Vector3 position)
    {
        _transform.position = position + Vector3.up * 0.5f;
    }
}