using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstText : MonoBehaviour
{
    public GameObject FirstTextUI;
    public GameObject SecondTextUI;
    public GameObject ThirdTextUI;
    
    //lasts 5 seconds and then hides firstTextUI
    void Start()
    {
        FirstTextUI.SetActive(true);
        SecondTextUI.SetActive(false);
        ThirdTextUI.SetActive(false);
        Invoke("HideFirstText", 5f);
    }
    void HideFirstText()
    {
        FirstTextUI.SetActive(false);
        ShowSecondText();
    }
    
    void ShowSecondText()
    {
        SecondTextUI.SetActive(true);
        Invoke("HideSecondText", 5f);
    }
    
    void HideSecondText()
    {
        SecondTextUI.SetActive(false);
        ShowThirdText();
    }
    
    void ShowThirdText()
    {
        ThirdTextUI.SetActive(true);
        Invoke("HideThirdText", 5f);
    }
    
    void HideThirdText()
    {
        ThirdTextUI.SetActive(false);
        SceneManager.LoadScene("Scenes/Levels/Level2");
    }
}
