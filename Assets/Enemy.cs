using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour, IcombatFunction{
    
    public StatusCounter statusCounter;
    public HPBar hpBar;

    public TMP_Text intentText;
    
    public GameObject statusBar;
    [SerializeField]
    public int maxHP = 60;
    [SerializeField]
    private int currentHP;

    public int shield = 0;
    public Dictionary<string,int> statuses = new Dictionary<string, int>();
    private string[] canApply = new string[]{"weakened","vulnerable"};
    int intent;
    int targetInd;

    string intentValue;
//intent who for what 
    // Start is called before the first frame update
    void Awake(){
        currentHP = maxHP;
        hpBar.setMax(maxHP);
    }
    void start(){

    }
    public void setIntention(){
        Debug.Log("YOWZA");
        int choiceVal = Random.Range(0,100);
        targetInd = Random.Range(0,BattleController.party.Count);
        if(choiceVal <=50){
            intent = 0; // attack
            intentValue = Random.Range(6,11).ToString();
            intentText.text = string.Format("Attacking {0} for {1}",BattleController.party[targetInd].name, intentValue);
        }else if(choiceVal <=80){
            intent = 1; //block
            intentValue = Random.Range(6,11).ToString();
            intentText.text = string.Format("Blocking for {0}", intentValue);
        }else{
            intent = 2;//status effect
            intentValue = canApply[Random.Range(0,2)];
            intentText.text = string.Format("Applying {0} on {1}",intentValue,BattleController.party[targetInd].name);
        }
    }

    public void takeTurn(){
        if(intent == 0){
            int dmg = statuses.ContainsKey("weakened")? int.Parse(intentValue)/2 : int.Parse(intentValue);
            dmg = statuses.ContainsKey("weakened")? dmg*2 : dmg;
            BattleController.party[targetInd].getHit(statuses.ContainsKey("weakened")? int.Parse(intentValue)/2 : int.Parse(intentValue));
        }else if(intent == 1){
            defend(int.Parse(intentValue));
        }else if(intent == 2){
            BattleController.party[targetInd].applyStatus(intentValue,2);
        }
    }
    public void applyStatus(string status, int amount){
        try{
            statuses[status] += amount;
            StatusCounter s = statusBar.transform.Find(status).GetComponent<StatusCounter>();
            s.updateCount(amount);
        }catch(KeyNotFoundException){
            statuses.Add(status,amount);
            StatusCounter s = Instantiate(statusCounter).GetComponent<StatusCounter>();
            s.transform.SetParent(statusBar.transform,false);
            s.statusType = status;
            s.updateCount(amount);
        }
        if (status == "weakened"){
            intentText.text = string.Format("Attacking {0} for {1}",BattleController.party[targetInd].name,int.Parse(intentValue)/2);
        }
    }
    public void reduceStatuses(){
        foreach(string key in statuses.Keys){
            statuses[key] -=1;
            if(statuses[key] ==0){
                statuses.Remove(key);
            }
        }
        for(int i=0; i<statusBar.transform.childCount; i++){
            StatusCounter s = statusBar.transform.GetChild(i).GetComponent<StatusCounter>();
            s.updateCount(-1);
        }
    }
    public void heal(int amount){
        currentHP = Mathf.Clamp(currentHP+amount,0,maxHP);
        hpBar.setHealth(currentHP);
    }
    public void getHit(int amount){
        shield -= (statuses.ContainsKey("vulnerable")) ? amount*2 : amount;
        if(shield < 0){
            currentHP +=shield;
            shield = 0;
            hpBar.setShield(0);
        }
        hpBar.setHealth(currentHP);
    }

    public void defend(int amount){
        shield += amount;
        hpBar.setShield(shield);
    }
    
}

