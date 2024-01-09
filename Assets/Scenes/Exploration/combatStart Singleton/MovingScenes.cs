using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement; 
public class MovingScenes: MonoBehaviour{
    public static MovingScenes instance; // static instance of the class that can be read by other scripts 
    public GameObject[] enemyList;
    private Vector3 preCombatPosition = Vector3.zero;
    private Quaternion levelDefaultCameraRotation;

    private Vector3 leveldefaultPosition;

    private Boolean combatSuccess;
    private string fromScene;
    private List<string> triggeredCombats;
    // singeleton

     void Awake()
    {
        string sName = SceneManager.GetActiveScene().name;
        
        if (instance == null)
        {
            instance = this;
            resetTriggers();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("instance exists in "+sName);
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
    
    public void setCombatTrigger(string s){
        this.triggeredCombats.Add(s);
    }
    public List<string> getTriggeredCombats(){
        return this.triggeredCombats;
    }

    public void setFromScene(string s){
        this.fromScene = s;
    }
    public string getFromScene(){
        return this.fromScene;
    }

    public void nextLevelStartPos(Vector3 pos){
        this.leveldefaultPosition = pos;
    }

    public void setCameraRotation(Quaternion v){
        this.levelDefaultCameraRotation = v;
    }
    public Quaternion getCameraRotation(){
        return this.levelDefaultCameraRotation;
    }
    public void onLoad(){
        this.preCombatPosition = this.leveldefaultPosition; 
    }
    public void resetTriggers(){
        this.triggeredCombats = new List<string>();
        this.triggeredCombats.Add("----");

    }
}