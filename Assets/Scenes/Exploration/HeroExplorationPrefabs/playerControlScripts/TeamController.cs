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
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mTarget.position);
        if ((transform.position - mTarget.position).magnitude > EPSILON)
        {
            transform.Translate(0.0f, 0.0f, (mSpeed * Time.deltaTime));
            anim.SetFloat("Velocity",1f);
        }else{
            anim.SetFloat("Velocity",0f);
        }

    }
}
