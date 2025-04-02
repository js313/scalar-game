using Unity.VisualScripting;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    Player player;
    [SerializeField] private float ledgeCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool canCheck = true;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (canCheck)
        {
            RaycastHit2D isLedge = Physics2D.CircleCast(transform.position, ledgeCheckRadius, Vector2.zero, 0, whatIsGround);
            player.SetIsLedge(isLedge);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canCheck = false;
            player.SetIsLedge(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canCheck = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ledgeCheckRadius);
    }
}
