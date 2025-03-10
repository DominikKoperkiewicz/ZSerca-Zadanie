using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D entityRigidbody;

    [SerializeField] private PlayerInputActions playerInputActions;
    //[SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private float movementSpeed = 5.0f;

    [Header("Animations")]
    private bool isFliped = false;
    [SerializeField] private AnimationReferenceAsset idleAnimation;
    [SerializeField] private AnimationReferenceAsset walkAnimation;


    private Vector2 moveDirection;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Movement.performed += Move;
        playerInputActions.Player.Movement.canceled += Move;
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.Movement.performed -= Move;
        playerInputActions.Player.Movement.canceled -= Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        float horizontalVelocity = context.ReadValue<float>() * movementSpeed;

        entityRigidbody.linearVelocityX = horizontalVelocity;
        //skeletonAnimation.AnimationState.SetAnimation(0, walkAnimation, true);
        //skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0f);

        if (horizontalVelocity < 0)
        {
            animator.SetBool("isWalking", true);
            SetFlip(true);
        }
        else if (horizontalVelocity > 0) 
        {
            animator.SetBool("isWalking", true);
            SetFlip(false);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void SetFlip(bool FlipValue)
    {
        if (isFliped != FlipValue) 
        {
            isFliped = !isFliped;
            Vector3 animatorCurrentScale = animator.transform.localScale;
            animatorCurrentScale.x *= -1;
            animator.transform.localScale = animatorCurrentScale;
        }
    }

    public void Cough() 
    {
        animator.SetTrigger("Cough");
    }
}
