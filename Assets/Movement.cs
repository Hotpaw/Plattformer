using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

//This script is a clean powerful solution to a top-down movement player
public class Movement : MonoBehaviour
{
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
    public float dashCooldown;

    float x; // Input
    //Private variables for internal logic

    float velocityX; //Our current velocity

    bool onGround = true;
    Rigidbody2D rb2D; //Ref to our rigidbody

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        //assign our ref.
        rb2D = GetComponent<Rigidbody2D>();
        var collider = GetComponent<Collider2D>();
        groundCheckLength = collider.bounds.size.y + groundCheckDistance;
    }

    void Update()
    {
        MovementX();
        Jump();
        GravityCheck();

        dashTimer += Time.deltaTime;
        
        if(Input.GetKey(KeyCode.LeftShift) && dashTimer > dashCooldown)
        {
            rb2D.velocity = new Vector2(x * dashStrength, rb2D.velocity.y);
            dashTimer = 0;
        }
      
    }

    private void GravityCheck()
    {
        if (rb2D.velocity.y < 0)
        {
            rb2D.gravityScale = 4;
        }
        else
        {
            rb2D.gravityScale = 1;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && amountOfJumps > 1)
        {
            amountOfJumps--;
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
        }
        // HOLD TO JUMP
        //if (Input.GetButtonDown("Jump") && rb2D.velocity.y > 0)
        //{
        //    
        //    rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.25f);
        //}
        if (onGround)
        {
            amountOfJumps = 2;
        }
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength);
    }

    private void MovementX()
    {

        x = Input.GetAxisRaw("Horizontal");

        velocityX += x * acceleration * Time.deltaTime;


        velocityX = Mathf.Clamp(velocityX, -maxSpeed, maxSpeed);


        if (x == 0 || (x < 0 == velocityX > 0))
        {
           
            velocityX *= 1 - deacceleration * Time.deltaTime;
        }


        rb2D.velocity = new Vector2(velocityX, rb2D.velocity.y);
    }
}