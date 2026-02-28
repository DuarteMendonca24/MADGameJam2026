using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public Animator animator;

    public void PlayTransitionInAnimation()
    { 
        animator.SetTrigger("TransitionIn");
    }

    public void PlayTransitionOutAnimation()
    {
        animator.SetTrigger("TransitionOut");
    }
}
