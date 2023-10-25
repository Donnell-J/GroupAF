using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Card : MonoBehaviour {
    
    // Start is called before the first frame update

    public int ID=0;
    private string title;
    private string description;
    private string img_path;
    private int base_value;
    private string functions;

    public TMP_Text[] cardText;

    void Start()
    {

        //id, title, description, img_path, base_value, functions
        string[] record = cardDB.instance.db[ID];
        this.title = record[1];
        this.description = record[2];
        this.img_path = record[3];
        this.base_value = int.Parse(record[4]);
        this.functions = record[5];

        cardText = GetComponentsInChildren<TMP_Text>();

        cardText[0].text = (this.title);
        cardText[1].text = string.Format(this.description, this.base_value);

    }

    // Update is called once per frame
    void Update()
    {
    }
}