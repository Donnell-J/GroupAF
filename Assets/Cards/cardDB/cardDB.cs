using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cardDB : MonoBehaviour {
    
    public static cardDB instance; //Establish as a singleton
    public Dictionary<int,string[]> db = new Dictionary<int, string[]>(); 
    public Dictionary<int[], int> comboDB = new Dictionary<int[], int>();
    public int keyCount =0;

    private int[][] startingDecks = {new int[]{0,0,0,0,1,1,2,2,2,2,2,3,4,5,6},
                                     new int[]{100,100,100,100,101,102,102,102,102,102,103,103,104,104,105},
                                     new int[]{200,200,200,200,200,201,201,201,201,201,202,202,203,204,204},
                                     new int[]{300,300,300,300,300,301,301,301,301,301,302,302,303,304,304}}; //Array of starting decks for each hero

    public Dictionary<int,List<int>> heroDecks = new Dictionary<int, List<int>>();
    public Dictionary<int,List<int>> checkPointHeroDecks = new Dictionary<int, List<int>>();
    public int[] heroMaxHPs; //initial max hero hp
    public int[] savedHeroMaxHPs;

    void Awake(){
        startSingleton();
        
    }
    
    void startSingleton(){ //Ensures that only one singleton exists at a time
        if(instance == null){
            instance = this;
            heroMaxHPs = new int[]{60,60,55,55};
            loadDB();
            setStartDecks();
        }else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//Makes it so singleton isnt destroyted when loading new scenes
    }

    void loadDB(){
        TextAsset dataAsset = Resources.Load<TextAsset>("cards"); //Loads Assets/Resources/cards.csv
        string[] cardData = dataAsset.text.Split('\n'); //Splits into rows

        for (int i = 1; i < cardData.Length - 1; i++){
            string[] record = cardData[i].Split(new char[] {','}); //splits row by ',' to create array
            if (record[0] != ""){
                db.Add(int.Parse(record[0]),record[1..]); //Add to dictionary where Id is the int key and the rest of the record is the string[] value
            }
        }
        TextAsset comboDataAsset = Resources.Load<TextAsset>("comboCards"); //Same as card csv, but for comboCards.csv
        cardData = comboDataAsset.text.Split('\n');

        /*Following loop splits csv into records, then converts it all into integers, to create a Dictionary, 
        where the keys are an array of 2 card IDs, and the value is the id of the card they combine to make
        */
        for (int i = 1; i < cardData.Length - 1; i++){
            string[] record = cardData[i].Split(new char[] {','});
            if (record[0] != ""){
                int id = int.Parse(record[0]);
                string[] splitReq = record[1].Split(new char[] {'.'});
                int[] reqs = Array.ConvertAll<string,int>(splitReq, int.Parse);
                comboDB.Add(reqs,id);
            }
        }

    }
        
    public void setStartDecks(){ //Sets each hero deck to the starting deck
        for(int i = 0; i < 4; i++){
            List<int> l =new List<int>();
            checkPointHeroDecks.Add(i,null);
            l.AddRange(startingDecks[i]);
            heroDecks.Add(i,l);
        }
    }
    public void nextLevelSave(){
        for(int i = 0; i < 4; i++){
            checkPointHeroDecks[i] = heroDecks[i];
            Debug.Log(i);
            
        }
        savedHeroMaxHPs = heroMaxHPs;
    }

    public void loadSaveState(){
        for(int i = 0; i < 4; i++){  
            heroDecks[i] = checkPointHeroDecks[i];
        }
        heroMaxHPs = savedHeroMaxHPs;
        keyCount =0;
    }
    
}
