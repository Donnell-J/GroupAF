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
        MovingScenes.instance.nextLevelStartPos(new Vector3(32f,1.6f,8.5f));
        MovingScenes.instance.setCameraRotation( Quaternion.Euler(0f,-75f,0f));
        SceneManager.LoadScene("Scenes/Exploration/Levels/Level2");
    }
}
