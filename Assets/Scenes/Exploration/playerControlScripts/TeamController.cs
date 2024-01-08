using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{

    public Transform mTarget;
    public Animator anim;

    float mSpeed = 10.0f;
    const float EPSILON = 1.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mTarget.position);
        if ((transform.position - mTarget.position).magnitude > EPSILON)
        {
            anim.SetFloat("Velocity",1f);
            transform.Translate(0.0f, 0.0f, (mSpeed * Time.deltaTime));
        }else{
            anim.SetFloat("Velocity",1f);
        }

    }
}
