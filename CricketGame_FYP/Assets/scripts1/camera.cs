using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public GameObject batsman;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - batsman.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset + batsman.transform.position;
    }
}
