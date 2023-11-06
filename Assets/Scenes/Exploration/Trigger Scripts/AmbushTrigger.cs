using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ambushTrigger : MonoBehaviour
{
    public GameObject AmbushText;
    public Transform player;
    public int numEnemies;
    private void Start()
    {
        AmbushText.SetActive(false);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2); //Waits for 2 seconds so player can read ambush text
        AmbushText.SetActive(false);
        MovingScenes.instance.setNumberEnemies(numEnemies); // load data to singleton and move to battle scene 
        MovingScenes.instance.setPreCombatPosition(player.position);
        SceneManager.LoadScene("BattleScene");
    }

    void OnMouseDown(){
        if (Input.GetMouseButtonDown(0))
        {
            if(Vector3.Distance(player.position,transform.position) <= 30){//Start countdown if clicked on and player in range
                AmbushText.SetActive(true); 
                StartCoroutine(Wait());
            }
        }
    }
}
