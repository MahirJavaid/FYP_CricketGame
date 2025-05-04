using UnityEngine;

public class FielderGroundFix : MonoBehaviour
{
    public Animator animator;

    public void SnapToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up*0.5f, Vector3.down, out hit, 5f))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y;
            transform.position = newPosition;
        }
    }
}
