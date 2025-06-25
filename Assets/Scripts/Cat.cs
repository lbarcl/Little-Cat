using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        UpdateCollider();
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
