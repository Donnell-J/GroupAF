using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Card : MonoBehaviour {
    
    // Start is called before the first frame update

    public int ID;
    private string title;
    private string description;
    private string img_path;
    private List<object> base_values = new List<object>();
    private List<string> targets = new List<string>();
    private List<string> functions = new List<string>();
    public  float h;
    public float scale;
    public TMP_Text[] cardText;
    //public static bool isGlobalTargetting = false;
    private bool isTargetting = false;
    public LineRenderer lineRenderer;

    [SerializeField]
    private InputActionReference mousePos;

    Hero mainHeroTarget;
    Enemy mainEnemyTarget;
    //For Bezier function;
    Canvas canvas;
    
    void Start(){

        canvas = transform.parent.parent.GetComponent<Canvas>(); //Getting size of the CardUI coanvas so that points are resized correctly for lineRenderer to draw to scale
        h = canvas.GetComponent<RectTransform>().rect.height;
        //record stores data in the format id, title, description, img_path, base_value, functions
        string[] record = cardDB.instance.db[ID];
        this.title = record[0];
        this.description = record[1];
        this.description = this.description.Replace("]",","); //Replaces arbitrary ] character with ',' as data is stored in a csv so ',' cannot be included in text
        this.img_path = record[2]; //no effect as of prototype, will be used later for loading card art


         //'.' character used in fields to create jagged Arrays so that enough paramters can be supplied when requirements are dynamic without overcomplicating the csv 
        foreach(string valString in record[3].Split(new char[] {'.'})){
            this.base_values.Add(valString); //Provides all base values for each of the card's functions 
        }
        foreach(string valString in record[4].Split(new char[] {'.'})){
            this.targets.Add(valString);//Provides what objects each function is supposed to effect. The first of these values determines where the card must be dragged to

        }
        foreach(string valString in record[5].Split(new char[] {'.'})){
            this.functions.Add(valString);
        }

        //Loads text into the card

        cardText = GetComponentsInChildren<TMP_Text>();
        cardText[0].text = (this.title);
        cardText[1].text = string.Format(this.description, this.base_values.ToArray());

    }

    // Update is called once per frame   
    void Update()
    {
        if(isTargetting){

            DrawTargetLine(); //If the card is currently targetting recalculate the line renderer points

        }
    }

    
    void playCard(){

        /*
        Creates a raycast from the Mouse's current position into the world. The first element of targets[] determines where the card is supposed to be played.
        If the ray cast collider matches a valid object for the target, then it will start running each of the card's function with collider as the main target.
        */

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos.action.ReadValue<Vector2>());
        if(Physics.Raycast(ray, out hit, 100)){

            if(targets[0]=="self" & hit.collider.transform == BattleController.party[BattleController.turnIndex].transform){
                startCardExecution(BattleController.party[BattleController.turnIndex] as IcombatFunction);
            }else if((targets[0] == "ally" | targets[0] == "team" )& hit.collider.TryGetComponent<Hero>(out mainHeroTarget)){

                startCardExecution(mainHeroTarget);
            }else if(targets[0]=="enemy" & hit.collider.TryGetComponent<Enemy>(out mainEnemyTarget)){
                startCardExecution(mainEnemyTarget as IcombatFunction);
            }
            
        }
    }
    void startCardExecution(IcombatFunction mainTarget){

        int valIndex =0; //Current offset in base_values[] we need to start reading from for the current function
        for(int i = 0; i < functions.Count;i++){
            List<IcombatFunction> allTargets = new List<IcombatFunction>(); 

            if(targets[i]=="team"){
                foreach(Hero hero in BattleController.party){
                    allTargets.Add(hero);//Targets everyone in the team
                }
            }else if(targets[i] == "enemyAdjacent"){
                int mainTargetPos = BattleController.enemies.IndexOf((Enemy)mainTarget); //Finds out if there are adjacent allies

                Debug.Log(mainTargetPos);
                if(mainTargetPos > 0){
                    allTargets.Add(BattleController.enemies[mainTargetPos-1] as IcombatFunction);
                }if (mainTargetPos <BattleController.enemies.Count){
                    allTargets.Add(BattleController.enemies[mainTargetPos+1] as IcombatFunction);
                }
            }else if(targets[i]=="self"){

                allTargets.Add(BattleController.party[BattleController.turnIndex]);//Targets the hero who's turn it currently is
            }else if(targets[i] == "enemy"){
                allTargets.Add(mainEnemyTarget);//targets enemy from the raycast earlier
            }else if(targets[i] == "ally"){
                allTargets.Add(mainHeroTarget);//targets hero from the raycast earlier

            }
            string func = this.functions[i];
            if(func.Contains("getHit")){
                foreach(IcombatFunction tgt in allTargets){

                    int calcDmg = Convert.ToInt32(this.base_values[valIndex]); //Vals stored as Object, casts to integer
                    if (BattleController.party[BattleController.turnIndex].statuses.ContainsKey("weakened")){
                        calcDmg /=2; //weakened halves dmg
                    }
                    if (BattleController.party[BattleController.turnIndex].statuses.ContainsKey("strengthen")){
                        calcDmg *=2; //strengthen doubles dmg
                    }
                        tgt.getHit(calcDmg,false); //hit the target without bypassing shields
                }
                valIndex++;//getHit only uses 1 param so next function starts +1 in 

            }else if(func.Contains("applyStatus")){
                foreach(IcombatFunction tgt in allTargets){
                    tgt.applyStatus(this.base_values[valIndex+1].ToString(),Convert.ToInt32(this.base_values[valIndex]));
                }

                valIndex +=2;//applyStatus uses 2 params, so increase index by 2

            }else if(func.Contains("defend")){
                foreach(IcombatFunction tgt in allTargets){
                    tgt.defend(Convert.ToInt32(this.base_values[valIndex]));
                }
                valIndex ++;
            }else if(func.Contains("heal")){
                foreach(IcombatFunction tgt in allTargets){
                    tgt.heal(Convert.ToInt32(this.base_values[valIndex]));
                }
                valIndex ++;
            }
        }

        BattleController.party[BattleController.turnIndex].discard(this.ID);//moves card played into discardPile
        BattleController.turnInProgress = true;//ends turn
        destroy();
    }

    public void destroy(){ //Destorys the gameobject and script instance for this card
        Destroy(gameObject);
        Destroy(this);
    }
    public void beginTargetting(){//When dragging has started, refind the current scale of the UI, and set the LineRenderer to have 30 points 

        
        scale = canvas.scaleFactor;
        lineRenderer.positionCount = 30;
        isTargetting = true;
    }
    

    public void endTargetting(){ //When dragging has ended, set Linerenderer to have 0 points, so that it is effectively hidden
        lineRenderer.positionCount = 0;
        isTargetting = false;
        playCard();//Attempt to play the card
    }

    void DrawTargetLine(){
        Vector2 p0 = new Vector2(0,30); //Start targetting Line 30 units up from the middle of the card 

        // find mouse position relative to card position and make it the final point for the line
        Vector2 mouseLoc = mousePos.action.ReadValue<Vector2>(); 
        Vector3 orig = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 p2 = new Vector2((mouseLoc.x-orig.x),mouseLoc.y-orig.y); 
        p2 = p2/(scale);
        Vector2 p1 = new Vector2(0,h/2); // Control point in the middle of the screen above card
        float t = 0f;
        Vector3 position;
        for(int i = 0; i < 30; i++){ //Quadratic Bezier curve function to create a better set of points for the line

   
			t = i / (30.0f-1.0f);
			position = ((1.0f - t)*(1.0f - t)) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
            
			lineRenderer.SetPosition(i, position);
        }
    }
}