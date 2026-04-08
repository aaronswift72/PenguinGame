using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 levelPos;
    void Start()
    {
        GetComponent<Camera>().orthographicSize = 3;
        transform.position = startPos;
    }
    public void SetUp()
    {
        GetComponent<Camera>().orthographicSize = 5;
        transform.position = levelPos;
    }
}
