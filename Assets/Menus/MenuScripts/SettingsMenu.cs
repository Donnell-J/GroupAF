using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public bool SettingsMenuIsOpen = false;
    
    public KeybindsMenu keybindsMenu;
    public GlossaryMenu glossaryMenu;
    
    public GameObject SettingsMenuUI;
    public GameObject PauseMenuUI;
    public GameObject KeybindsMenuUI;
    public GameObject GlossaryMenuUI;
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenuUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingsMenuIsOpen)
            {
                returnToPauseMenu();
            }
        }
    }
    
    public void returnToPauseMenu()
    {
        Debug.Log(SettingsMenuIsOpen);
        SettingsMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        SettingsMenuIsOpen = false;
    }
    
    public void LoadKeybinds()
    {
        Debug.Log("Loading keybinds...");
        SettingsMenuUI.SetActive(false);
        KeybindsMenuUI.SetActive(true);
        SettingsMenuIsOpen = false;
        keybindsMenu.KeybindsMenuIsOpen = true;
    }
    
    public void LoadGlossary()
    {
        Debug.Log("Loading glossary...");
        SettingsMenuUI.SetActive(false);
        GlossaryMenuUI.SetActive(true);
        SettingsMenuIsOpen = false;
        glossaryMenu.GlossaryMenuIsOpen = true;
    }

    public void setDifficulty(int level){
        cardDB.instance.difficulty = level;
    }
}
