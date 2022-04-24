using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : MonoBehaviour
{
    private GameObject aboutMenu;
    private GameObject creditsMenu;
    private void Awake()
    {
        Time.timeScale = 1;

        aboutMenu = GameObject.FindWithTag("AboutMenu");
        creditsMenu = GameObject.FindWithTag("Credits");

        if (aboutMenu == null)
        {
            Debug.LogWarning("There is no about menu");
        }        
        
        if (creditsMenu == null)
        {
            Debug.LogWarning("There is no credits");
        }
    }
    public void PlayButtonOnclick()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void AboutGameButtonOnclick()
    {
        gameObject.SetActive(false);
        aboutMenu.SetActive(true);
    }
    public void CreditsButtonOnclick()
    {
        gameObject.SetActive(false);
        creditsMenu.SetActive(true);
    }
    public void PlaySound()
    {
        Managers.Instance.AudioManager.PlaySound(AudioClipName.UISound);
    }
}
