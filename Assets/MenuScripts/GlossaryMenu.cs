using System;
using UnityEngine;

public class GlossaryMenu : MonoBehaviour
{
    public bool GlossaryMenuIsOpen = false;
    
    public SettingsMenu settingsMenu;
    
    public GameObject GlossaryMenuUI;
    public GameObject SettingsMenuUI;
    
    void Start()
    {
        GlossaryMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GlossaryMenuIsOpen)
            {
                exitMenu();
            }
        }
    }
    
    public void exitMenu()
    {
        Debug.Log("Exiting glossary menu...");
        Debug.Log(GlossaryMenuIsOpen);
        GlossaryMenuUI.SetActive(false); ;
        GlossaryMenuIsOpen = false;
    }
    
    public void ReturnToSettingsMenu()
    {
        Debug.Log(GlossaryMenuIsOpen);
        Debug.Log("Returning to settings menu...");
        GlossaryMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);
        settingsMenu.SettingsMenuIsOpen = true;
        GlossaryMenuIsOpen = false;
    }
}
