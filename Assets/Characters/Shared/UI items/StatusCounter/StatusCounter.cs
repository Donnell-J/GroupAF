using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusCounter : MonoBehaviour
{
    public Image statusimg;

    public Sprite weakenedIcon;
    public Sprite barricadeIcon;
    public Sprite stunIcon;
    public Sprite vulnerableIcon;
    public Sprite poisonIcon;
    public Sprite strengthenIcon;

    public TMP_Text counterText;
    public string statusType;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        name = statusType; // Set colour depending on status it represents
        switch(statusType){
            case "weakened":
                statusimg.sprite = weakenedIcon;
                break;

            case "barricade":
                statusimg.sprite = barricadeIcon;
                break;
            case "stun":
                statusimg.sprite = stunIcon;
                break;
            case "vulnerable":
                statusimg.sprite = vulnerableIcon;
                break;
            case "poison":
                statusimg.sprite = poisonIcon;
                break;
            case "strengthen":
                statusimg.sprite = strengthenIcon;
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
