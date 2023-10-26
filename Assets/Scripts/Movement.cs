
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;


//This script is a clean powerful solution to a top-down movement player
public class Movement : MonoBehaviour
{
    [Header("Move Variables")]
    public float maxSpeed; 
    public float acceleration = 20;
    public float deacceleration = 4;

    [Header("Jump Variables")]
    public float jumpPower;
    public float fallMultiplier;
    public float groundCheckLength;
    public float coyoteTime;
    public float jumpPressLeniancy;
    public float upperVerticalVelocityClamp;
    public float lowerVerticalVelocityClamp;
    public Vector2 groundCheckBoxSize;
    public LayerMask groundLayer;

    [Header("Dash Variables")]
    public float dashStrength;
    public float dashCooldown;
    private float dashTimer;

    [Header("Dead")]
    public bool dead;

    private float xInput;
    private float velocityX;
    private float timeSinceGrounded = 1;
    private float timeSinceJumpPressed = 1;
    private float gravityscale;
    private bool doubleJump;
    private bool isDashing;
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private Animator animator;

    private Vector2 vecGravity;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        gravityscale = rb.gravityScale;
    }

    void Update()
    {
        dashTimer += Time.deltaTime;
        timeSinceGrounded += Time.deltaTime;
        timeSinceJumpPressed += Time.deltaTime;

        if (!isDashing && !dead)
        {
            MovementHorizontal();
            GroundCheck();
            Flip();

            if (rb.velocity.y < 0 || !Gamepad.current.bButton.isPressed)
                rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;

            if (Mathf.Abs(rb.velocity.y) < 0.5f)
                rb.gravityScale = gravityscale / 4;
            else
                rb.gravityScale = gravityscale;

            if (timeSinceJumpPressed < jumpPressLeniancy && (timeSinceGrounded < coyoteTime || doubleJump))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);

                if (timeSinceGrounded > coyoteTime)
                    doubleJump = false;

                animator.SetTrigger("Jump");
            }
            RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.up, groundCheckLength + 0.1f, groundLayer);

            if (ray && ray.collider.gameObject.CompareTag("Ice"))
                deacceleration = 0;
            else
                deacceleration = 15;
        }

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -lowerVerticalVelocityClamp, upperVerticalVelocityClamp));
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VerticalSpeed", rb.velocity.y);
        animator.SetBool("Dead", dead);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            Die();
        }
    }

    public void Flip()
    {
        if (xInput < -0.1f)
            playerSprite.flipX = true;
        else if (xInput > 0.1f)
            playerSprite.flipX = false;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed() && dashTimer > dashCooldown && !dead)
        {
            int i;

            if (playerSprite.flipX)
                i = -1;
            else
                i = 1;

            isDashing = true;
            rb.velocity = new Vector2(i * dashStrength, 0);
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            dashTimer = 0;
            Invoke("DashDone", 0.2f);
        }
    }

    public void DashDone()
    {
        isDashing = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            timeSinceJumpPressed = 0;
        }
    }

    private void GroundCheck()
    {
        bool isGrounded = Physics2D.BoxCast(transform.position, groundCheckBoxSize, 0, -transform.up, groundCheckLength, groundLayer);
        if (isGrounded)
        {
            doubleJump = true;
            timeSinceGrounded = 0;
        }

        animator.SetBool("Grounded", isGrounded);
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        xInput = context.ReadValue<float>();
    }

    private void MovementHorizontal()
    {
        velocityX += xInput * acceleration * Time.deltaTime;

        velocityX = Mathf.Clamp(velocityX, -maxSpeed, maxSpeed);

        if (xInput == 0 || (xInput < 0 == velocityX > 0))
        {
            velocityX *= 1 - deacceleration * Time.deltaTime;
        }

        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    public void Die()
    {
        rb.velocity = new Vector2(0, 5);
        animator.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        dead = true;
        SceneChanger.instance.TransitionToNewScene(SceneManager.GetActiveScene().name);
    }
}
