using UnityEngine;

public class BowlerMovement : MonoBehaviour
{
    public Transform bowlPosition;
    public Animator animator;
    public GameObject ball;
    public Transform releasePoint;
    public Transform target;

    private float totalRunTime = 4.0f; // Time until release
    private float timer = 0f;
    private Vector3 startPos;
    private bool moving = false;
    private bool ballReleased = false;

    // Ball launch settings
    private float flightDuration = 1.0f;
    private float bounceHeight = 1.5f;
    private bool bounced = false;
    private float ballTimer = 0f;
    private Vector3 bounceStart;
    private Vector3 bounceTarget;

    // Bowling type
    private bool isSpin = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Input to start bowling with selected type
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

        // Move the bowler
        if (moving)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / totalRunTime);
            transform.position = Vector3.Lerp(startPos, bowlPosition.position, t);

            if (t >= 1.0f)
            {
                moving = false;
                ReleaseBall(); // Trigger ball release
            }
        }

        // Move the ball if it has been released
        if (ballReleased)
        {
            ballTimer += Time.deltaTime;
            float t = Mathf.Clamp01(ballTimer / flightDuration);

            if (!bounced)
            {
                Vector3 pos = Vector3.Lerp(bounceStart, bounceTarget, t);
                pos.y += 4 * bounceHeight * t * (1 - t);
                ball.transform.position = pos;

                if (t >= 1.0f)
                {
                    // Start second half after bounce
                    bounced = true;
                    ballTimer = 0f;
                    bounceStart = ball.transform.position;
                    bounceTarget = target.position;
                    bounceHeight /= 2f;
                }
            }
            else
            {
                Vector3 pos = Vector3.Lerp(bounceStart, bounceTarget, t);
                pos.y += 4 * bounceHeight * t * (1 - t);
                ball.transform.position = pos;
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

        // Choose animation and parameters
        if (isSpin)
        {
            animator.Play("LeftArmOrthodoxSpinner");
            flightDuration = 1.5f;
            bounceHeight = 0.7f;
        }
        else
        {
            animator.Play("LeftArmFastBowler");
            flightDuration = 1.0f;
            bounceHeight = 1.5f;
        }
    }

    void ReleaseBall()
    {
        if (ball == null || target == null || releasePoint == null) return;

        ball.transform.parent = null;
        ball.transform.position = releasePoint.position;

        bounceStart = releasePoint.position;

        // First bounce position (midway)
        bounceTarget = Vector3.Lerp(releasePoint.position, target.position, 0.5f);
        bounceTarget.y = releasePoint.position.y;

        ballTimer = 0f;
        bounced = false;
        ballReleased = true;
    }
}
