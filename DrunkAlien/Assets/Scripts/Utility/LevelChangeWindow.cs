using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangeWindow : MonoBehaviour
{
    public void Start()
    {
        Managers.Instance.EventManager.TriggerEvent(EventNames.ResetSceneEvent, Managers.Instance.GameManager.CurrentStage);

        Time.timeScale = 0;
    }
    public void Level2ButtonOnclick()
    {
        Time.timeScale = 1;
        Managers.Instance.EventManager.TriggerEvent(EventNames.MovingStartEvent, Managers.Instance.GameManager.CurrentStage);
        Destroy(gameObject);
    }
    public void PlaySound()
    {
        Managers.Instance.AudioManager.PlaySound(AudioClipName.UISound);
    }
}
