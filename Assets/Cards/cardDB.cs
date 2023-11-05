using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDB : MonoBehaviour {
    
    public static cardDB instance; 
    public Dictionary<int,string[]> db = new Dictionary<int, string[]>();
    public Dictionary<int, string[]> comboDB = new Dictionary<int, string[]>();


    private int[][] startingDecks = {new int[]{0,0,0,0,1,1,2,2,2,2,2,3,4,5,6},
                                     new int[]{100,100,100,100,101,102,102,102,102,102,103,103,104,104,105},
                                     new int[]{200,200,200,200,200,201,201,201,201,201,202,202,203,204,204},
                                     new int[]{300,300,300,300,300,301,301,301,301,301,302,302,303,304,304}};

    public Dictionary<int,List<int>> heroDecks = new Dictionary<int, List<int>>();


    void Awake(){
        startSingleton();
    }
    
    void startSingleton(){
        if(instance == null){
            instance = this;
            loadDB();
            setStartDecks();
        }else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void loadDB(){
        TextAsset dataAsset = Resources.Load<TextAsset>("cards");
        string[] cardData = dataAsset.text.Split('\n');

        for (int i = 1; i < cardData.Length - 1; i++){
            string[] record = cardData[i].Split(new char[] {','});
            if (record[0] != ""){
                db.Add(int.Parse(record[0]),record[1..]);
            }
        }
        TextAsset comboDataAsset = Resources.Load<TextAsset>("comboCards");
        cardData = dataAsset.text.Split('\n');

        for (int i = 1; i < cardData.Length - 1; i++){
            string[] record = cardData[i].Split(new char[] {','});
            if (record[0] != ""){
                comboDB.Add(int.Parse(record[0]),record[1..]);
            }
        }

    }
        
    public void setStartDecks(){
        for(int i = 0; i < 4; i++){
            List<int> l =new List<int>();
            l.AddRange(startingDecks[i]);
            heroDecks.Add(i,l);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
