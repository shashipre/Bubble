using UnityEngine;
using UnityEngine.InputSystem;

public class RelaxingBubble : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public AudioClip popSound;
    public GameObject popEffect;

    [Header("Movement Settings")]
    // We calculate the actual speed in Start(), so these are just limits
    public float minSpeed = 0.5f; // Speed for the biggest bubbles
    public float maxSpeed = 3.0f; // Speed for the smallest bubbles

    private float verticalSpeed;
    private float swaySpeed;
    private float swayWidth;
    private float timeOffset; // To make sure bubbles don't wave in sync

    private bool isPopping = false;

    void Start()
    {
        // 1. Auto-find components
        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // 2. Set Random Size (Between 1.2 and 2.5 as requested)
        float minSize = 1.2f;
        float maxSize = 2.5f;
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        // 3. Calculate Speed based on Size (Big = Slow, Small = Fast)
        // "t" is a percentage: 0.0 means we are at minSize, 1.0 means we are at maxSize
        float t = Mathf.InverseLerp(minSize, maxSize, randomSize);

        // If t is 0 (Smallest), we use maxSpeed. If t is 1 (Biggest), we use minSpeed.
        verticalSpeed = Mathf.Lerp(maxSpeed, minSpeed, t);

        // 4. Setup Wavy Movement Variables
        swaySpeed = Random.Range(1.0f, 2.0f); // How fast it wiggles left/right
        swayWidth = Random.Range(0.5f, 1.5f); // How wide the wiggle is
        timeOffset = Random.Range(0f, 100f);  // Random start point for the wave
    }

    void Update()
    {
        // Stop moving if we are popping
        if (isPopping) return;

        // --- WAVY MOVEMENT LOGIC ---

        // A. Calculate Upward Movement
        Vector3 movement = Vector3.up * verticalSpeed * Time.deltaTime;

        // B. Calculate Horizontal (Sine Wave) Movement
        float horizontalMove = Mathf.Sin((Time.time + timeOffset) * swaySpeed) * swayWidth;

        // C. Apply to X axis
        movement.x = horizontalMove * Time.deltaTime;

        // D. Move the object
        transform.Translate(movement);


        // Destroy if off-screen
        if (transform.position.y > 10f) Destroy(gameObject);

        // --- INPUT LOGIC ---
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);

            if (hit.collider != null)
            {
                // Check parent or child
                if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
                {
                    StartPopSequence(hit.collider);
                }
            }
        }
    }

    void StartPopSequence(Collider2D colliderToDisable)
    {
        isPopping = true;

        if (colliderToDisable != null)
        {
            colliderToDisable.enabled = false;
        }

        if (popSound) AudioSource.PlayClipAtPoint(popSound, transform.position);

        // Optional: Spawn particle effect if you have one
        if (popEffect) Instantiate(popEffect, transform.position, Quaternion.identity);

        if (animator != null)
        {
            // MAKE SURE THIS NAME MATCHES YOUR ANIMATOR EXACTLY (Case Sensitive!)
            animator.SetTrigger("popTrigger");
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}