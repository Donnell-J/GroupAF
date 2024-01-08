using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardViewMenu : MonoBehaviour
{
    public GameObject deckButton;
    public GameObject discardButton;
    public GameObject menu;

    public GameObject cardOOC;

    public GameObject[] tabs;
    public GameObject[] tabContents;

    public Button[] buttons;

    public Hero[] heroListSorted = new Hero[4];

    public string menuMode;

    public TMP_Text title;


    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        
        hideAllTabs();

    }

    public void initialise(){
        foreach(Hero h in BattleController.party){
            if (h.name.Contains("Knight")){
                heroListSorted[0] = h;
            }else if (h.name.Contains("Ranger")){
                heroListSorted[1] = h;
            }else if (h.name.Contains("Wizard")){
                heroListSorted[2] = h;
            }else{
                heroListSorted[3] = h;
            }
        }
        Debug.Log("inited");
    }
    public void openMenu(string m){
        title.text = m+" Piles";
        loadCards(m);
        menu.SetActive(true);
        int tab;
        string curName = BattleController.party[BattleController.turnIndex].name;
        if(curName.Contains("Knight")){
            tab = 0;
        }else if(curName.Contains("Ranger")){
            tab = 1;
        }else if(curName.Contains("Wizard")){
            tab = 2;
        }else{
            tab = 3;
        }
        selectTab(tab);
        
    }

    public void closeMenu(){
        for(int i = 0; i < 4; i++){
            Debug.Log(tabContents[i].transform.childCount);
            for(int j = 0; j<tabContents[i].transform.childCount ;j++){
                tabContents[i].transform.GetChild(j).GetComponent<cardOOC>().destroy();
            }
        }
        menu.SetActive(false);
    }

    public void loadCards(string mode){
        List<int> cardIDList;
        for(int i = 0; i < 4; i++){
            if(mode.Equals("Discard")){
                cardIDList = heroListSorted[i].discardPile;
            }else{
                cardIDList = heroListSorted[i].currentDeck;
            }

            for(int j = 0; j < cardIDList.Count; j++){
                cardOOC card = Instantiate(cardOOC).GetComponent<cardOOC>();
                card.ID = cardIDList[j];
                card.transform.SetParent(tabContents[i].transform,false);
            }
        }
    }
    public void hideAllTabs(){
        foreach(GameObject tab in tabs){
            tab.SetActive(false);
        }
        foreach(Button b in buttons){
            b.enabled = true;
        }
    }
    public void selectTab(int tabInd){
        hideAllTabs();
        tabs[tabInd].SetActive(true);
        buttons[tabInd].enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
