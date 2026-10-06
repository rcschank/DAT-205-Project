using UnityEngine;

// Pickup
// Attached to the GoldCoinBlank prefab.
// Each coin tells the GameManager it exists when it starts, then adds to
// the score and removes itself when the player touches it.
//
// Setup needed:
//   - The coin's collider must have "Is Trigger" checked
//   - The player object must be tagged "Player"
//   - One of the two objects needs a Rigidbody (or CharacterController) for triggers to fire
public class Pickup : MonoBehaviour
{

    [SerializeField] private AudioClip pickupSound; // Audio clip to play when the pickup is collected

    [SerializeField] private int scoreValue = 1; // Score value to add when the pickup is collected

    void Start()
    {
        if (GameManager.Instance != null)  // Skip if there is no GameManager (testing a level that doesn't have one)
        {
            GameManager.Instance.AddCoinToCollect(); // Increment the count of coins to collect in the GameManager when the pickup is created
        }
    }

    // Runs when another collider enters this coin's trigger collider.
    // "other" is the collider that touched us.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ignore anything that isn't the player (like the chaser)
        {
            // Play the pickup sound at the position of the pickup object.
            // PlayClipAtPoint creates a temporary sound object, so the sound
            // keeps playing even after this coin is destroyed below.
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.4f);  // 0.4 = volume
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue); // Add the score value to the GameManager's score
            }

            Destroy(gameObject);  // Remove the coin from the scene
        }
    }
}
