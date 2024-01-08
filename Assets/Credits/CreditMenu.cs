using System;
using UnityEngine;

public class CreditMenu : MonoBehaviour
{
    public GameObject FirstCreditMenuUI;
    public GameObject SecondCreditMenuUI;
    public GameObject ThirdCreditMenuUI;
    public GameObject FourthCreditMenuUI;

    private void Start()
    {
        SecondCreditMenuUI.SetActive(false);
        ThirdCreditMenuUI.SetActive(false);
        FourthCreditMenuUI.SetActive(false);
    }

    public void SwitchToSecondCreditMenu()
    {
        FirstCreditMenuUI.SetActive(false);
        SecondCreditMenuUI.SetActive(true);
    }
    
    public void SwitchToThirdCreditMenu()
    {
        SecondCreditMenuUI.SetActive(false);
        ThirdCreditMenuUI.SetActive(true);
    }
    
    public void SwitchToFourthCreditMenu()
    {
        ThirdCreditMenuUI.SetActive(false);
        FourthCreditMenuUI.SetActive(true);
    }
}
