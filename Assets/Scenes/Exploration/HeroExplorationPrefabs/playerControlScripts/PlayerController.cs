using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference lmb, mousePos;

    [SerializeField]
    private Animator anim;

    private UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update

    void Awake(){
        //MovingScenes.instance.player = this.gameObject;
    }
    void Start()
    {
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Debug.Log(MovingScenes.instance.getPreCombatPosition());
        if(MovingScenes.instance.getPreCombatPosition() != Vector3.zero){
            agent.Warp(MovingScenes.instance.getPreCombatPosition());
        }
        
        

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Velocity",agent.velocity.magnitude);
        RaycastHit hit;
        if(lmb.action.WasReleasedThisFrame()){ //Moves Player to in-world location based on where the mouse was 
            Ray ray = Camera.main.ScreenPointToRay(mousePos.action.ReadValue<Vector2>());
            if(Physics.Raycast(ray, out hit, 100)){
                agent.destination = hit.point;
            }
        }
    }
}