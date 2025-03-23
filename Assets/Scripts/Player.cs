using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private bool runBegun = false;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isOnGround = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (runBegun)
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, groundCheckDistance, whatIsGround);
        isOnGround = hit;
        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetFloat("yVelocity", rb.linearVelocityY);

        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            runBegun = true;

        if (Input.GetButtonDown("Jump") && isOnGround)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
