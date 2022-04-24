using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    private void Start()
    {
        Managers.Instance.EventManager.TriggerEvent(EventNames.ResetSceneEvent, Managers.Instance.GameManager.CurrentStage);

        Time.timeScale = 0;
    }
    public void MainMenuButtonOnclick()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
        //Managers.Instance.EventManager.TriggerEvent(EventNames.StageChangeEvent, Managers.Instance.GameManager.CurrentStage);
        SceneManager.LoadScene("MainMenu");
    }
    public void PlaySound()
    {
        Managers.Instance.AudioManager.PlaySound(AudioClipName.UISound);
    }
}
