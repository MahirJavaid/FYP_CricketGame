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
    private bool moving = true;
    private bool ballReleased = false;

    // Ball launch settings
    private float flightDuration = 1.0f;
    private float bounceHeight = 1.5f;
    private bool bounced = false;
    private float ballTimer = 0f;
    private Vector3 bounceStart;
    private Vector3 bounceTarget;

    void Start()
    {
        startPos = transform.position;
        animator.Play("LeftArmFastBowler");
    }

    void Update()
    {
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
                pos.y += 4 * bounceHeight * t * (1 - t); // Arc for bounce
                ball.transform.position = pos;

                if (t >= 1.0f)
                {
                    // Start second half after bounce
                    bounced = true;
                    ballTimer = 0f;
                    bounceStart = ball.transform.position;
                    bounceTarget = target.position;
                    bounceHeight /= 2f; // Lower bounce
                }
            }
            else
            {
                Vector3 pos = Vector3.Lerp(bounceStart, bounceTarget, t);
                pos.y += 4 * bounceHeight * t * (1 - t); // Second arc
                ball.transform.position = pos;
            }
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
        bounceTarget.y = releasePoint.position.y; // Make sure bounce lands on ground level

        ballTimer = 0f;
        bounced = false;
        ballReleased = true;
    }
}
