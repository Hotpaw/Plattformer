
using UnityEngine;
using UnityEngine.InputSystem;


//This script is a clean powerful solution to a top-down movement player
public class Movement : MonoBehaviour
{

    //Public variables that wer can edit in the editor
    public float maxSpeed; //Our max speed
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

    public float leftRayCheck;
    public float rightRayCheck;
    float x; // Input
    //Private variables for internal logic
    public Vector2 rayDir;
    float velocityX; //Our current velocity


    public bool onGround = true;
    Rigidbody2D rb; //Ref to our rigidbody
    bool isDashing;
    public GameObject playerSprite;
    public Animator SwordAnimator;

    Collider2D col;
    public bool wallhitLeft;
    public bool wallhitRight;
    private void Start()
    {
        col = GetComponent<Collider2D>();
        dashTimer += Time.deltaTime;

        Physics2D.queriesStartInColliders = false;
        //assign our ref.
        rb = GetComponent<Rigidbody2D>();
        var collider = GetComponent<Collider2D>();

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
            //CornerCorrecting();
        }
        WallStuck();
        wallhitLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), -Vector2.right,leftRayCheck);
        wallhitRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), Vector2.right, rightRayCheck);


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
            rb.velocity += new Vector2(x * dashStrength, 0);
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
        if (rb.velocity.y < 3 && !onGround)
        {
            rb.gravityScale = 5;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.2f * Time.deltaTime);

        }
        else
        {
            rb.gravityScale = 1;


        }
    }

    public void Jump(InputAction.CallbackContext context)
    {


        if (!wallhitLeft && context.action.WasPressedThisFrame() && amountOfJumps > 1)
        {
            amountOfJumps--;

           
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);


        }
        else if (wallhitLeft || wallhitRight && context.action.WasPressedThisFrame())
        {
            amountOfJumps++;
            WallJump();


        }

    }
    public void WallStuck()
    {
        if (wallhitLeft)
        {

            rb.sharedMaterial.friction = 0.8f;

        }
        if (wallhitRight)
        {

            rb.sharedMaterial.friction = 0.8f;

        }

    }
    public void WallJump()
    {
        isDashing = true;
        amountOfJumps = 0;
        if (wallhitLeft)
        {
          
            rb.velocity += new Vector2(20, 10);
        }
        if(wallhitRight)
        {
            Debug.Log("RIGHT");
            rb.velocity += new Vector2(-20, 10);
        }
     
      
        dashTimer = 0;
        Invoke("DashDone", 0.1f);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void GroundCheck()
    {
        if (onGround)
        {
            amountOfJumps = 2;
        }
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength);
        Debug.DrawRay(transform.position, Vector2.down, Color.green, groundCheckLength);
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

        rb.velocity = new Vector2(velocityX, rb.velocity.y);

    }
   
   
}