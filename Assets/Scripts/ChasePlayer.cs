using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ChasePlayer : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform playerTransform;

    [SerializeField] float timeBetweenCommands = 5f;  // Time in seconds between each command
    [SerializeField] float timeBetweenFootsteps = 0.5f;  // Time in seconds between each footstep sound

    [SerializeField] private AudioSource audioSource;  // Reference to the AudioSource component for playing sounds
    [SerializeField] private AudioSource footstepAudioSource;  // Reference to the AudioSource component for playing footstep sounds

    [SerializeField] private List<AudioClip> footsteps;
    [SerializeField] private List<AudioClip> commands;

    private float commandTimer = 0f;
    private float commandTimerOffset = 0f;
    private float footstepTimer = 0f;

    private float pauseTimer = 0f;
    private float distance = 100f;  // Initialize distance to a large value to ensure the enemy starts chasing the player immediately

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        commandTimer = 0f;
        footstepTimer = 0f;
        commandTimerOffset = Random.Range(0f, timeBetweenCommands);  // Randomize the initial command timer offset to avoid synchronized commands from multiple enemies
    }

    // Update is called once per frame
    void Update()
    {
        commandTimer += Time.deltaTime;  // Increment the timer by the time elapsed since the last frame
        footstepTimer += Time.deltaTime;  // Increment the timer by the time elapsed since the last frame

        if (footstepTimer >= timeBetweenFootsteps)  // If the footstep timer has reached the specified time between footsteps, play a random footstep sound
        {
            footstepTimer = 0f;  // Reset the footstep timer
            if (footsteps.Count > 0)  // Check if there are any footstep sounds in the list
            {
                int randomIndex = Random.Range(0, footsteps.Count);  // Get a random index from the list of footstep sounds
                footstepAudioSource.PlayOneShot(footsteps[randomIndex]);  // Play the selected footstep sound at the enemy's position
            }
        }

        if (commandTimer >= timeBetweenCommands)  // If the command timer has reached the specified time between commands, play a random command sound
        {
            commandTimer = 0f;  // Reset the command timer
            if (commands.Count > 0)  // Check if there are any command sounds in the list
            {
                int randomIndex = Random.Range(0, commands.Count);  // Get a random index from the list of command sounds
                audioSource.PlayOneShot(commands[randomIndex]);  // Play the selected command sound at the enemy's position
            }
        }

        if (pauseTimer > 0f)  // If the pause timer is greater than 0, decrement it by the time elapsed since the last frame
        {
            pauseTimer -= Time.deltaTime;
            navMeshAgent.isStopped = true;  // Stop the NavMeshAgent from moving while paused
            return;  // Exit the Update method early to prevent the enemy from chasing the player while paused
        }

        navMeshAgent.isStopped = false;  // Resume the NavMeshAgent's movement if the pause timer has expired


        navMeshAgent.SetDestination(playerTransform.position);  // Set the destination of the NavMeshAgent to the player's position, causing the enemy to chase the player


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
