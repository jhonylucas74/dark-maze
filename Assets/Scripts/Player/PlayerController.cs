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

    bool _hasCode;

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

        RaycastHit raycastHit;
        Ray ray = new Ray(_transform.position, _transform.forward);
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("AlwaysLit")))
            if(raycastHit.distance - 0.3f <= 0.1f)
                Events.OnDoorTouching?.Invoke(raycastHit.collider.tag == "Door");
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
        _lampLight.DOIntensity(1f, 0.25f).OnUpdate(() => Events.OnLampIntensityUpdate?.Invoke(_lampLight.intensity)).SetEase(Ease.OutSine).Play();

        while(true)
        {
            _currentBattery -= _batteryDecayRatio * Time.deltaTime;
            Events.OnBatteryLampUpdate?.Invoke(_currentBattery / _batteryLife);

            yield return null;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch(hit.collider.tag)
        {
            case "KeyCode":
                _hasCode = true;
                hit.collider.gameObject.SetActive(false);
                Events.OnPasswordFound?.Invoke();
            break;
            /*case "Door":
                Events.OnDoorTouching?.Invoke(true);
            break;*/
        }
    }
}