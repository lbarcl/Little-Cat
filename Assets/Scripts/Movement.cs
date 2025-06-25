using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Tooltip("The height that object will reach. Its used to callculate the force.")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float speed;
    private float jumpForce;
    private bool isGrounded;
    private bool isJumped;

    PlayerAction actions;
    Animator animator;
    Rigidbody2D body;
    Vector3 subPos;

    void Awake()
    {
        actions = new PlayerAction();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        actions.Movement.Jump.performed += Jump;

        float velocity = Mathf.Sqrt(jumpHeight * 2 * (Mathf.Abs(Physics2D.gravity.magnitude) * body.gravityScale));
        jumpForce = body.mass * velocity;
        subPos = new Vector3(0, transform.localScale.y, 0);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && !isJumped)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Magic number
            isJumped = true;
            animator.SetBool("Jump", true);
        }
    }

    void OnEnable()
    {
        actions.Movement.Enable();
    }

    void OnDisable()
    {
        actions.Movement.Disable();
    }

    void FixedUpdate()
    {
        float horizontal = actions.Movement.Horizontal.ReadValue<float>();
        bool isRuning = actions.Movement.Run.inProgress;

        if (horizontal > 0 && transform.localScale.x == 1)
            transform.localScale = new Vector2(-1, 1);
        else if (horizontal < 0)
            transform.localScale = Vector2.one;

        if (isGrounded)
        {
            Vector2 velocity = body.velocity;
            velocity.x = horizontal * (isRuning ? speed * 1.5f : speed);
            body.velocity = velocity;
        }

        animator.SetBool("Run", isRuning);
        animator.SetFloat("Input x", Mathf.Abs(horizontal));
    }

    void LateUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - subPos, transform.up * -1, .05f);
        isGrounded = hit.collider != null;

        isJumped = !(isJumped && isGrounded);
        animator.SetBool("isGrounded", isGrounded);
    }
}
