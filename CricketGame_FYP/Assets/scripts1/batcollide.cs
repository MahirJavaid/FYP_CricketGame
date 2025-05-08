using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batcollide : MonoBehaviour
{
    public float hitForce = 15f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 hitDirection = transform.forward + Vector3.up * 0.3f;
                rb.velocity = Vector3.zero;
                rb.AddForce(hitDirection.normalized * hitForce, ForceMode.VelocityChange);
                Debug.Log("ball hit!");
            }
        }
    }
}

