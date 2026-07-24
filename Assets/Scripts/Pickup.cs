using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] private AudioClip pickupSound; // Audio clip to play when the pickup is collected

    [SerializeField] private int scoreValue = 1; // Score value to add when the pickup is collected

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the pickup sound at the position of the pickup object
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            GameManager.Instance.AddScore(scoreValue); // Add the score value to the GameManager's score

            Destroy(gameObject);
        }
    }
}
