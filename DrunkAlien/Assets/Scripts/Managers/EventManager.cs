using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager: MonoBehaviour
{
    private Dictionary<EventNames, UnityEvent<StageName>> _eventsDictionary = new Dictionary<EventNames, UnityEvent<StageName>>();

    private void Awake()
    {
        if (_eventsDictionary == null)
        {
            _eventsDictionary = new Dictionary<EventNames, UnityEvent<StageName>>();
        }
    }
    public void StartListening(EventNames eventName, UnityAction<StageName> listener)
    {
        UnityEvent<StageName> thisEvent = null;
        if(_eventsDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<StageName>();
            thisEvent.AddListener(listener);
            _eventsDictionary.Add(eventName, thisEvent);
        }
    }
    public void StopListening(EventNames eventName, UnityAction<StageName> listener)
    {
        if (_eventsDictionary == null)
        {
            return;
        }
        UnityEvent<StageName> thisEvent = null;
        if (_eventsDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public void TriggerEvent(EventNames eventName, StageName stage)
    {
        UnityEvent<StageName> thisEvent = null;
        if (_eventsDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(stage);
        }
    }
}

