using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject[] heroPrefabs;
    public GameObject enemyPrefab;
    public int partySize;
    public GameObject cardDefault;
    public static List<Hero> party;
    public static int turnIndex =0;

    public static List<Enemy> enemies;
    public int enemyCount;
    void Start()
    {   
        randomizeTurnOrder();
        party = new List<Hero>();
        enemies = new List<Enemy>();
        populateSides();
        setEnemyIntention();
        for(int i = 0; i <5; i++){
            drawCard();
        }
        
        

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

    void drawCard(){
        Card card = Instantiate(cardDefault).GetComponent<Card>();
        card.transform.SetParent(transform.Find("CardUI/HandArea"),false);
        card.ID = party[0].draw();
        
    }
}
