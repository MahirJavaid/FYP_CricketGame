using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform batTarget;           // already assigned
    public float launchForce = 10f;
    public float hitForce = 8f;

    private Rigidbody rb;
    private bool launched = false;
    private bool hit = false;

    private float flightTimer = 0f;
    private float maxFlightDuration = 2.5f; // time after which we assume it's missed

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void LaunchTowardsBat()
    {
        if (batTarget == null)
        {
            Debug.LogWarning("Bat target not assigned!");
            return;
        }

        launched = true;
        hit = false;
        flightTimer = 0f;

        Vector3 direction = (batTarget.position - transform.position).normalized;
        rb.linearVelocity = direction * launchForce;
    }

    void Update()
    {
        if (launched && !hit)
        {
            flightTimer += Time.deltaTime;

            // Case: ball passed the target zone
            if (flightTimer >= maxFlightDuration)
            {
                hit = true;
                StartCoroutine(ResetAfterDelay());
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hit) return;

        if (collision.gameObject.CompareTag("Bat"))
        {
            hit = true;

            GameObject batsman = GameObject.FindGameObjectWithTag("Batsman");
            if (batsman != null)
            {
                Animator anim = batsman.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("Hit");
                }
            }

            Vector3 bounceDirection = (transform.position - collision.transform.position).normalized;
            bounceDirection.y = 1f;

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(bounceDirection * hitForce, ForceMode.Impulse);

            StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);

        BowlerMovement bowler = FindObjectOfType<BowlerMovement>();
        if (bowler != null)
        {
            bowler.ResetForNextDelivery();
        }
    }
}
