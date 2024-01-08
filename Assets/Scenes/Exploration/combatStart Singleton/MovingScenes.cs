using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement; 
public class MovingScenes: MonoBehaviour{
    public static MovingScenes instance; // static instance of the class that can be read by other scripts 
    public GameObject[] enemyList;
    private Vector3 preCombatPosition;
    private Quaternion preCombatCameraRotation;
    private Boolean combatSuccess;
    private Scene fromScene;
    private List<string> triggeredCombats;
    public GameObject player;
    // singeleton

     void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //triggeredCombats = new List<string>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //getter for number of enemies
    public GameObject[] getEnemyList()
    {
        return this.enemyList;
    }
    //setter for number of enemies
    public void setEnemyList(GameObject[] enemies)
    {
        this.enemyList = enemies;
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

    public void setFromScene(Scene s){
        this.fromScene = s;
    }
    public Scene getFromScene(){
        return this.fromScene;
    }

    public void nextLevelStartPos(Vector3 pos){
        this.preCombatPosition = pos;
    }

    public void setCameraRotation(Quaternion v){
        this.preCombatCameraRotation = v;
    }
    public Quaternion getCameraRotation(){
        return this.preCombatCameraRotation;
    }
}