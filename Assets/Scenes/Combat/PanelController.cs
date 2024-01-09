using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PanelController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Panel;
    public BattleController bc;

    public Transform handUI;
    public Transform comboUI;
    public Transform[] inputSlots;
    public cardOOC[] inputCards = new cardOOC[2];
    public Transform outputSlot;
    public GameObject cardOOC;
    private int locPointer = 0;
    private List<int> currentHand = new List<int>();

    void Awake(){
        Panel.SetActive(false);
    }

    public void OpenPanel(){
        if(!Panel.activeSelf){
            Panel.SetActive(true);
            SetCards();
        }
        
    }

    public void ClosePanel(){
        Panel.SetActive(false);
        //Go Through all necessary feilds and game objects and restore them to a default setting when the panel is closed
        for(int i =0; i < handUI.childCount;i++){
            handUI.GetChild(i).GetComponent<cardOOC>().destroy();
        }
        inputCards = new cardOOC[2];
        if(outputSlot.childCount > 0){
            outputSlot.GetChild(0).GetComponent<cardOOC>().destroy();
        }
        for(int i =0; i < 2;i++){
            if (inputSlots[i].childCount > 0){
                inputSlots[i].GetChild(0).GetComponent<cardOOC>().destroy();
            }
        }    
    }

    public void SetCards(){
        currentHand = BattleController.party[BattleController.turnIndex].hand;
        for(int i = 0; i < currentHand.Count; i++){
            cardOOC card = Instantiate(cardOOC).GetComponent<cardOOC>();
            card.ID = currentHand[i];
            card.transform.SetParent(handUI,false);
            card.GetComponent<Button>().onClick.AddListener(delegate {cardButtonPressed(card);});
        }

    }

    public void cardButtonPressed(cardOOC cardIn){
        if(inputCards.Contains(cardIn)){
            return; //If the card has already been selected, ignore it's call to this function so no duplication involved
        }
        inputCards[locPointer] = cardIn; //add card pointer to input Array
        cardOOC card = Instantiate(cardOOC).GetComponent<cardOOC>(); 
        card.ID = cardIn.ID;//Setup new card for visual confirmation of selection
        if (inputSlots[locPointer].childCount > 0){
            inputSlots[locPointer].GetChild(0).GetComponent<cardOOC>().destroy();//Destroy the previous visaul-only card, prevent excess objs
        }
        card.transform.SetParent(inputSlots[locPointer], false);
        locPointer = (locPointer + 1) %2; //Move to next pointer and loop back if >1
        comboCheck();
    }  

    void comboCheck(){
        if(inputCards[0] == null | inputCards[1] == null){return;}; //If 2 cards havent been selected you, don't do anything
        int[] cardIDtoCheck = new int[]{inputCards[0].ID,inputCards[1].ID}; //Create new int array of selected card IDs

        foreach(int[] key in cardDB.instance.comboDB.Keys){ //Loop through each combination in the singleton comboDB
            if(cardIDtoCheck.All(key.Contains)){ //If there is no difference in the array of selected IDs and the requirements, instance a new card with the Id of the resulting card 
                cardOOC card = Instantiate(cardOOC).GetComponent<cardOOC>(); 
                card.ID = cardDB.instance.comboDB[key];
                Debug.Log(card.ID);//
                    if (outputSlot.childCount > 0){
                        outputSlot.GetChild(0).GetComponent<cardOOC>().destroy();
                }
                card.transform.SetParent(outputSlot, false);
                return;
            }
        }   
        if (outputSlot.childCount > 0){
            outputSlot.GetChild(0).GetComponent<cardOOC>().destroy();
        }
    }

    public void onConfirm(){ 
        if(outputSlot.childCount > 0){ //Means valid combo has already been found
            ref List<int> heroHand = ref BattleController.party[BattleController.turnIndex].hand;
            heroHand.Remove(inputCards[0].ID); //Remove the cards used to make the combo card 
            heroHand.Remove(inputCards[1].ID);
            heroHand.Add(outputSlot.GetChild(0).GetComponent<cardOOC>().ID);
            bc.reloadCards(); //Reload cards in battle scene to reflect new hand state
            BattleController.turnInProgress = true;
            ClosePanel();
        }
    }
    void Start()
    {
        Panel.transform.SetAsLastSibling(); //ensure is drawn on top

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
