using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource), typeof(Animator), typeof(Movement))]
public class Cat : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] meows;
    [SerializeField] private AudioClip lick;
    [SerializeField] private AudioClip hum;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D body2D;
    private AudioSource audioSource;
    private Animator animator;
    private Movement movement;
    private PlayerAction actions;
    private Sprite previousSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();

        actions = new PlayerAction();
        actions.Interractions.Enable();

        actions.Interractions.Lick.performed += Lick;
        actions.Interractions.Sleep.performed += Sleep;
        actions.Interractions.Speek.performed += Meow;
    }

    private void OnDestroy()
    {
        // Properly remove input listeners
        actions.Interractions.Lick.performed -= Lick;
        actions.Interractions.Sleep.performed -= Sleep;
        actions.Interractions.Sleep.performed -= Meow;
        actions.Dispose();
    }

    private void Meow(InputAction.CallbackContext context)
    {
        if (meows.Length > 0)
        {
            AudioClip clip = meows[Random.Range(0, meows.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void Sleep(InputAction.CallbackContext context)
    {
        bool goingToSleep = movement.enabled;
        movement.enabled = !goingToSleep;

        animator.SetBool("Sleep", goingToSleep);
        Meow(new InputAction.CallbackContext());

        if (goingToSleep)
        {
            if (audioSource.clip != hum)
                audioSource.clip = hum;

            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == hum)
                audioSource.Stop();

            audioSource.loop = false;
        }
    }

    private void Lick(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Lick");
        if (lick != null)
            audioSource.PlayOneShot(lick);
    }

    private void LateUpdate()
    {
        UpdateColliderIfNeeded();
        animator.SetFloat("Velocity Y", body2D.velocity.y);
    }

    private void UpdateColliderIfNeeded()
    {
        Sprite currentSprite = spriteRenderer.sprite;
        if (currentSprite != null && currentSprite != previousSprite)
        {
            previousSprite = currentSprite;
            Vector2 size = currentSprite.bounds.size;
            boxCollider2D.size = size;
            boxCollider2D.offset = currentSprite.bounds.center;
        }
    }
}
