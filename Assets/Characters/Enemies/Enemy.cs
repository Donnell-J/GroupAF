using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;
public class Enemy : MonoBehaviour, IcombatFunction{

    //Following the 
    public StatusCounter statusCounter;
    public HPBar hpBar;

    public VisualEffect vfxPlayer;
    public Animator animPlayer;

    public IntentionIcon intentionIcon;
    
    public GameObject statusBar;

    public string Name; 
    public TMP_Text nameBar;
    

    //  List of ALL possible actions
    enum  Action{
        attack,
        block ,
        debuff ,
        buff ,
        attackBlock,
        attackDebuff,
        teamAttack,
        heal
    }
      //  If an enemy is not meant to use an action, weight is 0. Sum of base weights 100 
    
    [SerializeField]
    public List<int> baseActionWeights;
    private int[] actionWeights;
    [SerializeField]
    public List<int> actionStrengths;
    public List<string> debuffsCanApply;// what statuses the enemy can apply, each with equal probability
    public List<string> buffsCanApply;// what buffs the enemy can give to themselves/other enemies
    //Enemies 
    public int personalityType;
    private string[,] personalityKeywords = {{"Threatening","Combative","Destructive","Scrappy","Angry"},
                                             {"Cautious","Wary","Alert","Warding","Defensive"},
                                             {"Sapping","Debilitating","Draining","Pernicious","Malicious"}};

    //Determines base modifier towards actions. 
    private float[][] baseWeightModifiers  = new float[][]{
                                                            new float[]{1.3f, 0.7f, 0.7f, 1.1f, 1.1f, 1.0f, 1.1f, 0.6f},
                                                            new float[]{0.7f, 1.3f, 1.1f, 1.0f, 1.1f, 1.0f, 0.9f, 1.0f},
                                                            new float[]{0.9f, 0.9f, 1.3f, 1.3f, 0.8f, 1.1f, 1.0f, 1.2f}};
    private float[] weightModifiers;
    
    
    //HP AND STATS
    [SerializeField]
    public int maxHP = 60; //Amount of hp the enemy starts with

    public float[] difficultyHPMulti = new float[]{0.5f,1f,1.75f};
    [SerializeField]
    private int currentHP;
    public List<string> dmgResist; //damage of this type is halved against the enemy
    public List<string> dmgWeakness; //damage of this type is doubled against the enemy

    

    public int shield = 0;
    public Dictionary<string,int> statuses = new Dictionary<string, int>();
    public bool isDead, isStunned;
    Action intent;
    int targetInd;

    int intentValue;


    void Awake(){
        maxHP += Random.Range(-(maxHP/10),(maxHP/10));
        maxHP = Mathf.RoundToInt(maxHP * difficultyHPMulti[cardDB.instance.difficulty]);
        currentHP = maxHP;
        hpBar.setMax(maxHP);
        personalityType = Random.Range(0,3);
        nameBar.text = personalityKeywords[personalityType,Random.Range(0,5)] +" "+ Name ;
        actionWeights = new int[baseActionWeights.Count];
        
    }
    void start(){

    }
    public void setIntention(){
        targetInd = Random.Range(0,BattleController.party.Count);
        
        weightModifiers = baseWeightModifiers[personalityType];
        
        if (currentHP < maxHP/3){
            weightModifiers[1] +=0.2f;
        }
        if(statuses.ContainsKey("strengthen")){
            weightModifiers[0] +=0.2f;
            weightModifiers[4] +=0.2f;
            weightModifiers[5] +=0.2f;
            weightModifiers[6] +=0.2f;
        }

        for(int i = 0; i<baseActionWeights.Count; i++){
            actionWeights[i] = Mathf.RoundToInt(baseActionWeights[i] * weightModifiers[i]);
        }
        
        int total = 0;
        foreach(int x in actionWeights){
            total +=x;
            
        }


        int choiceVal = Random.Range(0,total); //Random number from 0-99 to decide action with % weightings
        int pointer = 0;
        for(int i = 0; i < actionWeights.Length; i++){
            pointer += actionWeights[i];
            if (choiceVal < pointer){
                intent = (Action)i;
                break;
            }
        }
        intentValue = Random.Range(actionStrengths[(int)intent], Mathf.RoundToInt(actionStrengths[(int)intent] * 1.5f));
        if(intent == Action.buff){ 
            targetInd = Random.Range(0,BattleController.enemies.Count);

        }else if(intent == Action.heal){
            targetInd = Random.Range(0,BattleController.enemies.Count);
        }
        
        intentionIcon.setIntentIcon((int) intent, intentValue, findTargetName());
    }
    

    public void forceAction(int action){
        intent = (Action) action;
        intentValue = Random.Range(actionStrengths[(int)intent], Mathf.RoundToInt(actionStrengths[(int)intent] * 1.5f));
        intentionIcon.setIntentIcon((int) intent, intentValue, findTargetName());
    }    

    private string findTargetName(){
        int targetGroup = 0; //0: player, 1: self, 2:other enemy
             
        if(intent == Action.block){ 
            targetGroup = 1;

        }else if(intent == Action.buff){ 
            targetInd = Random.Range(0,BattleController.enemies.Count);
            targetGroup = 2;

        }else if(intent == Action.heal){
            targetInd = Random.Range(0,BattleController.enemies.Count);
            targetGroup = 2;
        }

        string targetName = ""; 
        if(targetGroup ==0){
            targetName = BattleController.party[targetInd].name;
        } else if(targetGroup ==1){
            targetName = "self";
        }else if(targetGroup ==2){
            if (BattleController.enemies[targetInd] == this){
                targetName = "self";
            } else{
                targetName = (targetInd+1).ToString();
            }
        }
        return targetName;
    }

    public void takeTurn(){
        targetInd = targetInd % BattleController.party.Count; //If the enemy's target has died before its turn, it will target the next hero, loops back to first
        if(intent == Action.attack){
            animPlayer.SetTrigger("playAttack");
            attack(targetInd);

        }else if(intent == Action.block){
            animPlayer.SetTrigger("playBlock");
            defend(intentValue);
        
        }else if(intent == Action.debuff){
            animPlayer.SetTrigger("playCast");
            BattleController.party[targetInd].applyStatus(debuffsCanApply[Random.Range(0,debuffsCanApply.Count)],intentValue, false);
        
        }else if(intent == Action.buff){ 
            animPlayer.SetTrigger("playCast");
            targetInd = targetInd % BattleController.enemies.Count;
            BattleController.enemies[targetInd].applyStatus(buffsCanApply[Random.Range(0,buffsCanApply.Count)],intentValue, true);

        }else if(intent == Action.attackBlock){ 
            animPlayer.SetTrigger("playAttack");
            attack(targetInd);
            defend(intentValue);

        }else if(intent == Action.attackDebuff){ 
            animPlayer.SetTrigger("playAttack");
            attack(targetInd);
            BattleController.party[targetInd].applyStatus(debuffsCanApply[Random.Range(0,debuffsCanApply.Count)],Random.Range(1,4), false);
        
        }else if(intent == Action.teamAttack){
            animPlayer.SetTrigger("playAttack");
            for(int i = 0; i< BattleController.party.Count; i++){
                attack(i);
            }

        }else{ //Heal
            animPlayer.SetTrigger("playCast");
            targetInd = targetInd % BattleController.enemies.Count;
            BattleController.enemies[targetInd].heal(intentValue);
        }
    }

    public void attack(int targetInd){
        int dmg = statuses.ContainsKey("weakened")? intentValue/2 : intentValue; //if weakened deal 1/2 dmg
        dmg = statuses.ContainsKey("strengthen")? dmg*2 : dmg;//if strengthen deal *2 dmg
        BattleController.party[targetInd].getHit(statuses.ContainsKey("weakened")? intentValue/2 : intentValue,false, "none"); //hit target hero
    }

    public void applyStatus(string status, int amount, bool isGood){
        vfxPlayer.Reinit();
        if(isGood){
            vfxPlayer.SendEvent("OnBuff");
        }else{
            vfxPlayer.SendEvent("OnDebuff");
        }
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
            intentionIcon.updateDamageValue(intentValue/2);
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
            getHit(statuses["poison"],true,"poison");
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
        vfxPlayer.Reinit();
        vfxPlayer.SendEvent("OnHeal");
        currentHP = Mathf.Clamp(currentHP+amount,0,maxHP);
        hpBar.setHealth(currentHP);
    }
    public void getHit(int amount, bool ignoreShield, string dmgType){//get hit for amount dmg. If the dmg source ignores shields it will deal directly to hp, weithout going through shield
        vfxPlayer.Reinit();
        vfxPlayer.SendEvent("OnGetHit");
        animPlayer.SetTrigger("playGetHit");
        
        amount = dmgWeakness.Contains(dmgType) ? (int) (amount*1.5f) : amount;
        amount = dmgResist.Contains(dmgType) ? (int) (amount * 0.5f) : amount;
        if(ignoreShield){
            currentHP -= amount;
        }else{
            shield -= (statuses.ContainsKey("vulnerable")) ? amount*2 : amount;//vulnerable targets take double dmg
            if(shield < 0){
                currentHP +=shield;
                shield = 0;
            hpBar.setShield(shield);
        }
        }
        if(currentHP <= 0){
            currentHP=0;
            die();
        }
        hpBar.setHealth(currentHP);
    }

    public void defend(int amount){//increase shield by amount
        vfxPlayer.Reinit();
        vfxPlayer.SendEvent("OnDefend");

        shield += amount;
        hpBar.setShield(shield);
    }
    public void die(){//removes enemy from list of active enemies and rotates it to show its dead
        animPlayer.SetTrigger("playDead");
        BattleController.enemies.Remove(this);
        transform.GetChild(0).gameObject.SetActive(false);
        isDead = true;
    }

    public void removeNegativeStatuses(){
        return;
    }
}

