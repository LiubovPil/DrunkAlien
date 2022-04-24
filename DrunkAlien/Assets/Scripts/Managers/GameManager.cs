using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The target that we are following")]
    [SerializeField] private GameObject _mainCharacter;
    [Tooltip("Ñurrent game stage")]
    [SerializeField] private StageName _currentStage;
    
    //The explosion of drunk alien
    private GameObject _drunkAlienExplosion;
    private ParticleSystem _drunkAlienParticleSystem;

    //path traveled by the target and path length
    private const int _totalPathLength = 5000;

    //information about cows
    private float _numberOfCows = 10f;

    //flying area boundaries
    private int _leftPositionX = 120;
    private int _rightPositionX = 280;

    //support for spawning tree
    private int _levelTreeNumber;
    private int _level1TreeNumber = 10;
    private int _level2TreeNumber = 15;
    
    #region Properties
    public StageName CurrentStage
    {
        get { return _currentStage; }
    }
    public GameObject MainCharacter
    {
        get { return _mainCharacter; }
    }
    public float NumberOfCows
    {
        get { return _numberOfCows; }
    }
    public int TotalPathLenght
    {
        get { return _totalPathLength; }
    }
    public int LeftPositionX
    {
        get { return _leftPositionX; }
    }
    public int RightPositionX
    {
        get { return _rightPositionX; }
    }
    public int LevelTreeNumber
    {
        get { return _levelTreeNumber; }
        private set { _levelTreeNumber = value; }
    }
    #endregion

    private void Awake()
    {
        string nameScene = SceneManager.GetActiveScene().name;
        
        if (nameScene == "MainMenu")
        {
            Debug.Log("Game manager was awaked in MainMenu");
            _currentStage = StageName.MainMenuWindow;
        }
        else
        {
            _currentStage = StageName.Level1Window;
            Debug.Log("Game manager was awaked in Level 1");
            //GoToLevel1(); //If Manager was loaded in scene "GamePlay" firstly, the drunk alien isn't move
        }
        _drunkAlienExplosion = Resources.Load<GameObject>("DrunkAlienExplosion");
    }
    private void Start()
    {
        SetEvents();

        if (_currentStage == StageName.Level1Window)
        {
            GoToLevel1();
        }
        Managers.Instance.AudioManager.PlayMusic(AudioClipName.MainTheme);
    }
    private void ShuffleStage(StageName currentStage)
    {
        switch (currentStage)
        {
            case StageName.MainMenuWindow:
                _currentStage = StageName.Level1Window;
                GoToLevel1();
                break;
            case StageName.Level1Window:
                _currentStage = StageName.Level2Window;
                GoToLevel2();
                break;
            case StageName.Level2Window:
                _currentStage = StageName.FinalWindow;
                GoToWin();
                break;
            case StageName.FinalWindow:
                _currentStage = StageName.MainMenuWindow;
                GoToMain();
                break;
            default:
                break;
        }
    }
    private void SetEvents()
    {
        Managers.Instance.EventManager.StartListening(EventNames.StageChangeEvent, ShuffleStage);
        Managers.Instance.EventManager.StartListening(EventNames.LastCowLostEvent, CompleteTheGame);

        SceneManager.activeSceneChanged += CheckScene;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

#region Stage switches

    private void CheckScene(Scene current, Scene next)
    {
        Managers.Instance.AudioManager.PlayMusic(AudioClipName.MainTheme);
        ShuffleStage(_currentStage);
    }
    private void GoToMain()
    {
        _mainCharacter = null;
    }
    private void GoToLevel1()
    {
        SetMainCharacter(_currentStage);

        Managers.Instance.EventManager.TriggerEvent(EventNames.ResetSceneEvent, _currentStage);

        LevelTreeNumber = _level1TreeNumber;

        Managers.Instance.EventManager.TriggerEvent(EventNames.MovingStartEvent, _currentStage);
        
        Debug.Log("Settings for level 1 was instaled");
    }
    private void GoToLevel2()
    {
        LevelTreeNumber = _level2TreeNumber;

        Object.Instantiate(Resources.Load("Level2Window"));
    }
    private void GoToWin()
    {
        Object.Instantiate(Resources.Load("WinWindow"));
    }
    private void SetMainCharacter(StageName inputStage)
    {
        if (_mainCharacter == null)
        {
            _mainCharacter = GameObject.Find("DrunkAlien");
        }
        if(_mainCharacter != null && _mainCharacter.name == "DrunkAlien")
        {
            Debug.Log("MainCharacter is drunk alien");
        }
    }
    private void CompleteTheGame(StageName inputStage)
    {
        _currentStage = inputStage;

        GameObject explosion = Instantiate(_drunkAlienExplosion, _mainCharacter.transform.position, Quaternion.identity);
        _drunkAlienParticleSystem = explosion.GetComponent<ParticleSystem>();
        var main = _drunkAlienParticleSystem.main;
        main.useUnscaledTime = true;

        _mainCharacter.GetComponentInChildren<MeshRenderer>().enabled = false;

        Object.Instantiate(Resources.Load("GameOverWindow"));
    }
    #endregion
}
