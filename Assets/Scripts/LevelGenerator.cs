using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Platform[] platformTypes;
    [SerializeField] float spawnDistanceThreshold;
    [SerializeField] float destroyDistanceThreshold;

    Player player;
    Platform lastPlatform;

    void Start()
    {
        player = FindFirstObjectByType<Player>().GetComponent<Player>();
    }

    void Update()
    {
        if (lastPlatform == null || lastPlatform.GetStartPoint().x - player.transform.position.x <= spawnDistanceThreshold)
        {
            GeneratePlatform();
        }
        DestroyUnusedPlatform();
    }

    void GeneratePlatform()
    {
        if (!player) return;
        Platform randomPlatformType = platformTypes[Random.Range(0, platformTypes.Length)];
        if (randomPlatformType == null) return;

        Vector3 spawnPoint = (lastPlatform == null) ? Vector3.zero : lastPlatform.GetEndPoint() + (randomPlatformType.transform.position - randomPlatformType.GetStartPoint());
        Platform nextPlatform = Instantiate(randomPlatformType, spawnPoint, Quaternion.identity);
        nextPlatform.transform.SetParent(transform);
        lastPlatform = nextPlatform;

    }

    void DestroyUnusedPlatform()
    {
        Transform platformToDelete = transform.GetChild(0);
        if (platformToDelete != null && player != null)
        {
            if (player.transform.position.x - platformToDelete.position.x >= destroyDistanceThreshold) Destroy(platformToDelete.gameObject);
        }
    }
}
