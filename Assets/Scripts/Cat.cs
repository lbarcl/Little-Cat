using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cat : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;
    PlayerAction actions;
    Rigidbody2D body2D;
    Animator animator;

    void Awake()
    {
        actions = new PlayerAction();
        actions.Interractions.Enable();

        actions.Interractions.Lick.performed += Lick;
        actions.Interractions.Sleep.performed += Sleep;
    }

    private void Sleep(InputAction.CallbackContext context)
    {
        bool isSleeping = GetComponent<Movement>().enabled;

        animator.SetBool("Sleep", isSleeping);
        GetComponent<Movement>().enabled = !isSleeping;
    }

    private void Lick(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Lick");
    }

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        UpdateCollider();
        
        animator.SetFloat("Velocity Y", body2D.velocity.y);
    }

    void UpdateCollider()
    {
        Sprite sprite = spriteRenderer.sprite;
        if (sprite != null)
        {
            Vector2 size = sprite.bounds.size;
            boxCollider2D.size = size;
            boxCollider2D.offset = sprite.bounds.center;
        }
    }

}
