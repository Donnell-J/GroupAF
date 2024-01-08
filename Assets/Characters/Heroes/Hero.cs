using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
public class Hero : MonoBehaviour, IcombatFunction
{
    // Start is called before the first frame update
    public StatusCounter statusCounter;
    public HPBar hpBar;

    public Animator animPlayer;
    public VisualEffect vfxPlayer;
    public GameObject statusBar;
    [SerializeField]
    public int maxHP = 60;
    [SerializeField]
    private int currentHP;
    public bool isDead = false;
    [SerializeField]
    private int heroID; //0 = Melee Hero, 1= RangeHero, 2=AOE Hero, 3=SuppHero
    [SerializeField]
    public new string name;
    public List<int> currentDeck;
    public List<int> hand = new List<int>();
    public List<int> discardPile = new List<int>();
    
    public Dictionary<string,int> statuses = new Dictionary<string, int>();
    private int shield = 0;
    public GameObject currentArrow; 
       
    void Awake(){
        List<int> d = cardDB.instance.heroDecks[heroID]; //Get deck from Card DB singleton
        currentDeck = d;
        maxHP = cardDB.instance.heroMaxHPs[heroID]; ///get maxHP from static variable in cardDB singleton so that it persists between instances
        currentHP= maxHP;
        hpBar.setMax(maxHP);
        shuffleDeck();
        currentArrow.SetActive(false);

    }
    void Start(){
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void toggleActive(){
        currentArrow.SetActive(!currentArrow.activeSelf);
    }
    public int draw(){
        if(currentDeck.Count == 0){
            currentDeck = discardPile;
            discardPile = new List<int>();
            shuffleDeck();
        } 
        int drawnID =this.currentDeck[0];
        this.hand.Add(drawnID);
        this.currentDeck.RemoveAt(0);
        return drawnID;
    }
    public void discard(int id){
        this.hand.Remove(id);
        this.discardPile.Add(id);
    }

    private void shuffleDeck(){
        for (int i = currentDeck.Count - 1; i > 0; i--){
            var k = Random.Range(0,i+1);
            var value = currentDeck[k];
            currentDeck[k] = currentDeck[i];
            currentDeck[i] = value;
        }
    }

    public void applyStatus(string status, int amount, bool isGood){
        vfxPlayer.Reinit();
        if(isGood){
            vfxPlayer.SendEvent("OnBuff");
        } else{
            vfxPlayer.SendEvent("OnDebuff");
        }
        try{//if the hero already has status applied, increase the amount of it, otherwise add the status to it's statuses dictionary. update/Create corresponding StatusCounter
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
    }
    public void resolveStatuses(){
        if(statuses.ContainsKey("poison")){//Poison deals dmg based on the number of stacks
            getHit(statuses["poison"],true,"poison");
        }
        if(!statuses.ContainsKey("barricade")){//barricade stops shield from being reset
            shield = 0;
            hpBar.setShield(0);
        }
        if(statuses.ContainsKey("stun")){ //stun skips turn
            BattleController.turnInProgress = true;
        }
    }
    public void reduceStatuses(){ //Goes through each status in dictionary and decreases it's amonut by 1, and shows this on status counters. If amount = 0, it removes it from the dictionary
        List<string> keys = new List<string>(statuses.Keys);
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
    public void heal(int amount){//increase hp by amount, can't go above max hp
        vfxPlayer.Reinit();
        vfxPlayer.SendEvent("OnHeal");


        currentHP = Mathf.Clamp(currentHP+amount,0,maxHP);
        hpBar.setHealth(currentHP);
    }
   public void getHit(int amount, bool ignoreShield, string dmgType){//get hit for amount dmg. If the dmg source ignores shields it will deal directly to hp, weithout going through shield
        vfxPlayer.Reinit();
        vfxPlayer.SendEvent("OnGetHit");
        animPlayer.SetTrigger("playGetHit");

        if(ignoreShield){
            currentHP -= amount;
        }else{
            shield -= (statuses.ContainsKey("vulnerable")) ? amount*2 : amount; //vulnerable targets take double dmg
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

    public void defend(int amount){
        vfxPlayer.Reinit();
        vfxPlayer.SendEvent("OnDefend");

        shield += amount;
        hpBar.setShield(shield);
    }
    public void die(){
        BattleController.party.Remove(this);
        animPlayer.SetTrigger("playDead");
        transform.GetChild(0).gameObject.SetActive(false);
        isDead = true;
    }

    public void forceAction(int action){
        return;
    }
    public void removeStatuses(){
        List<string> keys = new List<string>(statuses.Keys);
        foreach(string key in keys){
            if(key.Equals("poison")|key.Equals("weakened")|key.Equals("vulnerable")){
                for(int i=0; i<statusBar.transform.childCount; i++){
                    StatusCounter s = statusBar.transform.GetChild(i).GetComponent<StatusCounter>();
                    if(s.name == key){
                        s.updateCount(-(statuses[key]));
                        break;
                    }
                }
                statuses.Remove(key);
            }
        }
        

    }
}
