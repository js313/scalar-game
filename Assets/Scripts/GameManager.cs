using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int coinsCollected = 0;
    
    private void Awake()
    {
        instance = this;
    }
}
