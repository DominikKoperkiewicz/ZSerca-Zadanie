using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D entityRigidbody;
    public Ladder targetLadder;

    [SerializeField] private PlayerInputActions playerInputActions;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float climbSpeed = 2.6f;

    private bool isFlipped = false;
    private bool isClimbing = false;

    public bool IsClimbing
    {
        get => isClimbing;
        set
        {
            if (isClimbing != value) 
            {
                isClimbing = value;
                animator.SetBool("isClimbing", isClimbing);
            }
        }
    }


    private void FixedUpdate()
    {
        if (IsClimbing) {
            ClimbExitCheck();
        }
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    private void OnDestroy()
    {
        playerInputActions.Dispose();
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
        if (IsClimbing) return;

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
        if (IsClimbing || targetLadder == null) return;

        float inputAxis = context.ReadValue<float>();
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

    private void ClimbExitCheck()
    {
        if (!IsClimbing || targetLadder == null) { return; }

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

    public void SnapToPosition(Vector3 targetPosition)
    {
        Vector3 snapPosition = targetPosition;
        snapPosition.z = this.transform.position.z;

        this.transform.position = snapPosition;
    }

    public void SetFlip(bool FlipValue)
    {
        if (isFlipped == FlipValue) return;

        isFlipped = FlipValue;
        Vector3 animatorCurrentScale = animator.transform.localScale;
        animatorCurrentScale.x *= -1;
        animator.transform.localScale = animatorCurrentScale;
    }

    public void Cough() 
    {
        animator.SetTrigger("Cough");
    }
}
