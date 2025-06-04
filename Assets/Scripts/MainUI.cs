using UnityEngine;

public class MainUI : MonoBehaviour
{
    public void SwitchUITo(GameObject ui)
    {
        for(int i=0;i< transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        ui.SetActive(true);
    }
}
