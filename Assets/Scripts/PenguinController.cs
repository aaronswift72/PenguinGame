using UnityEngine;
using UnityEngine.InputSystem;

public class PenguinController : MonoBehaviour
{
    public float speed = 5f;
    public float smoothTime = 1f;

    public float diveForceMultiplier = 2.5f;
    public float diveAcceleration = 20f;

    public Rigidbody2D rb;
    private float xVelocityRef = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        float smoothedX = Mathf.SmoothDamp(rb.linearVelocity.x, speed, ref xVelocityRef, smoothTime);
        rb.linearVelocity = new Vector2(smoothedX, rb.linearVelocity.y);
        //Vector2 currentVelocity = rb.linearVelocity;
        //Vector2 targetVelocity = new Vector2(speed, currentVelocity.y);

        if (Keyboard.current.spaceKey.isPressed)
        {
            float downwardBoost = Mathf.Abs(rb.linearVelocity.x) * diveForceMultiplier;
            rb.AddForce(Vector2.down * downwardBoost * diveAcceleration * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }
}
