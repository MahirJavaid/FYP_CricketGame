using UnityEngine;

public class FielderThrowTrigger : MonoBehaviour
{
    public string ballTag = "ball";         // Tag for the ball
    public float detectionRadius = 3f;      // Radius to detect the ball
    public string throwTriggerName = "throw"; // Animator trigger name

    private Animator animator;
    private Transform ballTransform;

    private bool hasThrown = false; // To prevent repeated triggering

    void Start()
    {
        animator = GetComponent<Animator>();

        // Find ball by tag
        GameObject ball = GameObject.FindGameObjectWithTag(ballTag);
        if (ball != null)
        {
            ballTransform = ball.transform;
        }
        else
        {
            Debug.LogError("Ball with tag ball not found in scene!");
        }
    }

    void Update()
    {
        if (ballTransform == null || hasThrown)
            return;

        float distanceToBall = Vector3.Distance(transform.position, ballTransform.position);
        if (distanceToBall <= detectionRadius)
        {
            animator.SetTrigger("throw");
            hasThrown = true; // Prevent repeat trigger, remove if you want looping
        }
    }
}
