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
    public bool isDead, isStunned;
    private string[] canApply = new string[]{"weakened","vulnerable"};// what statuses the enemy can apply
    int intent;
    int targetInd;

    string intentValue;
    void Awake(){
        currentHP = maxHP;
        hpBar.setMax(maxHP);
    }
    void start(){

    }
    public void setIntention(){
        int choiceVal = Random.Range(0,100); //Random number from 0-99 to decide action with % weightings
        targetInd = Random.Range(0,BattleController.party.Count);
        if(choiceVal <50){ //50% chance to attack
            intent = 0; 
            intentValue = Random.Range(6,11).ToString();
            intentText.text = string.Format("Attacking {0} for {1}",BattleController.party[targetInd].name, intentValue);
        }else if(choiceVal <80){//30% chance to block
            intent = 1; 
            intentValue = Random.Range(6,11).ToString();
            intentText.text = string.Format("Blocking for {0}", intentValue);
        }else{//20% chance to apply a debuff with equal chance of each
            intent = 2;
            intentValue = canApply[Random.Range(0,2)];
            intentText.text = string.Format("Applying {0} on {1}",intentValue,BattleController.party[targetInd].name);
        }
    }

    public void takeTurn(){
        targetInd = targetInd % BattleController.party.Count; //If the enemy's target has died before it's turn, it will target the next hero, loops back to first
        if(intent == 0){
            int dmg = statuses.ContainsKey("weakened")? int.Parse(intentValue)/2 : int.Parse(intentValue); //if weakened deal 1/2 dmg
            dmg = statuses.ContainsKey("strengthen")? dmg*2 : dmg;//if strengthen deal *2 dmg
            BattleController.party[targetInd].getHit(statuses.ContainsKey("weakened")? int.Parse(intentValue)/2 : int.Parse(intentValue),false); //hit target hero
        }else if(intent == 1){
            defend(int.Parse(intentValue));
        }else if(intent == 2){
            BattleController.party[targetInd].applyStatus(intentValue,2);
        }
    }
    public void applyStatus(string status, int amount){
        try{//if the enemy already has status applied, increase the amount of it, otherwise add the status to it's statuses dictionary. update/Create corresponding StatusCounters
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
        if (status == "weakened" & intent ==0){
            intentText.text = string.Format("Attacking {0} for {1}",BattleController.party[targetInd].name,int.Parse(intentValue)/2);
        }
    }
    public void reduceStatuses(){
        List<string> keys = new List<string>(statuses.Keys); //Loop through each status currently effecting this enemy, reduce it by 1 and reflect change in status counters
        foreach(string key in keys){
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
    public void resolveStatuses(){
        if(statuses.ContainsKey("poison")){//Poison deals dmg based on the number of stacks
            getHit(statuses["poison"],true);
        }
        if(!statuses.ContainsKey("barricade")){//barricade stops shield from being reset
            shield = 0;
            hpBar.setShield(0);
        }
        if(statuses.ContainsKey("stun")){//stun skips turn
            isStunned = true;
        }
    }
    public void heal(int amount){//increase HP by amount, without exceeding max hp
        currentHP = Mathf.Clamp(currentHP+amount,0,maxHP);
        hpBar.setHealth(currentHP);
    }
    public void getHit(int amount, bool ignoreShield){//get hit for amount dmg. If the dmg source ignores shields it will deal directly to hp, weithout going through shield
        if(ignoreShield){
            currentHP -= amount;
        }else{
            shield -= (statuses.ContainsKey("vulnerable")) ? amount*2 : amount;//vulnerable targets take double dmg
            if(shield < 0){
                currentHP +=shield;
                shield = 0;
                hpBar.setShield(0);
        }
        }
        if(currentHP <= 0){
            currentHP=0;
            die();
        }
        hpBar.setHealth(currentHP);
    }

    public void defend(int amount){//increase shield by amount
        shield += amount;
        hpBar.setShield(shield);
    }
    public void die(){//removes enemy from list of active enemies and rotates it to show its dead
        BattleController.enemies.Remove(this);
        transform.eulerAngles = new Vector3(0,0,90);
        transform.GetChild(0).gameObject.SetActive(false);
        isDead = true;
    }
}

