using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class BattleController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject[] heroPrefabs;
    public GameObject enemyPrefab;
    [SerializeField]
    public Transform handUI;
    public int partySize;
    public GameObject cardDefault;
    public static List<Hero> party;
    public static int turnIndex;

    public static List<Enemy> enemies;
    public int enemyCount;
    public static bool turnInProgress = false;
    public TMP_Text roundCounter;

    public GameObject deckButton;
    public GameObject discardButton;
    void Start()
    {   
        randomizeTurnOrder();
        party = new List<Hero>();
        enemies = new List<Enemy>();
        populateSides();
        StartCoroutine(Combat());
       
    }
    IEnumerator Combat(){
        int RoundCount=0;
        bool finished = false;
        bool allTeamEmpty;
        while(!finished){
            roundCounter.text = string.Format("Round: {0}",++RoundCount);
            setEnemyIntention();
            allTeamEmpty = true;
            foreach(Hero hero in party){
                if(hero.hand.Count != 0){
                    allTeamEmpty = false;
                }
            }
            foreach(Hero hero in party.ToList()){ 
                hero.toggleActive();
                hero.resolveStatuses();
                hero.reduceStatuses();

                if(allTeamEmpty){
                    drawCards();
                }else{
                    reloadCards();
                }

                deckButton.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format("Deck: {0}",hero.currentDeck.Count);
                discardButton.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format("Discard: {0}",hero.discardPile.Count);


                if(hero.hand.Count ==0){
                    turnInProgress = true;
                }

                yield return new WaitUntil(() => turnInProgress);
                turnInProgress = false;
                
                if(enemies.Count ==0){
                    finished = true;
                    break;
                }
                hero.toggleActive();
                nextTurn();
            }
            if(!finished){
                foreach(Enemy enemy in enemies.ToList()){
                    enemy.resolveStatuses();
                    if(!enemy.isDead){
                        enemy.takeTurn();
                        enemy.reduceStatuses();
                        if(party.Count ==0){
                            finished = true;
                            break;
                        }
                    }
                }
            }
        }
        //CombatOver here;
        if(enemies.Count ==0){
            Debug.Log("Combat won");
            //func for Rewards
        }else{
            Debug.Log("Game over");
        }
    }
    void nextTurn(){
        turnIndex = (turnIndex + 1) % party.Count;
        reloadCards();
    }
    private void populateSides(){
        Vector3 StartPos = new Vector3(-1,1,10);
        Vector3 offset =new Vector3(-11,0,-12);

        for(int i = 1; i < partySize+1;i++){
            GameObject obj = Instantiate<GameObject>(heroPrefabs[i-1],transform.position,transform.rotation);
            obj.transform.position  = StartPos + i*(offset/(partySize+1));
            Hero hScript = obj.GetComponent<Hero>();
            party.Add(hScript);
        }
        for(int i=1;i<enemyCount +1;i++){
            GameObject obj = Instantiate<GameObject>(enemyPrefab,transform.position,transform.rotation);
            obj.transform.position=Vector3.Reflect(StartPos + i*(offset/(enemyCount+1)),Vector3.right);
            Enemy eScript = obj.GetComponent<Enemy>();
            enemies.Add(eScript);
        }
    }

    private void randomizeTurnOrder(){
        for (int i = heroPrefabs.Length - 1; i > 0; i--){
            var k = Random.Range(0,i+1);
            var value = heroPrefabs[k];
            heroPrefabs[k] = heroPrefabs[i];
            heroPrefabs[i] = value;
        }
    }

    private void setEnemyIntention(){
        foreach(Enemy enemy in enemies){
            enemy.setIntention();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void drawCards(){
        for(int i = 0; i <5; i++){
            Card card = Instantiate(cardDefault).GetComponent<Card>();
            card.transform.SetParent(handUI,false);
            card.ID = party[turnIndex].draw();
        }
    }
    void reloadCards(){
        for(int i =0; i < handUI.childCount;i++){
            handUI.GetChild(i).GetComponent<Card>().destroy();
        }
        foreach(int id in party[turnIndex].hand){
            Card card = Instantiate(cardDefault).GetComponent<Card>();
            card.transform.SetParent(handUI,false);
            card.ID = id;
        }
    }
}
