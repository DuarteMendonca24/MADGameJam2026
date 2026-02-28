using UnityEngine;

public class QuitButton : MonoBehaviour
{
    [SerializeField] private GameObject Popup;

    public void OpenPopup()
    {
        Popup.SetActive(true);
    }
}
