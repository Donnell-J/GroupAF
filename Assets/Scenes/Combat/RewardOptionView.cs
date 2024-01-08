using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class RewardOptionView : MonoBehaviour
{

    public GameObject rewardScreenUI;
    public GameObject cardRandomiserUI;
    private int rerollCount =6;
    public TMP_Text rerollLabel;
    public Button[] rerollButtons;
    public GameObject[] panels;
    public GameObject cardDefault;
    private int[] rewardIDs = new int[4];
    private int[][] possibleDraws= {new int[]{3,8}, //Jagged of array of ID ranges that can be gained for each hero
                         new int[]{103,108},
                         new int[]{202,207},
                         new int[]{303,308}}; 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false); //Hide the reward screen on start, so that it's not visible during combat
        for(int i = 0; i<4 ;i++){
            int j = i;
            rerollButtons[i].onClick.AddListener(delegate {rerollCard(j);}); // Set the index each button calls rerollCard with, so that it rerolls the approriate card
            rerollCard(j);
        }
        cardRandomiserUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void viewCardRandomiser()
    {
        rewardScreenUI.SetActive(false); //When "get a new card" is selected, hide the main part of this menu and show the card randomiser panel 
        cardRandomiserUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void rerollCard(int index){
        if(rerollCount > 0){
            cardOOC card;
            if(panels[index].transform.GetChild(0).TryGetComponent<cardOOC>(out card)){//If the first child of the panel is a card, destroy it, so only 1 card is active
                card.destroy();
            }
            card = Instantiate(cardDefault).GetComponent<cardOOC>(); //Make a new card obj and set its parent to the given panel
            card.transform.SetParent(panels[index].transform,false);
            card.ID = Random.Range(possibleDraws[index][0],possibleDraws[index][1]+1); //Choose an ID from the range of possible IDs for this hero
            card.transform.SetAsFirstSibling();
            rewardIDs[index] = card.ID;
            rerollCount -=1; //reduce number of rerolls by 1 and show this in the label
            rerollLabel.text = string.Format("Rerolls Left: {0}",rerollCount);
        }

    }

    public void increaseMaxHP(){
        for (int i =0; i <4; i++){
            cardDB.instance.heroMaxHPs[i] += 5; //increases max hp of each hero by 5 for the next combat
        }
        SceneManager.LoadScene(MovingScenes.instance.getFromScene());//Sends us back to Exploration scene 

    }

    public void confirmCardChoices(){ //Adds each of the chosen cards to their respective hero's deck 
        for(int i = 0; i < 4; i++){
            cardDB.instance.heroDecks[i].Add(rewardIDs[i]);
        }
        SceneManager.LoadScene(MovingScenes.instance.getFromScene());//Sends us back to Exploration scene 
    }

}
