using UnityEngine;

public class PenguinController : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    private Vector2 velocityRef = Vector2.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 targetVelocity = new Vector2(speed, currentVelocity.y);
        rb.linearVelocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref velocityRef, 1f);
    }
}
