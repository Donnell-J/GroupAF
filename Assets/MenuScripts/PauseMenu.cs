using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    
    public SettingsMenu settingsMenu;
    
    public GameObject PauseMenuUI;
    public GameObject SettingsMenuUI;
    
    void Start()
    {
        PauseMenuUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    
    public void LoadSettings()
    {
        Debug.Log("Loading settings...");
        PauseMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);
        settingsMenu.SettingsMenuIsOpen = true;
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
