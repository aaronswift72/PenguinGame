using UnityEngine;
using UnityEngine.InputSystem;

public class PenguinController : MonoBehaviour
{
    public float speed = 5f;
    public float smoothTime = 1f;

    public float diveForceMultiplier = 2.5f;
    public float diveAcceleration = 20f;
    public float swimForce = 5f;
    public float swimSpeed = 3f;

    public Rigidbody2D rb;
    private float xVelocityRef = 0;

    public enum PenguinState {Sliding, Swimming}
    public PenguinState currentState = PenguinState.Sliding;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (currentState == PenguinState.Sliding)
        {
            HandleSliding();
        }
        else
        {
            HandleSwimming();
        }
    }
    
    //Called when in land
    void HandleSliding()
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

    //Called when swimming
    void HandleSwimming()
    {
        //Makes penguin float
        rb.AddForce(Vector2.up * Physics2D.gravity.magnitude * rb.mass, ForceMode2D.Force);

        //Slows horizontal movement
        rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, swimSpeed, Time.fixedDeltaTime * 3f), rb.linearVelocity.y);

        //Space to swim up
        if(Keyboard.current.spaceKey.isPressed)
        {
            rb.AddForce(Vector2.up * swimForce, ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentState = PenguinState.Swimming;
            animator.SetBool("isSwimming", true);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentState = PenguinState.Sliding;
            animator.SetBool("isSwimming", false);
        }
    }
}
