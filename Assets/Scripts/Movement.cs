using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed when walking (Run = 1.5x)")]
    [SerializeField] private float speed = 5f;
    [Tooltip("Target jump height in Unity units")]
    [SerializeField] private float jumpHeight = 2f;

    [Header("Ground Detection")]
    [Tooltip("Distance below collider to check for ground")]
    [SerializeField] private float groundCheckDistance = 0.05f;

    private PlayerAction actions;
    private Animator animator;
    private Rigidbody2D body;
    private float jumpForce;
    private bool isGrounded;
    private bool hasJumped;

    private void Awake()
    {
        actions = new PlayerAction();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        actions.Movement.Jump.performed += OnJump;

        jumpForce = CalculateJumpForce(jumpHeight);
    }

    private float CalculateJumpForce(float targetHeight)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y * body.gravityScale);
        float velocity = Mathf.Sqrt(2 * targetHeight * gravity);
        return velocity * body.mass;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && !hasJumped)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            hasJumped = true;
            animator.SetBool("Jump", true);
        }
    }

    private void OnEnable()
    {
        actions.Movement.Enable();
    }

    private void OnDisable()
    {
        actions.Movement.Disable();
        body.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        float horizontal = actions.Movement.Horizontal.ReadValue<float>();
        bool isRunning = actions.Movement.Run.inProgress;

        HandleMovement(horizontal, isRunning);
    }

    private void HandleMovement(float horizontal, bool isRunning)
    {
        FlipSprite(horizontal);


        Vector2 velocity = body.velocity;
        velocity.x = horizontal * ( isGrounded ? (isRunning ? speed * 1.5f : speed) : speed / 1.5f);
        body.velocity = velocity;


        animator.SetBool("Run", isRunning);
        animator.SetFloat("Input x", Mathf.Abs(horizontal));
    }

    private void FlipSprite(float horizontal)
    {
        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = Vector3.one;
    }

    private void LateUpdate()
    {
        UpdateGroundedStatus();
    }

    private void UpdateGroundedStatus()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);
        bool wasGrounded = isGrounded;
        isGrounded = hit.collider != null;

        // Only reset jump when landing again
        if (!wasGrounded && isGrounded)
        {
            hasJumped = false;
            animator.SetBool("Jump", false);
        }

        animator.SetBool("isGrounded", isGrounded);
    }
}
