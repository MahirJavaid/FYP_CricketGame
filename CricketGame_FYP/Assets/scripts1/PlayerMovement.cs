using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement
    private Animator animator; // Reference to Animator component

    void Start()
    {
        animator = GetComponent<Animator>(); // Get Animator component
    }

    void Update()
    {
        // Get input from arrow keys or WASD
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Move the player
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Rotate player towards movement direction if moving
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            animator.SetBool("isRunning", true); // 👉 Trigger running animation
        }
        else
        {
            animator.SetBool("isRunning", false); // 👉 Stop animation when idle
        }
    }
}
