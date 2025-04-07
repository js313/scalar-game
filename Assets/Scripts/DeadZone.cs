using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.ResetLevel();
    }
}
