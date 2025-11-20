using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip popSound;

    public GameObject popEffectPrefab;
    void OnMouseDown()
    {
        // --- This is where the magic happens ---

        // 1. Play a sound (if you added one)
        if (popSound != null)
        {
            // Play the sound at the bubble's position
            AudioSource.PlayClipAtPoint(popSound, transform.position);
        }

        // 2. Show a particle effect (if you added one)
        if (popEffectPrefab != null)
        {
            Instantiate(popEffectPrefab, transform.position, Quaternion.identity);
        }

        // 3. Destroy the bubble
        Destroy(gameObject);
    }
}
