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
        name = statusType;
        switch(statusType){
            case "weakened":
                Debug.Log(statusType);
                img.color = new Color32(188,153,130,255);
                break;

            case "barricade":
                Debug.Log(statusType);
                img.color = new Color32(41,228,249,255);
                break;
            case "stun":
                Debug.Log(statusType);
                img.color = new Color32(249,207,41,255);
                break;
            case "vulnerable":
                Debug.Log(statusType);
                img.color = new Color32(248,53,91,255);
                break;
            case "poison":
                Debug.Log(statusType);
                img.color = new Color32(0,153,0,255);
                break;
            case "strengthen":
                Debug.Log(statusType);
                img.color = new Color32(155,158,159,255);
                break;
        }
        counterText.text = string.Format("{0}",count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateCount(int change){
        if(count + change ==0){
            Destroy(gameObject);
        }
        count += change;
        counterText.text = string.Format("{0}",count);
    }
}
