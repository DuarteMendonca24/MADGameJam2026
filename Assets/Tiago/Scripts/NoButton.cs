using UnityEngine;

public class NoButton : MonoBehaviour
{
    [SerializeField] private GameObject Popup;

    public void ClosePopup()
    {
        Popup.SetActive(false);
    }
}
