using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The current number of cows")]
    [SerializeField] private Text _cowsScore;
    [Tooltip("The display of the traveled path")]
    [SerializeField] private Slider _progressBar;

    private const string _cowsScorePrefix = "Cows: ";

    //data from GameManager
    private Transform _objectPos;
    private float _score;
    private int _finishPosZ;

    private float _valueSlider;

    private float _currentPosZ = 0f;

    void Start()
    {
        Managers.Instance.EventManager.StartListening(EventNames.MovingStartEvent, Initialized);
        Managers.Instance.EventManager.StartListening(EventNames.CowLostEvent, LostCow);

        Initialized(StageName.Level1Window);

    }
    void Update()
    {
        _currentPosZ = _objectPos.transform.position.z;
        _valueSlider = Mathf.Clamp01(_currentPosZ / _finishPosZ);
        _progressBar.value = _valueSlider;
    }
    void Initialized(StageName inputStage)
    {
        Debug.Log("Update data...");

        _finishPosZ = Managers.Instance.GameManager.TotalPathLenght;
        _score = Managers.Instance.GameManager.NumberOfCows;

        _cowsScore.text = _cowsScorePrefix + _score.ToString();

        _objectPos = Managers.Instance.GameManager.MainCharacter.GetComponent<Transform>();

        Debug.Log("Data were update!");
    }
    void LostCow(StageName inputStage)
    {
        _score--;
        _cowsScore.text = _cowsScorePrefix + _score.ToString();

        if(_score <= 0)
        {
            _score = 0;
            _cowsScore.text = _cowsScorePrefix + _score.ToString();
            Managers.Instance.EventManager.TriggerEvent(EventNames.LastCowLostEvent, StageName.FinalWindow);
        }
    }
}
