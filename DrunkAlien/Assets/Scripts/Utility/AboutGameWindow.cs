using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutGameWindow : MonoBehaviour
{
    private GameObject mainMenu;
    private void Awake()
    {
        mainMenu = GameObject.FindWithTag("MainMenu");
        if (mainMenu == null)
        {
            Debug.LogWarning("There is no main menu");
        }
    }
    private void Start()
    { 
        gameObject.SetActive(false);
    }
    public void MainMenuButtonOnclick()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void PlaySound()
    {
        Managers.Instance.AudioManager.PlaySound(AudioClipName.UISound);
    }
}
