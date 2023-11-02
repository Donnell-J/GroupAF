using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference lmb, mousePos;

    private UnityEngine.AI.NavMeshAgent agent;

    private InteractSceneSwitch interactCanvas;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        interactCanvas = FindAnyObjectByType<InteractSceneSwitch>();
    }
    
    //Update is called once per frame
    void Update()
    {
        if (!interactCanvas.openMenu)
        {
            RaycastHit hit;
            if (lmb.action.WasReleasedThisFrame())
            {
                Ray ray = Camera.main.ScreenPointToRay(mousePos.action.ReadValue<Vector2>());
                if (Physics.Raycast(ray, out hit, 100))
                {
                    agent.destination = hit.point;
                }
            }
        }
    }
}
