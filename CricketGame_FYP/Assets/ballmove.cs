using UnityEngine;

public class BallController : MonoBehaviour
{
    private Vector3 target;
    private bool launched = false;
    private float flightDuration = 1.0f;
    private float timer = 0f;
    private Vector3 startPos;
    private float height = 1.5f;
    private bool bounced = false;

    public void Launch(Vector3 targetPosition)
    {
        startPos = transform.position;
        target = targetPosition;
        timer = 0f;
        launched = true;
    }

    void Update()
    {
        if (!launched) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / flightDuration);

        // Simulate arc with bounce at midpoint
        Vector3 currentPos = Vector3.Lerp(startPos, target, t);
        float arc = 4 * height * t * (1 - t); // simple parabola

        currentPos.y += arc;
        transform.position = currentPos;

        if (t >= 1.0f && !bounced)
        {
            // Bounce: simulate second arc with lower height
            bounced = true;
            timer = 0f;
            startPos = transform.position;
            target.y = transform.position.y; // flat ground after bounce
            height /= 2f;
        }
    }
}
