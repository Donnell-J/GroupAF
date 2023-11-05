using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusCounter : MonoBehaviour
{
    public Image img;
    public TMP_Text counterText;
    public string statusType;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        name = statusType; // Set colour depending on status it represents
        switch(statusType){
            case "weakened":
                img.color = new Color32(188,153,130,255);
                break;

            case "barricade":
                img.color = new Color32(41,228,249,255);
                break;
            case "stun":
                img.color = new Color32(249,207,41,255);
                break;
            case "vulnerable":
                img.color = new Color32(248,53,91,255);
                break;
            case "poison":
                img.color = new Color32(0,153,0,255);
                break;
            case "strengthen":
                img.color = new Color32(155,158,159,255);
                break;
        }
        counterText.text = string.Format("{0}",count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateCount(int change){ //update text to show new number of stacks of status, deletes counter object if number is 0 
        if(count + change ==0){
            Destroy(gameObject);
        }
        count += change;
        counterText.text = string.Format("{0}",count);
    }
}
