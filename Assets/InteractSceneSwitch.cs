using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractSceneSwitch : MonoBehaviour
{
    public GameObject SwitchToScenceChoice;
    public Boolean openMenu;

    private void Start()
    {
        if (SwitchToScenceChoice != null)
        {
            SwitchToScenceChoice.SetActive(false);
            openMenu = false;
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(openMenu);
            SwitchToScenceChoice.SetActive(true);
            openMenu = true;
        }
    }
    

    public void ButtonYesClicked()
    {
        Debug.Log("YES BUTTON CLICKED");
        openMenu = false;
        SceneManager.LoadScene("testSwitch");
    }
    public void ButtonNoClicked()
    {
        Debug.Log("NO BUTTON CLICKED");
        SwitchToScenceChoice.SetActive(false);
        openMenu = false;
    }
}