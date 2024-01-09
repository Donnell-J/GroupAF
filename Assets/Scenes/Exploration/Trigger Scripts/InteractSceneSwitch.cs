using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractSceneSwitch : MonoBehaviour
{
    public GameObject startCombatMenu;
    public Transform player;
    public Boolean openMenu;
    public GameObject[] encounterList;
    public int enemyCount;
    public string item;
    public ItemMenu itemMenu;
    
    public string battleName;
    private void Start()
    {
        if (startCombatMenu != null)
        {
            startCombatMenu.SetActive(false); //Hide it's screen overlay until necessary 
            openMenu = false;
        }
        if(MovingScenes.instance.getTriggeredCombats().Last().Equals(name)){
            itemMenu.showItemMenu(item);
        }
        if (MovingScenes.instance.getTriggeredCombats().Contains(name)){ //If this obj has the same name as object thatr previously started combat, destroy it
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(Vector3.Distance(player.position,transform.position) <= 10){
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
        GameObject[] eList = new GameObject[enemyCount];
        for(int i = 0; i < enemyCount; i++){
            eList[i] = encounterList[UnityEngine.Random.Range(0,encounterList.Length)];
        }
        MovingScenes.instance.setEnemyList(eList);
        MovingScenes.instance.setFromScene(SceneManager.GetActiveScene().name);
        MovingScenes.instance.setPreCombatPosition(player.position); //Load relevant data into singleton, switch to battle scene
        MovingScenes.instance.setCombatTrigger(name);
        SceneManager.LoadScene(battleName);
    }
    public void ButtonNoClicked()
    {
        Debug.Log("NO BUTTON CLICKED");
        startCombatMenu.SetActive(false); //Simply hide the menu again
        openMenu = false;
    }
}