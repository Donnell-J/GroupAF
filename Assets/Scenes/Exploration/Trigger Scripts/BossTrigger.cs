using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BossTrigger : MonoBehaviour
{
    public GameObject startCombatMenu;
    public Transform player;
    public Boolean openMenu;
    public GameObject[] encounterList;
    
    public string nextScene;
    public Vector3 nextPos;
    public Vector3 camRotation;

    public Button yesButton;
    public TMP_Text yesButtonText;
    
    private void Start()
    {
        if (startCombatMenu != null)
        {
            startCombatMenu.SetActive(false); //Hide it's screen overlay until necessary 
            openMenu = false;
        }
        Debug.Log(MovingScenes.instance == null);
        if (MovingScenes.instance.getTriggeredCombats().Contains(name)){ //If this obj has the same name as object thatr previously started combat, destroy it
            MovingScenes.instance.nextLevelStartPos(nextPos);
            MovingScenes.instance.setCameraRotation(Quaternion.Euler(camRotation));
            cardDB.instance.nextLevelSave();
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
                yesButtonText.text = cardDB.instance.keyCount.ToString()+"/3";
                if(cardDB.instance.keyCount < 3){
                    yesButton.enabled = false;
                } else{
                    yesButton.enabled = true;
                }
            }
        }
    }
    

    public void ButtonYesClicked()
    {
        Debug.Log("YES BUTTON CLICKED");
        openMenu = false;
        MovingScenes.instance.setEnemyList(encounterList);
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