using UnityEngine;

public class Keeper : MonoBehaviour
{
   public void TriggerCatch() => GetComponent<Animator>().SetTrigger("Catch");
}
