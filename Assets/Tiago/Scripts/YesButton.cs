using UnityEngine;

public class YesButton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
