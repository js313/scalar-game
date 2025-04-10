using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

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

    [Header("Knockback Info")]
    [SerializeField] private Vector2 knockbackDir;
    [SerializeField] private float invincibilityTime;
    private bool canBeKnocked = true;
    private bool isKnocked;

    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        jumpForce = defaultJumpForce;
        rbGravityScale = rb.gravityScale;
        milestone = milestoneDistance;
        defaultMilestoneDistance = transform.position.x + milestoneDistance;
        defaultMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        AnimatorControllers();

        if (isKnocked || isDead || isLedge) return;

        if (runBegun && (!isFacingWall || isSliding))
            HandleMove();
        else if (isFacingWall && !isSliding)
            ResetSpeed();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownTimerCounter -= Time.deltaTime;

        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector3.down, groundCheckDistance, whatIsGround);
        isOnGround = groundHit;
        if (isOnGround) canDoubleJump = true;

        RaycastHit2D wallHit = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        isFacingWall = wallHit;

        CheckInput();
    }

    private void AnimatorControllers()
    {
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
        if (rb.linearVelocityY <= -20 && !isLedge) anim.SetBool("canRoll", true);
        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isDead", isDead);

        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            runBegun = true;

        if (Input.GetButtonDown("Jump") && runBegun)
            HandleJump();

        if (Input.GetKeyDown(KeyCode.LeftShift) && runBegun)
            HandleSlide();
    }

    private void HandleJump()
    {
        if (isOnGround)
        {
            jumpForce = defaultJumpForce;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            jumpForce = doubleJumpForce;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
        }
    }

    private void HandleMove()
    {
        if (isOnGround && isSliding && slideTimerCounter >= 0 && rb.linearVelocityX > 0 || (isSliding && isFacingWall) || (rb.linearVelocityX == 0 && isFacingWall))
        {
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocity.y);
        }
        else if(!isFacingWall)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            isSliding = false;
        }
        if(rb.linearVelocityX > 0) SpeedModifier();
    }

    private void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        milestoneDistance = defaultMilestoneDistance;
        milestone = transform.position.x;
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
        if (slideCooldownTimerCounter < 0 && rb.linearVelocityX > 0 || (rb.linearVelocityX == 0 && isFacingWall))
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCooldownTimerCounter = slideCooldownTimer;
        }
    }

    private void KnockbackStart()
    {
        if (!canBeKnocked) return;
        isKnocked = true;
        ResetSpeed();
        rb.linearVelocity = knockbackDir;
        StartCoroutine(InvincibilityRoutine());
        StartCoroutine(AnimateInvincibility());
    }

    public void KnockbackEnd()
    {
        isKnocked = false;
    }

    private IEnumerator InvincibilityRoutine()
    {
        canBeKnocked = false;
        yield return new WaitForSeconds(invincibilityTime);
        canBeKnocked = true;
    }

    private IEnumerator AnimateInvincibility()
    {
        float percent = 0;
        Color originalColor = sr.color;
        Color transparentColor = new(originalColor.r, originalColor.g, originalColor.b, 0.1f);
        float blinkDuration = invincibilityTime / 10;
        while (percent <= 1)
        {
            sr.color = Color.Lerp(transparentColor, originalColor, Mathf.PingPong(Time.time / blinkDuration, 1));
            percent += Time.deltaTime / invincibilityTime;
            yield return null;
        }
        sr.color = originalColor;
    }

    private IEnumerator Die()
    {
        isDead = true;
        rb.linearVelocity = knockbackDir;
        yield return new WaitForSeconds(0.5f);
        rb.linearVelocity *= 0;
        yield return new WaitForSeconds(1f);
        GameManager.instance.ResetLevel();
    }

    // BUG: Ledge Climb feature is very buggy and needs a major fix
    private void LedgeClimbAnimationStart()
    {
        isOnGround = false;
        canDoubleJump = false;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isLedge", true);
    }

    public void LedgeClimbAnimationEnd()
    {
        isOnGround = true;
        canDoubleJump = true;
        rb.gravityScale = rbGravityScale;
        transform.position = endPosition.position;
        anim.SetBool("isLedge", false);
    }

    public void SetIsLedge(bool isLedge)
    {
        if (this.isLedge == isLedge || !isFacingWall) return;
        this.isLedge = isLedge;
        if (this.isLedge) LedgeClimbAnimationStart();
    }

    public void RollAnimationEnd() => anim.SetBool("canRoll", false);

    public void Damage()
    {
        if (moveSpeed >= maxSpeed)
            KnockbackStart();
        else
            StartCoroutine(Die());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
