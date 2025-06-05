using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public static GameManager instance;
    public int coinsCollected = 0;
    public float distance = 0.0f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        distance = Mathf.Max(distance, player.transform.position.x); // only increase the distance, if went farther from last spot
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(0);
    }
}
