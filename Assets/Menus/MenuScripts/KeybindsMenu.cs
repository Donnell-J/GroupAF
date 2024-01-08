using UnityEngine;

public class KeybindsMenu : MonoBehaviour
{
    public bool KeybindsMenuIsOpen = false;
    
    public SettingsMenu settingsMenu;
    
    public GameObject KeybindMenuUI;
    public GameObject SettingsMenuUI;
    
    void Start()
    {
        KeybindMenuUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (KeybindsMenuIsOpen)
            {
                exitMenu();
            }
        }
    }
    
    public void exitMenu()
    {
        Debug.Log(KeybindsMenuIsOpen);
        KeybindMenuUI.SetActive(false); ;
        KeybindsMenuIsOpen = false;
    }
    
    public void returnToSettingsMenu()
    {
        Debug.Log("Returning to settings menu...");
        KeybindMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);
        settingsMenu.SettingsMenuIsOpen = true;
        KeybindsMenuIsOpen = false;
    }
}
