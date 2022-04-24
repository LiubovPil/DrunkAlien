using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    private GameManager _gameManager;
    private EventManager _eventManager;
    private AudioManager _audioManager;
    public static Managers Instance //singelton pattern
    {
        get
        {
            if (!_instance)
            {
                GameObject managers = GameObject.Find("Managers");

                if (managers == null)
                {
                    managers = Instantiate(Resources.Load<GameObject>("Managers"));
                }
            }
            return _instance;
        }
    }
    #region GameManager, EventManager and AudioManager Properties
    public GameManager GameManager
    {
        get { return _gameManager; }
    }
    public EventManager EventManager
    {
        get {return _eventManager; }
    }
    public AudioManager AudioManager
    {
        get { return _audioManager; }
    }
    #endregion

    private void Awake()
    {
        /*if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);

            _gameManager = GetComponentInChildren<GameManager>() as GameManager;
            _eventManager = GetComponentInChildren<EventManager>() as EventManager;
        }
        else if(_instance != null && Instance != this)
        {
            Destroy(gameObject);
        }*/
        if (_instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Duplicated manager was deleted");
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
            Debug.Log("Manager was instaled");
        }
        _gameManager = GetComponentInChildren<GameManager>() as GameManager;
        _eventManager = GetComponentInChildren<EventManager>() as EventManager;
        _audioManager = GetComponentInChildren<AudioManager>() as AudioManager;
    }
}
