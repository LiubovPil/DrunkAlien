using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkAlienMovement : MonoBehaviour
{
    //component of the player
    private Rigidbody _rb;

    //support for movement
    private Vector3 _moveDirection;
    private int _speedLevel1 = 75, _speedLevel2 = 150;

    [Tooltip("The default speed of the target")]
    [SerializeField] private float _speedLevel;
    [Tooltip("The current speed of the target - z axis")]
    [SerializeField] private float _currentSpeed;
    [Tooltip("The current speed of the target - x axis")]
    [SerializeField] private float _moveXForce;

    private const float _increaseZspeed = 75f;
    private const float _increaseXspeed = 20f;

    //indicates whether the target is currently moving
    private bool _isMoving;

    private float _defaultMoveXForce = 50f;
    private float _incline = 0.05f;

    private int _totalPath;

    public float CurrentSpeed
    {
        get
        {
            if (_currentSpeed == 0)
            {
                return _speedLevel1;
            }
            else
            {
                return _currentSpeed;
            }
        }
    }
    public float MoveXFoce
    {
        get { return _defaultMoveXForce; }
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        Managers.Instance.EventManager.StartListening(EventNames.MovingStartEvent, StartMoving); //was in start method
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); //get the Input from Horizontal axis

        if (transform.position.z <= _totalPath)
        {
            _moveDirection = new Vector3(horizontalInput * _moveXForce, 0, _currentSpeed);

            _currentSpeed = _speedLevel + ((transform.position.z / _totalPath) * _increaseZspeed);
            _moveXForce = _defaultMoveXForce + ((transform.position.z / _totalPath) * _increaseXspeed);
        }
        else if (_isMoving == true)
        {
            _isMoving = false; //otherwise, the event may be triggered multiple times

            Managers.Instance.AudioManager.PlaySound(AudioClipName.WinSound);
            Managers.Instance.EventManager.TriggerEvent(EventNames.StageChangeEvent, Managers.Instance.GameManager.CurrentStage);
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = _moveDirection;
        _rb.rotation = Quaternion.Euler(0, 0, -_rb.velocity.x * _incline);
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll is BoxCollider)
        {
            Managers.Instance.EventManager.TriggerEvent(EventNames.ResetSceneEvent, Managers.Instance.GameManager.CurrentStage);
            Managers.Instance.EventManager.TriggerEvent(EventNames.MovingStartEvent, Managers.Instance.GameManager.CurrentStage);
        }
    }
    void StartMoving(StageName inputStage)
    {
        this.transform.position = new Vector3(195f, 100f, 50f);

        if (inputStage == StageName.Level2Window)
        {
            _speedLevel = _speedLevel2;
        }
        else
        {
            _speedLevel = _speedLevel1;
        }
        _currentSpeed = _speedLevel;

        _moveXForce = _defaultMoveXForce;

        _totalPath = Managers.Instance.GameManager.TotalPathLenght;

        _isMoving = true;
    }
}
