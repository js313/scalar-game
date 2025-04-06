using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Coin coin;
    [SerializeField] private int minCoins;
    [SerializeField] private int maxCoins;
    private int totalCoins;
    private float coinWidth;

    private void Start()
    {
        sr = coin.gameObject.GetComponent<SpriteRenderer>();
        coinWidth = sr.bounds.size.x * 1.5f;
        totalCoins = Random.Range(minCoins, maxCoins);
        for (int i = 0; i < totalCoins; i++)
        {
            Vector3 offset = new((i - (float)totalCoins / 2) * coinWidth + coinWidth / 2, 0, 0);
            Instantiate(coin, transform.position + offset, Quaternion.identity, transform);
        }
    }
}
