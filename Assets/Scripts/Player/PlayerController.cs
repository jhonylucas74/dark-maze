using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    [SerializeField] float _batteryLife = 5f;
    [SerializeField] float _batteryDecayRatio;
    float _currentBattery;

    Transform _transform;
    CharacterController _characterController;
    Animator _animator;
    Light _lampLight;

    bool _lampOn;
    Coroutine _batteryCoroutine;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _lampLight = GetComponentInChildren<Light>();

        _currentBattery = _batteryLife;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

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

    void ToggleLamp(bool toggle)
    {
        if (toggle)
        {
            if (_batteryCoroutine == null && !_lampOn)
                _batteryCoroutine = StartCoroutine(BatteryRoutine());
        }
        else
        {
            if (_batteryCoroutine != null && _lampOn)
            {
                _lampOn = false;
                Events.OnLightOff?.Invoke();
                _lampLight.DOIntensity(0f, 0.5f).OnComplete(() =>
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
        _lampLight.DOIntensity(1f, 0.25f).SetEase(Ease.OutSine).Play();
        Events.OnLightOn?.Invoke();

        while(true)
        {
            _currentBattery -= _batteryDecayRatio * Time.deltaTime;
            Events.OnBatteryLampUpdate?.Invoke(_currentBattery / _batteryLife);

            yield return null;
        }
    }
}