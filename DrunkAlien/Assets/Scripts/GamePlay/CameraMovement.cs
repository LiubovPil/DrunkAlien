using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform _objectPos;
    
    //support for target tracking
    private Vector3 _distance = new Vector3(0, 25f, -30f);
    private Vector3 _destination;

    //support for drunk effect
    private float _magnitudeX = 20, _magnitudeY = 5, _offsetZangle = 7;
    private float _offsetX = 0, _offsetY = 0, _angleZ = 0;
    private float _timeStep = 0, _timeLimit = 10;

    void Start()
    {
        _objectPos = Managers.Instance.GameManager.MainCharacter.GetComponent<Transform>();

        transform.parent.position = _objectPos.transform.position + _distance;
        transform.parent.rotation = Quaternion.Euler(30f, 0, 0);
    }
    void LateUpdate()
    {
        _destination = _objectPos.transform.position + _distance;

        _destination.x = Mathf.Clamp(_destination.x, 0f, 400f);

        transform.parent.position = new Vector3(Mathf.SmoothStep(transform.parent.position.x, _destination.x, 1f), _destination.y, _destination.z);
    }

    void FixedUpdate()
    {
        if (_timeStep >= _timeLimit)
        {
            _timeStep = 0;
            Debug.Log(transform.localPosition);
        }
        _offsetX = Mathf.Sin((_timeStep / _timeLimit) * 2 * Mathf.PI) * _magnitudeX;
        _offsetY = Mathf.Sin((2 * _timeStep / _timeLimit) * 2 * Mathf.PI) * _magnitudeY;
        _angleZ = Mathf.Sin((_timeStep / _timeLimit) * 2 * Mathf.PI) * _offsetZangle;
        _timeStep += Time.fixedDeltaTime;

        transform.localPosition = new Vector3(_offsetX, _offsetY, 0f);
        transform.localRotation = Quaternion.Euler(0, 0, _angleZ);
    }
}
