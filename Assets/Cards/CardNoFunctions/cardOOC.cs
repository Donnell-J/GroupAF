using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cardOOC : MonoBehaviour
{
     public int ID;
    private string title;
    private string description;
    private string img_path;
    private List<object> base_values = new List<object>();
    private List<string> targets = new List<string>();
    private List<string> functions = new List<string>();
    public TMP_Text[] cardText;
    // Start is called before the first frame update
    void Start(){
        //id, title, description, img_path, base_value, functions
        string[] record = cardDB.instance.db[ID];
        this.title = record[0];
        this.description = record[1];
        this.description = this.description.Replace("]",",");
        this.img_path = record[2];
        foreach(string valString in record[3].Split(new char[] {'.'})){
            this.base_values.Add(valString);
        }
        foreach(string valString in record[4].Split(new char[] {'.'})){
            this.targets.Add(valString);
        }
        foreach(string valString in record[5].Split(new char[] {'.'})){
            this.functions.Add(valString);
        }


        cardText = GetComponentsInChildren<TMP_Text>();
        cardText[0].text = (this.title);
        cardText[1].text = string.Format(this.description, this.base_values.ToArray());

    }

    public void destroy(){
        Destroy(gameObject);
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
