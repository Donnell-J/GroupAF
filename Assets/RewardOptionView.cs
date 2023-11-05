using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardOptionView : MonoBehaviour
{

    public GameObject rewardScreenUI;
    public GameObject cardRandomiserUI;
    public GameObject healthBoostUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void viewCardRandomiser()
    {
        Debug.Log("Loading Card Randomiser..");
        rewardScreenUI.SetActive(false);
        cardRandomiserUI.SetActive(true);
        Time.timeScale = 0f;
        // gameIsPaused = false;
    }
    // void pause()
    // {
    //     pauseMenuUI.SetActive(true);
    //     Time.timeScale = 0f;
    //     gameIsPaused = true;
    // }
}
