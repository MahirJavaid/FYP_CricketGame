using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlerMovement : MonoBehaviour
{
    public Transform bowlPosition;
    public Animator animator;
    public GameObject ball;
    public Transform releasePoint;
    public Transform target;

    private float totalRunTime = 4.0f;
    private float timer = 0f;
    private Vector3 startPos;
    private bool moving = false;
    private bool ballReleased = false;

    private bool isSpin = false;

    private int currentDelivery = 0;
    private const int maxDeliveries = 6;
    private Vector3 ballStartPos;

    void Start()
    {
        startPos = transform.position;
        ballStartPos = ball.transform.position;
    }

    void Update()
    {
        if (!moving && !ballReleased)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartBowling(true); // spin
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                StartBowling(false); // fast
            }
        }

        if (moving)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / totalRunTime);
            transform.position = Vector3.Lerp(startPos, bowlPosition.position, t);

            if (t >= 1.0f)
            {
                moving = false;
                ReleaseBall();
            }
        }
    }

    void StartBowling(bool spin)
    {
        isSpin = spin;
        timer = 0f;
        moving = true;
        transform.position = startPos;
        ballReleased = false;

        if (isSpin)
        {
            animator.Play("LeftArmOrthodoxSpinner");
        }
        else
        {
            animator.Play("LeftArmFastBowler");
        }
    }

    void ReleaseBall()
    {
        if (ball == null || target == null || releasePoint == null) return;

        ball.transform.parent = null;
        ball.transform.position = releasePoint.position;

        // Call LaunchTowardsBat() from your renamed script 'ballmove'
        BallController ballController = ball.GetComponent<BallController>();
        if (ballController != null)
        {
            ballController.LaunchTowardsBat();
        }
        
        ballReleased = true;
    }

    public void ResetForNextDelivery()
    {
        currentDelivery++;

        if (currentDelivery >= maxDeliveries)
        {
            Debug.Log("Over complete!");
            return; // Optional: trigger UI, next bowler, etc.
        }

        // Reset bowler
        transform.position = startPos;
        moving = false;
        timer = 0f;

        // Reset ball
        ball.transform.position = ballStartPos;
        ball.transform.rotation = Quaternion.identity;
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        // Reset states
        ballReleased = false;

        // Ready for next input (S/F)
    }
}
