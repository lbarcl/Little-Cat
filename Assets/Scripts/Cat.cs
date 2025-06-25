using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D body2D;

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
