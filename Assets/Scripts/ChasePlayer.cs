using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject coinPrefab;  // Reference to the coin prefab to instantiate when dropping a coin
    [SerializeField] private AudioClip dropCoinSound;  // Reference to the sound clip to play when dropping a coin]

    private float pauseTimer = 0f;
    private float distance = 100f;  // Initialize distance to a large value to ensure the enemy starts chasing the player immediately

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (pauseTimer > 0f)  // If the pause timer is greater than 0, decrement it by the time elapsed since the last frame
        {
            pauseTimer -= Time.deltaTime;
            navMeshAgent.isStopped = true;  // Stop the NavMeshAgent from moving while paused
            return;  // Exit the Update method early to prevent the enemy from chasing the player while paused
        }

        navMeshAgent.isStopped = false;  // Resume the NavMeshAgent's movement if the pause timer has expired

        if (GameManager.Instance.score > 0)  // If the player's score is greater than 0, set the destination of the NavMeshAgent to the player's position, causing the enemy to chase the player
        {
            navMeshAgent.SetDestination(playerTransform.position);  // Set the destination of the NavMeshAgent to the player's position, causing the enemy to chase the player
        }

        distance = Vector3.Distance(transform.position, playerTransform.position);  // Calculate the distance between the enemy and the player

        if (distance < 1f)  // If the distance between the enemy and the player is less than 0.5 units, pause the enemy's movement for 2 seconds
        {
            pauseTimer = 2f;  // Set the pause timer to 2 seconds, causing the enemy to stop moving for that duration
            Caught();  // Call the Caught method to handle the event when the enemy is close enough to the player
        }
    }

    private void Caught()
    {
        GameManager.Instance.GameOver(true);  // Call the GameOver method on the GameManager instance, passing true to indicate that the player was caught by the enemy
    }

}
