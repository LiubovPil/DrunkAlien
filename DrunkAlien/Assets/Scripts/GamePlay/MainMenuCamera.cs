using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The speed at which the camera rotates per second (in degrees)")]
    [SerializeField] private float _rotationSpeed = 30.0f;
    // The starting position of the object.
    private Vector3 _startPosition = new Vector3(500f, 100f, 500f);

    private void Start()
    {
        transform.position = _startPosition;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * Time.deltaTime * _rotationSpeed);
    }
}
