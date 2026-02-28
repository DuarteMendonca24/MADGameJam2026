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
        StartCoroutine(Wait(scene));
    }

    private IEnumerator Wait(string scene)
    {
        transition.SetActive(true);
        transitionController.PlayTransitionInAnimation();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }
}
