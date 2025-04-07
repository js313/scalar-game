using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int coinsCollected = 0;
    
    private void Awake()
    {
        instance = this;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(0);
    }
}
