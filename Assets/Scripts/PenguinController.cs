using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PenguinController : MonoBehaviour
{
    public float speed = 5f;
    public float smoothTime = 1f;

    public float diveForceMultiplier = 2.5f;
    public float diveAcceleration = 20f;
    public float swimForce = 5f;
    public float swimSpeed = 3f;

    public Rigidbody2D rb;
    public TMP_Text scoreText;
    private int score = 0;
    private float xVelocityRef = 0;
    private float airTime;
    private Vector2 collisionVelocity;

    public enum PenguinState {Sliding, Swimming, Gliding}
    public PenguinState currentState = PenguinState.Sliding;
    private Animator animator;
    public AudioSource splash;
    public AudioSource swim;
    public AudioSource exitWater;

    void Start()
    {
        airTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (currentState == PenguinState.Sliding && Time.time - airTime > 0.5f)
        {
            currentState = PenguinState.Gliding;
            animator.SetBool("isGliding", true);
        }

        if (currentState == PenguinState.Sliding)
        {
            HandleSliding();
        }
        else if (currentState == PenguinState.Swimming)
        {
            HandleSwimming();
        }
        else
        {
            HandleGliding();
        }
        collisionVelocity = rb.linearVelocity;
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

    // Called when gliding
    void HandleGliding()
    {
        //Space to descend
        if(Keyboard.current.spaceKey.isPressed)
        {
            rb.AddForce(Vector2.down * swimForce, ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentState = PenguinState.Swimming;
            animator.SetBool("isSwimming", true);
            animator.SetBool("isGliding", false);
            splash.Play();
            swim.Play();
        }

        if (other.CompareTag("Fish"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("fishCollect");
            score++;
            scoreText.SetText("Score: " + score);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentState = PenguinState.Sliding;
            animator.SetBool("isSwimming", false);
            splash.Stop();
            swim.Stop();
            exitWater.Play();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // if penguin lands on ice
        if (collision.gameObject.CompareTag("Ice") && currentState == PenguinState.Gliding)
        {
            // calculate landing angle
            Vector2 normal = collision.GetContact(0).normal;
            float impactAngle = Vector2.Angle(collisionVelocity, -normal);
            
            animator.SetBool("isGliding", false);
            airTime = Time.time;
            currentState = PenguinState.Sliding;

            // for debug
            print("v: " + collisionVelocity);
            print("n: " + normal);
            print("Impact angle: " + impactAngle);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            airTime = Time.time;
        }
    }
}
