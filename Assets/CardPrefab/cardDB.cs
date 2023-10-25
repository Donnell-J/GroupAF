using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDB : MonoBehaviour {
    
    public static cardDB instance; 
    public List<string[]> db = new List<string[]>();


    void Awake(){
        startSingleton();
    }
    
    void startSingleton(){
        if(instance == null){
            instance = this;
            loadDB();
        }else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void loadDB(){
        TextAsset dataAsset = Resources.Load<TextAsset>("cards");
        Debug.Log(dataAsset);
        string[] cardData = dataAsset.text.Split('\n');

        for (int i = 1; i < cardData.Length - 1; i++){
            string[] record = cardData[i].Split(new char[] {','});
            db.Add(record);
        }

        
        Debug.Log(db[0][1]);
        Debug.Log(db[1][1]);
    }
        
        
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
