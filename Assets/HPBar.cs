using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations;
public class HPBar : MonoBehaviour
{
    Camera cam;   
    public Slider slider;
    public TMP_Text tField;
    private int maxHP;
    private int shield;
    // Start is called before the first frame update
    public void setMax(int maxHP){
        slider.maxValue = maxHP;
        setHealth(maxHP);
    }
    public void setHealth(int newHP){
        slider.value = newHP;
        tField.text = string.Format("{0}/{1}  (+{2})",slider.value,slider.maxValue,shield);
    }
    public void setShield(int newshieldAmount){
        shield = newshieldAmount;
        tField.text = string.Format("{0}/{1}  (+{2})",slider.value,slider.maxValue,shield);

    }
    void Start(){
        cam = Camera.main;
        slider.value = slider.maxValue;
        tField.text = string.Format("{0}/{1}  (+{2})",slider.value,slider.maxValue,shield);
    }

    void LateUpdate(){
        Vector3 newRotation = cam.transform.eulerAngles;
        newRotation.z = 0;
        newRotation.y = 0;
        transform.parent.eulerAngles = newRotation;
    }
}
