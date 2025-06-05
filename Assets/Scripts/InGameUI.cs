using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distance;
    [SerializeField] private TextMeshProUGUI coins;

    void Start()
    {
        InvokeRepeating("UpdateInfo", 0, 0.15f);
    }

    void UpdateInfo()
    {
        distance.text = GameManager.instance.distance.ToString("#,#") + "  m";
        coins.text = GameManager.instance.coinsCollected.ToString("#,#");
    }
}
