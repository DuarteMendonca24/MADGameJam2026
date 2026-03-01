using System.Collections;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public Animator animator;

    public IEnumerator PlayTransitionInAnimation()
    { 
        animator.SetTrigger("TransitionIn");
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator PlayTransitionOutAnimation()
    {
        animator.SetTrigger("TransitionOut");
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator PlayFullTransition()
    {
        gameObject.SetActive(true);

        yield return PlayTransitionOutAnimation();
        yield return PlayTransitionInAnimation();

        gameObject.SetActive(false);
    }
}
