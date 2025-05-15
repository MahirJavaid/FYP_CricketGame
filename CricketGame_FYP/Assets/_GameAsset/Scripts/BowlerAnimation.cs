using System;
using UnityEngine;

public class BowlerAnimation : MonoBehaviour
{
    bool changeBowler = false;

    public void BowlingAnimation()
    {
        if(changeBowler)
            GetComponent<Animator>().SetTrigger("Bowl");
        else
            GetComponent<Animator>().SetTrigger("Bowl2");
    }

    internal void ChangeBowllingStyle()
    {
        changeBowler = !changeBowler;
    }
}
