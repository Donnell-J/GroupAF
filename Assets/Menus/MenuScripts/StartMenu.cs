using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;

    public GameObject StartMenuUI;
    public GameObject SettingsMenuUI;
    // Start is called before the first frame update
    void Start()
    {
        StartMenuUI.SetActive(true);
    }

    public void LoadSettings()
    {
        StartMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);
        settingsMenu.SettingsMenuIsOpen = true;
    }
    
    public void StartGame()
    {
        StartMenuUI.SetActive(false);
        SceneManager.LoadScene("Scenes/Exploration/Levels/Intro");
    }
}

