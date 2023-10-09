using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;


//This script is a clean powerful solution to a top-down movement player
public class Movement : MonoBehaviour
{
    PlayerInput input;
    //Public variables that wer can edit in the editor
    public float maxSpeed = 5; //Our max speed
    public float acceleration = 20; //How fast we accelerate
    public float deacceleration = 4; //brake power
    public float jumpPower;
    public float groundCheckLength;
    public float groundCheckDistance = 0.1f;
    public int amountOfJumps;

    public float dashStrength;
    float dashTimer;
    float attackTimer;
    public float attackCooldown;
    public float dashCooldown;

    float x; // Input
    //Private variables for internal logic

    float velocityX; //Our current velocity

    bool onGround = true;
    Rigidbody2D rb2D; //Ref to our rigidbody
    bool isDashing;
    public GameObject playerSprite;
    public Animator SwordAnimator;

    private void Start()
    {
        dashTimer += Time.deltaTime;
        input = GetComponent<PlayerInput>();
        Physics2D.queriesStartInColliders = false;
        //assign our ref.
        rb2D = GetComponent<Rigidbody2D>();
        var collider = GetComponent<Collider2D>();
        groundCheckLength = collider.bounds.size.y + groundCheckDistance;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        if (!isDashing)
        {

            MovementHorizontal();
            GravityCheck();
            GroundCheck();
            Flip();
        }


    }
    public void Flip()
    {
        if (x < 0)
        {
            playerSprite.transform.rotation = new(0, 180, 0, 0);
        }
        else
        {
            playerSprite.transform.rotation = new(0, 0, 0, 0);
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed() && dashTimer > dashCooldown)
        {
            isDashing = true;
            rb2D.velocity += new Vector2(x * dashStrength, 0);
            dashTimer = 0;
            Invoke("DashDone", 0.1f);
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed() && attackTimer > attackCooldown)
        {
            SwordAnimator.SetTrigger("Attack");

        }

    }
    public void DashDone()
    {
        isDashing = false;
    }

    private void GravityCheck()
    {
        if (rb2D.velocity.y < 0)
        {
            rb2D.gravityScale = 5;
        }
        else
        {
            rb2D.gravityScale = 1;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed() && amountOfJumps > 1)
        {
            amountOfJumps--;
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
        }


    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            maxSpeed = 5;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            maxSpeed = 15;
        }
    }
    private void GroundCheck()
    {
        if (onGround)
        {
            amountOfJumps = 2;
        }
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength);
    }

    public void MovementInput(InputAction.CallbackContext context)
    {

        x = context.ReadValue<float>();

    }

    private void MovementHorizontal()
    {
        velocityX += x * acceleration * Time.deltaTime;


        velocityX = Mathf.Clamp(velocityX, -maxSpeed, maxSpeed);



        if (x == 0 || (x < 0 == velocityX > 0))
        {

            velocityX *= 1 - deacceleration * Time.deltaTime;
        }

        rb2D.velocity = new Vector2(velocityX, rb2D.velocity.y);
        Debug.Log(x);
    }
}