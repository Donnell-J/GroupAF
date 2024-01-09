using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{

    public Transform mTarget;
    public Animator anim;
    public UnityEngine.AI.NavMeshAgent agent;

    float mSpeed = 10.0f;
    const float EPSILON = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = mTarget.position;
        agent.speed = mSpeed;
    }

    // Update is called once per frame
    void Update()
    {
            anim.SetFloat("Velocity",agent.velocity.magnitude);
        if ((transform.position - mTarget.position).magnitude < EPSILON){
            agent.destination = transform.position;
        }else{
            agent.destination = transform.position;
        }

    }
}
