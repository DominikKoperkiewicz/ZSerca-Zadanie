using Spine.Unity;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D entityRigidbody;

    [SerializeField] private PlayerInputActions playerInputActions;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float climbSpeed = 3.0f;

    private bool isClimbing = false;
    public Ladder targetLadder;

    public bool IsClimbing
    {
        get => isClimbing;
        set
        {
            if (isClimbing != value) // Only update if the value changes
            {
                isClimbing = value;
                animator.SetBool("isClimbing", isClimbing);
            }
        }
    }

    [Space(25)]
    [Header("Animations")]
    private bool isFliped = false;
    [SerializeField] private AnimationReferenceAsset idleAnimation;
    [SerializeField] private AnimationReferenceAsset walkAnimation;


    private Vector2 moveDirection;

    private void FixedUpdate()
    {
        //Debug.Log("IsClimbing: " + IsClimbing.ToString());
        //Debug.Log("targetLadder: ", null);
        if (IsClimbing) {
            ClimbCheck();
        }
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Movement.performed += Move;
        playerInputActions.Player.Movement.canceled += Move;
        playerInputActions.Player.Climb.started += ClimbStart;
        playerInputActions.Player.Climb.performed += Climb;
        playerInputActions.Player.Climb.canceled += Climb;
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.Movement.performed -= Move;
        playerInputActions.Player.Movement.canceled -= Move;
        playerInputActions.Player.Climb.started -= ClimbStart;
        playerInputActions.Player.Climb.performed -= Climb;
        playerInputActions.Player.Climb.canceled -= Climb;
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (IsClimbing) { return; }

        float horizontalVelocity = context.ReadValue<float>() * movementSpeed;

        entityRigidbody.linearVelocityX = horizontalVelocity;

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

    private void Climb(InputAction.CallbackContext context)
    {
        float inputAxis = context.ReadValue<float>();
        /*
        if(!IsClimbing && targetLadder != null )
        {
            Transform nearestExitPoint = targetLadder.GetNearestExitPoint(this.transform);

            if (nearestExitPoint == targetLadder.BottomExitPoint && inputAxis < 0 || 
                nearestExitPoint == targetLadder.UpperExitPoint && inputAxis > 0) 
            {
                return;
            }

            IsClimbing = true;
            float initialPositionZ = this.transform.position.z;

            this.transform.position = nearestExitPoint.position;
            //this.transform.position = initialPositionZ;

        }*/

        if (IsClimbing)
        {
            float verticalVelocity = inputAxis * climbSpeed;

            entityRigidbody.linearVelocityX = 0;
            entityRigidbody.linearVelocityY = verticalVelocity;

            if (verticalVelocity != 0)
            {
                animator.speed = 1.0f;
            }
            else
            {
                animator.speed = 0.0f;
            }
        }

    }

    private void ClimbStart(InputAction.CallbackContext context) 
    {
        float inputAxis = context.ReadValue<float>();

        if (!IsClimbing && targetLadder != null)
        {
            Transform nearestExitPoint = targetLadder.GetNearestExitPoint(this.transform);

            if (nearestExitPoint == targetLadder.BottomExitPoint && inputAxis < 0 ||
                nearestExitPoint == targetLadder.UpperExitPoint && inputAxis > 0)
            {
                return;
            }

            IsClimbing = true;
            animator.SetBool("isWalking", false);

            SnapToPosition(nearestExitPoint.position);

        }
    }

    private void ClimbCheck()
    {

        if (IsClimbing && targetLadder != null)
        {
            if (this.transform.position.y < targetLadder.BottomExitPoint.position.y)
            {
                IsClimbing = false;
                entityRigidbody.linearVelocityY = 0;
                animator.speed = 1.0f;
                SnapToPosition(targetLadder.BottomExitPoint.position);
            }

            if (this.transform.position.y > targetLadder.UpperExitPoint.position.y)
            {
                IsClimbing = false;
                entityRigidbody.linearVelocityY = 0;
                animator.speed = 1.0f;
                SnapToPosition(targetLadder.UpperExitPoint.position);
            }
        }
    }

    private void SnapToPosition(Vector3 targetPosition)
    {
        Vector3 snapPosition = targetPosition;
        snapPosition.z = this.transform.position.z;

        this.transform.position = snapPosition;
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
