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
        canvas = transform.parent.parent.GetComponent<Canvas>();
        h = canvas.GetComponent<RectTransform>().rect.height;
        //id, title, description, img_path, base_value, functions
        string[] record = cardDB.instance.db[ID];
        this.title = record[0];
        this.description = record[1];
        this.description = this.description.Replace("]",",");
        this.img_path = record[2];
        foreach(string valString in record[3].Split(new char[] {'.'})){
            this.base_values.Add(valString);
        }
        foreach(string valString in record[4].Split(new char[] {'.'})){
            this.targets.Add(valString);
        }
        foreach(string valString in record[5].Split(new char[] {'.'})){
            this.functions.Add(valString);
        }


        cardText = GetComponentsInChildren<TMP_Text>();
        cardText[0].text = (this.title);
        cardText[1].text = string.Format(this.description, this.base_values.ToArray());

    }

    // Update is called once per frame   
    void Update()
    {
        if(isTargetting){
            DrawTargetLine();
        }
    }

    
    void playCard(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos.action.ReadValue<Vector2>());
        if(Physics.Raycast(ray, out hit, 100)){

            if(targets[0]=="self" & hit.collider.transform == BattleController.party[BattleController.turnIndex].transform){
                Debug.Log("Target self");
                Debug.Log(BattleController.party[BattleController.turnIndex]);
                startCardExecution(BattleController.party[BattleController.turnIndex] as IcombatFunction);
            }else if((targets[0] == "ally" | targets[0] == "team" )& hit.collider.TryGetComponent<Hero>(out mainHeroTarget)){
                Debug.Log("Target Ally");
                startCardExecution(mainHeroTarget as IcombatFunction);
            }else if(targets[0]=="enemy" & hit.collider.TryGetComponent<Enemy>(out mainEnemyTarget)){
                Debug.Log("Attacking Enemy");
                startCardExecution(mainEnemyTarget as IcombatFunction);
            }
            
        }
    }
    void startCardExecution(IcombatFunction mainTarget){
        int valIndex =0;
        for(int i = 0; i < functions.Count;i++){
            List<IcombatFunction> allTargets = new List<IcombatFunction>(); 
            if(targets[i]=="team"){
                foreach(Hero hero in BattleController.party){
                    allTargets.Add(hero);
                }
            }else if(targets[i] == "enemyAdjacent"){
                Debug.Log("Running function on adjacents");
                int mainTargetPos = BattleController.enemies.IndexOf((Enemy)mainTarget);
                Debug.Log(mainTargetPos);
                if(mainTargetPos > 0){
                    allTargets.Add(BattleController.enemies[mainTargetPos-1].GetComponent<Enemy>() as IcombatFunction);
                }if (mainTargetPos <3){
                    allTargets.Add(BattleController.enemies[mainTargetPos+1].GetComponent<Enemy>() as IcombatFunction);
                }
            }else if(targets[i]=="self"){
                allTargets.Add(BattleController.party[BattleController.turnIndex]);
            }else if(targets[i] == "enemy" | targets[i] =="ally"){
                allTargets.Add(mainEnemyTarget);
            }
            string func = this.functions[i];
            //Debug.Log(func);
            //Debug.Log(func.Contains("getHit"));
            if(func.Contains("getHit")){
                foreach(IcombatFunction tgt in allTargets){
                    Debug.Log(tgt);
                    int calcDmg = Convert.ToInt32(this.base_values[valIndex]);
                    Debug.Log(calcDmg);
                    if (BattleController.party[BattleController.turnIndex].statuses.ContainsKey("weakened")){
                        calcDmg /=2;
                    }
                    if (BattleController.party[BattleController.turnIndex].statuses.ContainsKey("strengthen")){
                        calcDmg *=2;
                    }
                        tgt.getHit(calcDmg);
                }
                valIndex++;
            }else if(func.Contains("applyStatus")){
                foreach(IcombatFunction tgt in allTargets){
                    tgt.applyStatus(this.base_values[valIndex+1].ToString(),Convert.ToInt32(this.base_values[valIndex]));
                }
                valIndex +=2;
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
        BattleController.party[BattleController.turnIndex].discard(this.ID);
        BattleController.turnInProgress = true;
        destroy();
    }

    public void destroy(){
        Destroy(gameObject);
        Destroy(this);
    }
    public void beginTargetting(){
        
        scale = canvas.scaleFactor;
        lineRenderer.positionCount = 30;
        isTargetting = true;
    }
    
    public void endTargetting(){
        lineRenderer.positionCount = 0;
        isTargetting = false;
        playCard();
    }

    void DrawTargetLine(){
        Vector2 p0 = new Vector2(0,30);
        Vector2 mouseLoc = mousePos.action.ReadValue<Vector2>();
        Vector3 orig = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 p2 = new Vector2((mouseLoc.x-orig.x),mouseLoc.y-orig.y);
        p2 = p2/(scale);
        Vector2 p1 = new Vector2(0,h/2);
        float t = 0f;
        Vector3 position;
        for(int i = 0; i < 30; i++){
			t = i / (30.0f-1.0f);
			position = ((1.0f - t)*(1.0f - t)) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
            
			lineRenderer.SetPosition(i, position);
        }
    }
}