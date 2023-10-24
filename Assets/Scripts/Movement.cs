
using UnityEngine;
using UnityEngine.InputSystem;
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

    private float xInput;
    private float velocityX;

    private float timeSinceGrounded = 1;
    private float timeSinceJumpPressed = 1;
    private bool doubleJump;
    private bool isDashing;
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;

    public bool wallhitLeft;
    public bool wallhitRight;

    private Vector2 vecGravity;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
    }

    void Update()
    {
        dashTimer += Time.deltaTime;
        timeSinceGrounded += Time.deltaTime;
        timeSinceJumpPressed += Time.deltaTime;

        if (!isDashing)
        {
            MovementHorizontal();
            GroundCheck();
            Flip();

            if (rb.velocity.y < 0 || !Gamepad.current.bButton.isPressed)
                rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;

            if (Mathf.Abs(rb.velocity.y) < 0.1f)
                rb.gravityScale = 0.5f;
            else
                rb.gravityScale = 1f;

            if (timeSinceJumpPressed < jumpPressLeniancy && (timeSinceGrounded < coyoteTime || doubleJump))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);

                if (timeSinceGrounded > coyoteTime)
                    doubleJump = false;
            }
        }

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -lowerVerticalVelocityClamp, upperVerticalVelocityClamp));
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
        if (context.action.IsPressed() && dashTimer > dashCooldown)
        {
            int i;

            if (playerSprite.flipX)
                i = -1;
            else
                i = 1;

            isDashing = true;
            rb.velocity = new Vector2(i * dashStrength, 0);
            dashTimer = 0;
            Invoke("DashDone", 0.2f);
        }
    }

    public void DashDone()
    {
        isDashing = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!wallhitLeft && context.action.WasPressedThisFrame())
        {
            timeSinceJumpPressed = 0;
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, groundCheckBoxSize, 0, -transform.up, groundCheckLength, groundLayer))
        {
            doubleJump = true;
            timeSinceGrounded = 0;
        }
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
}
