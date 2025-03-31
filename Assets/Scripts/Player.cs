using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;
    private float jumpForce;
    [SerializeField] private float defaultJumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool runBegun = false;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isOnGround = false;
    private bool canDoubleJump = false;
    private bool isFacingWall = false;

    [Header("Slide Info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCooldownTimer;
    private float slideTimerCounter;
    private float slideCooldownTimerCounter;
    private bool isSliding = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        jumpForce = defaultJumpForce;
    }

    private void Update()
    {
        if (runBegun && !isFacingWall)
            HandleMove();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownTimerCounter -= Time.deltaTime;

        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector3.down, groundCheckDistance, whatIsGround);
        isOnGround = groundHit;
        if(isOnGround) canDoubleJump = true;

        RaycastHit2D wallHit = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        isFacingWall = wallHit;

        AnimatorControllers();

        CheckInput();
    }

    private void AnimatorControllers()
    {
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);

        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            runBegun = true;

        if (Input.GetButtonDown("Jump"))
            HandleJump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            HandleSlide();
    }

    private void HandleJump()
    {
        if (isOnGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = true;
            jumpForce = doubleJumpForce;
        }
        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
            jumpForce = defaultJumpForce;
        }
    }

    private void HandleMove()
    {
        // When under a "roof" the box cast keeps detecting a wall and never calls "HandleMove" so we keep sliding
        // And as we are resetting the "isSliding" flag in here it gives us the desired output
        if (isOnGround && isSliding && slideTimerCounter >= 0)
        {
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            isSliding = false;
        }
    }

    private void HandleSlide()
    {
        if (rb.linearVelocity.x > 0 && slideCooldownTimerCounter < 0)
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCooldownTimerCounter = slideCooldownTimer;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
