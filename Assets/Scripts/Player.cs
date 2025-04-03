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
    private bool isLedge = false;

    [Header("Slide Info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCooldownTimer;
    private float slideTimerCounter;
    private float slideCooldownTimerCounter;
    private bool isSliding = false;

    [Header("Ledge Climb Info")]
    [SerializeField] Transform endPosition;
    private float rbGravityScale;

    [Header("Speed Control Info")]
    [SerializeField] private float speedIncreaseMultiplier;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float milestoneDistance;
    private float milestone;
    private float defaultMoveSpeed;
    private float defaultMilestoneDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        jumpForce = defaultJumpForce;
        rbGravityScale = rb.gravityScale;
        milestone = milestoneDistance;
        defaultMilestoneDistance = milestoneDistance;
        defaultMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (runBegun && !isFacingWall)
            HandleMove();
        else if (isFacingWall)
            ResetSpeed();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownTimerCounter -= Time.deltaTime;

        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector3.down, groundCheckDistance, whatIsGround);
        isOnGround = groundHit;
        if (isOnGround) canDoubleJump = true;

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
        if(rb.linearVelocityY <= -20 && !isLedge) anim.SetBool("canRoll", true);

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
        SpeedModifier();
    }

    private void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        milestoneDistance = defaultMilestoneDistance;
    }

    private void SpeedModifier()
    {
        if (maxSpeed > moveSpeed && transform.position.x >= milestone)
        {
            moveSpeed *= speedIncreaseMultiplier;
            if (moveSpeed > maxSpeed) moveSpeed = maxSpeed;
            milestoneDistance *= speedIncreaseMultiplier;
            milestone += milestoneDistance;
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

    private void LedgeClimbAnimationStart()
    {
        isOnGround = false;
        canDoubleJump = false;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("isLedge");
    }

    public void LedgeClimbAnimationEnd()
    {
        isOnGround = true;
        canDoubleJump = true;
        rb.gravityScale = rbGravityScale;
        transform.position = endPosition.position;
    }

    public void SetIsLedge(bool isLedge)
    {
        if (this.isLedge == isLedge) return;
        this.isLedge = isLedge;
        if (this.isLedge) LedgeClimbAnimationStart();
    }

    public void RollAnimationEnd() => anim.SetBool("canRoll", false);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
