using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, IcombatFunction
{
    // Start is called before the first frame update
    public StatusCounter statusCounter;
    public HPBar hpBar;
    public static int[] maxHPs = new int[]{60,60,55,55}; 
    public GameObject statusBar;
    [SerializeField]
    public int maxHP = 60;
    [SerializeField]
    private int currentHP;
    [SerializeField]
    private int heroID; //0 = Melee Hero, 1= RangeHero, 2=AOE Hero, 3=SuppHero
    [SerializeField]
    public new string name;
    private List<int> currentDeck;
    private List<int> hand = new List<int>();
    private List<int> discardPile = new List<int>();
    
    public Dictionary<string,int> statuses = new Dictionary<string, int>();
    private int shield = 0;
       
    void Awake(){
        List<int> d = cardDB.instance.heroDecks[heroID];
        currentDeck = d;
        maxHP = maxHPs[heroID];
        currentHP= maxHP;
        hpBar.setMax(maxHP);
        shuffleDeck();

    }
    void Start(){
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public int draw(){
        int drawnID =this.currentDeck[0];
        this.hand.Add(drawnID);
        this.currentDeck.RemoveAt(0);
        return drawnID;
    }
    public void discard(int id){
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
