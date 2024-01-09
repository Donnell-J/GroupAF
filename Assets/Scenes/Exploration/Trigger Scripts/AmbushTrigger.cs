using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ambushTrigger : MonoBehaviour
{
    public GameObject AmbushText;
    public Transform player;
    public GameObject[] encounterList;
    public int enemyCount;
    public string item;

    public ItemMenu itemMenu;

    public string battleName;
    private void Start()
    {
        AmbushText.SetActive(false);
        Debug.Log(MovingScenes.instance.getTriggeredCombats().Last());
        Debug.Log(name);
        if(MovingScenes.instance.getTriggeredCombats().Last().Equals(name)){
            itemMenu.showItemMenu(item);
            cardDB.instance.keyCount+=1;
        }
        if (MovingScenes.instance.getTriggeredCombats().Contains(name)){ //If this obj has the same name as object thatr previously started combat, destroy it
            Destroy(gameObject);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2); //Waits for 2 seconds so player can read ambush text
        AmbushText.SetActive(false);
        GameObject[] eList = new GameObject[enemyCount];
        for(int i = 0; i < enemyCount; i++){
            eList[i] = encounterList[UnityEngine.Random.Range(0,encounterList.Length)];
        }
        MovingScenes.instance.setEnemyList(eList); // load data to singleton and move to battle scene 
        MovingScenes.instance.setPreCombatPosition(player.position);
        MovingScenes.instance.setFromScene(SceneManager.GetActiveScene().name);
        MovingScenes.instance.setCombatTrigger(name);
        SceneManager.LoadScene(battleName);
    }

    void OnMouseDown(){
        Debug.Log("herer");
        if(Vector3.Distance(player.position,transform.position) <= 10){//Start countdown if clicked on and player in range
            AmbushText.SetActive(true); 
            StartCoroutine(Wait());
        }
    }
}
