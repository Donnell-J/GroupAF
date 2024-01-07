using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MovingScenes: MonoBehaviour{
    public static MovingScenes instance; // static instance of the class that can be read by other scripts 
    private int numberEnemies =3;
    private Vector3 preCombatPosition = new Vector3(760,80,746);
    private Boolean combatSuccess;
    private List<string> triggeredCombats;
    public GameObject player;
    // singeleton

     void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //getter for number of enemies
    public int getNumberEnemies()
    {
        return numberEnemies;
    }
    //setter for number of enemies
    public void setNumberEnemies(int numberEnemies)
    {
        this.numberEnemies = numberEnemies;
    }
    //getter for precombat position
    public Vector3 getPreCombatPosition()
    {
        return preCombatPosition;
    }
    //setter for precombat position
    public void setPreCombatPosition(Vector3 preCombatPosition)
    {
        this.preCombatPosition = preCombatPosition;
    }
    //getter for combat success
    public Boolean getCombatSuccess()
    {
        return combatSuccess;
    }
    //setter for combat success
    public void setCombatSuccess(Boolean combatSuccess)
    {
        this.combatSuccess = combatSuccess;
    }
    
    public void setCombatTrigger(string name){
        this.triggeredCombats.Add(name);
    }
    public List<string> getTriggeredCombats(){
        return this.triggeredCombats;
    }

}