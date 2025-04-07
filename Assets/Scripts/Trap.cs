using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null )
        {
            player.Damage();
        }
    }
}
