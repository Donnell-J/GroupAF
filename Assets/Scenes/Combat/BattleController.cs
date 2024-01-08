using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
public class BattleController : MonoBehaviour
{
    // 
    [SerializeField]
    public GameObject[] heroPrefabs; //List of hero Prefabs set in editor for instantiating players
    public GameObject enemyPrefab; //For proto: single enemy type, will become List later on;
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
    public GameObject rewardScreen;
    void Start()
    {   
        enemyCount = MovingScenes.instance.getNumberEnemies();
        randomizeTurnOrder(); //randomize turn order for each combat so that it's not too repetetive
        party = new List<Hero>();
        enemies = new List<Enemy>();
        populateSides(); //Instanciate all necessary hero/enemy objects and scripts to populate party and enemies lists
        StartCoroutine(Combat()); //Coroutine so that the function can wait until the player takes their turn for each hero
       
    }

    /*
        The following Combat function is set up to perform the following combat cycle in a sensical manner:

        1. Enemy actions are decided and shown to the player
        2. Determine if this round all heroes will draw cards
        3.Player turn starts, performing the following steps on each hero not already dead
            a. Any statuses that occur before the hero starts their turn (e.g poison,stun) are considered and applied accordingly
            b. All statuses affecting the current hero are reduced by 1
            c. If it's time to draw new cards, draw 5 new cards to create a hand
            d. The player can decide to combine any number of  valid pairs of cards, or not at all
            e. Once the player drags a card onto a valid target, the card is executed and it will end the current hero's turn
            f. If the last card killed the last enemy, combat is ended instead of moving on to the next hero, otherwise, do so
        4. Once all Players have made their turns, it loops through each enemy still in combat performing the following:
            a. Any statuses that occur before the enemy performs their declared action(e.g poison,stun) are considered and applied accordingly
            b. All statuses affecting the current enemy are reduced by 1
            c. The enemy will perform their declared action. If between their declaration and performing said action their target dies or is otherwise removed, it will move to the next hero
            d. If the last hero is killed, combat is ended 
        5. Round count is incremented and it goes back to step 1
    */
    IEnumerator Combat(){
        int RoundCount=0;//Total Round Counter for UI
        bool finished = false; 
        bool allTeamEmpty;
        while(!finished){ 
            roundCounter.text = string.Format("Round: {0}",++RoundCount);
            setEnemyIntention();

            /*Loop through each hero to determine if it's time to draw new cards at the next turn. 
            Heroes that run out of cards sooner due to combining are forced to wait until all other heroes run out of cards too
            */
            allTeamEmpty = true; 
            foreach(Hero hero in party){
                if(hero.hand.Count != 0){
                    allTeamEmpty = false;
                }
            }
            
            foreach(Hero hero in party.ToList()){ 
                hero.toggleActive(); //turn on arrow pointer for hero to show who's turn it is
                hero.resolveStatuses();
                hero.reduceStatuses();

                if(allTeamEmpty){
                    drawCards();//Draws and creates 5 card objects
                }else{
                    reloadCards(); //Set all the cards in the HandArea to the current hand
                }

                deckButton.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format("Deck: {0}",hero.currentDeck.Count); //Show number of cards in deck/discard 
                discardButton.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format("Discard: {0}",hero.discardPile.Count);


                if(hero.hand.Count ==0 | hero.isDead){
                    turnInProgress = true; //turnInProgress controls the current turn, if it's set to true, the current turn is skipped as it's only set true from playing cards
                }

                yield return new WaitUntil(() => turnInProgress);//Wait until a card has been played if turn not skipped for other reasons, setting it to false after this wait will reset the wait condition for the next turn
                turnInProgress = false;
                
                if(enemies.Count ==0){
                    finished = true;
                    break;
                }
                hero.toggleActive(); //Turns off current hero's arrow 
                nextTurn();//Moves to next turn, reloads cards mostly deprecated
            }
            if(!finished){
                foreach(Enemy enemy in enemies.ToList()){
                    enemy.resolveStatuses();
                    if(!enemy.isDead & !enemy.isStunned){
                        enemy.takeTurn();
                    }
                    enemy.isStunned = false;
                    enemy.reduceStatuses();
                        if(party.Count ==0){
                            finished = true;
                            break;
                        }
                }
            }
        }
        //CombatOver here;
        if(enemies.Count ==0){
            Debug.Log("Combat won");
            rewardScreen.SetActive(true);
        }else{
            Debug.Log("Game over");
            SceneManager.LoadScene("GameOver");
        }
    }
    void nextTurn(){
        turnIndex = (turnIndex + 1) % party.Count;
        reloadCards();
    }
    private void populateSides(){
        Vector3 StartPos = new Vector3(-1,1,10); //Position and length of spawning line to dynamically position heroes
        Vector3 offset =new Vector3(-11,0,-12);

        for(int i = 1; i < partySize+1;i++){
            GameObject obj = Instantiate<GameObject>(heroPrefabs[i-1],transform.position,transform.rotation);
            obj.transform.position  = StartPos + i*(offset/(partySize+1));
            Hero hScript = obj.GetComponent<Hero>();
            party.Add(hScript);
        }
        for(int i=1;i<enemyCount +1;i++){
            GameObject obj = Instantiate<GameObject>(enemyPrefab,transform.position,transform.rotation);
            obj.transform.position=Vector3.Reflect(StartPos + i*(offset/(enemyCount+1)),Vector3.right); //Set position to opposite heroes
            Enemy eScript = obj.GetComponent<Enemy>();
            enemies.Add(eScript);
        }
    }

    private void randomizeTurnOrder(){
        for (int i = heroPrefabs.Length - 1; i > 0; i--){ //Fisher yates algorithm to randomize array order
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
            Card card = Instantiate(cardDefault).GetComponent<Card>(); //Create card from prefab and get pointer to script 
            card.transform.SetParent(handUI,false);
            card.ID = party[turnIndex].draw(); //get ID from hero.draw function
        }
    }
    public void reloadCards(){//Destroys all card objects in the hand in the battle scene, and reinstances new cards based on the current hero's hand
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
