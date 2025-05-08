using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattingScript : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0, 0, 0.1f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(0, 0, -0.1f);
        }

        if (Input.GetKey(KeyCode.C))
        {
            anim.SetTrigger("coverDrive");
        }

        if (Input.GetKey(KeyCode.L))
        {
            anim.SetTrigger("leave");
        }

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetTrigger("defence");
        }

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetTrigger("strike");
        }

        if (Input.GetKey(KeyCode.W))
        {
            anim.SetTrigger("flick");
        }

        anim.SetTrigger("idle");
    }
}
