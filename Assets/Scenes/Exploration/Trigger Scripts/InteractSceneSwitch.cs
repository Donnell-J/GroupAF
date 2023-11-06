using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractSceneSwitch : MonoBehaviour
{
    public GameObject startCombatMenu;
    public Transform player;
    public Boolean openMenu;
    public int numEnemies;
    
    private void Start()
    {
        if (startCombatMenu != null)
        {
            startCombatMenu.SetActive(false); //Hide it's screen overlay until necessary 
            openMenu = false;
        }
        if (name.Equals(MovingScenes.instance.getCombatTrigger())){ //If this obj has the same name as object thatr previously started combat, destroy it
            Destroy(this);
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(Vector3.Distance(player.position,transform.position) <= 30){
                Debug.Log(openMenu);
                startCombatMenu.SetActive(true);//Show overlay if clicked on and player is close enough
                openMenu = true;
            }
        }
    }
    

    public void ButtonYesClicked()
    {
        Debug.Log("YES BUTTON CLICKED");
        openMenu = false;
        MovingScenes.instance.setNumberEnemies(numEnemies);
        MovingScenes.instance.setPreCombatPosition(player.position); //Load relevant data into singleton, switch to battle scene
        MovingScenes.instance.setCombatTrigger(name);
        SceneManager.LoadScene("BattleScene");
    }
    public void ButtonNoClicked()
    {
        Debug.Log("NO BUTTON CLICKED");
        startCombatMenu.SetActive(false); //Simply hide the menu again
        openMenu = false;
    }
}