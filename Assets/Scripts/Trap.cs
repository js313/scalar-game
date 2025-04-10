using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int chanceToSpawn;

    private void Start()
    {
        if(chanceToSpawn > Random.Range(1, 101))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null )
        {
            player.Damage();
        }
    }
}
