using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        animator.Play("BoomAnimation");
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
    public void EnableGameObject()
    {
        gameObject.SetActive(true);
    }
}
