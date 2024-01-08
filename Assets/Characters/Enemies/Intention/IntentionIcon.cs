using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class IntentionIcon : MonoBehaviour
{
    
    public Image middleImg;
    public Image leftImg;
    public Image rightImg;
    public TMP_Text leftVal;
    public TMP_Text rightVal;
    public TMP_Text middleVal;
    
    public Sprite attackIcon;
    public Sprite blockIcon;
    public Sprite debuffIcon;
    public Sprite buffIcon;
    public Sprite attackBlockIcon;
    public Sprite attackDebuffIcon;
    public Sprite teamAttackIcon;
    public Sprite healIcon;

    public Sprite targetIcon;
    public Sprite knightIcon;
    public Sprite wizardIcon;
    public Sprite RangerIcon;
    public Sprite clericIcon;
    public Sprite enemyIcon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setIntentIcon(int action, int value, string targetName){
        leftImg.enabled = false;
        rightImg.enabled = false;
        leftVal.text  ="";
        middleVal.text = "";
        rightVal.text = "";

        if(targetName.Contains("self") || action == 6){
            middleImg.sprite = findIntent(action);
            middleVal.text = value.ToString();
        } else{
            leftImg.enabled = true;
            rightImg.enabled = true;
            middleImg.sprite = targetIcon;
            leftImg.sprite = findIntent(action);
            leftVal.text = value.ToString();
            if(action == 2 || action == 3){
                leftVal.text = "";
            }

            if(targetName.Contains("Knight")){
                rightImg.sprite = knightIcon;

            } else if(targetName.Contains("Wizard")){

                rightImg.sprite = wizardIcon;
            } else if(targetName.Contains("Ranger")){

                rightImg.sprite = RangerIcon;
            } else if(targetName.Contains("Cleric")){
                rightImg.sprite = clericIcon;
            }else{
                rightImg.sprite = enemyIcon;
                rightVal.text = "#" + targetName;
            }

        }

       

        
    }

    public void updateDamageValue(int newVal){
        if(middleImg.sprite == teamAttackIcon){
            middleVal.text = newVal.ToString();
        }else if (leftImg.sprite == attackIcon ||leftImg.sprite == attackBlockIcon || leftImg.sprite == attackDebuffIcon){
            leftVal.text = newVal.ToString();
        }
    }

    public Sprite findIntent(int action){
        switch(action){
            case 0: //attack
                return attackIcon;
            case 1: //block
                return blockIcon;
                
            case 2: //debuff
                return debuffIcon;
                
            case 3: //buff
                return buffIcon;
                
            case 4: //attackBlock
                return attackBlockIcon;
                
            case 5: //attackDebuff
                return attackDebuffIcon;
                
            case 6: //teamAttack
                return teamAttackIcon;
            case 7: //Heal
                return healIcon;
                
        }
        return null;
    }
}
