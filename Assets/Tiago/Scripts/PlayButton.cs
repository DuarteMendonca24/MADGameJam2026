using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject transition;
    private TransitionController transitionController;

    private void Start()
    {
        transitionController = transition.GetComponent<TransitionController>();
    }

    public void ChangeScene(string scene)
    {
        transition.SetActive(true);
        transitionController.PlayTransitionInAnimation();
        Wait(2f);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator Wait(float secs)
    {
        yield return new WaitForSeconds(secs);
    }
}
