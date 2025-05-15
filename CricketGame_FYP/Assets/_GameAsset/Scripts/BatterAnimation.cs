using UnityEngine;

public class BatterAnimation : MonoBehaviour
{
    public void SetStrikeAnimaiont()
    {
        // Set the animation trigger for strike
        GetComponent<Animator>().SetTrigger("Hit");
    }

    public void SetMissAnimaiont()
    {
        // Set the animation trigger for miss
        GetComponent<Animator>().SetTrigger("Miss");
    }
}
