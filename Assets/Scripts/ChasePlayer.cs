using UnityEngine;
using UnityEngine.AI;                 // NavMeshAgent lives here
using System.Collections.Generic;     // Needed for List<T>

// ChasePlayer
// Attached to the Chaser prefab (the police officer).
// Uses Unity's NavMesh system to walk toward the player every frame,
// plays footstep and "command" voice clips on timers, and ends the game
// when it gets close enough to catch the player.
//
// Setup needed in the scene:
//   - A baked NavMesh (each level has one in Scenes/Level X/)
//   - A NavMeshAgent component on the same GameObject
//   - Player Transform dragged into the Player Transform slot
//     (prefabs can't point at scene objects, so this is set per scene)
public class ChasePlayer : MonoBehaviour
{

    // The NavMeshAgent does the pathfinding and movement for us.
    // It is private and filled in by GetComponent in Start.
    private NavMeshAgent navMeshAgent;

    // The thing we are chasing. [SerializeField] lets us set a private
    // variable from the Inspector without making it public.
    [SerializeField] private Transform playerTransform;

    [SerializeField] float timeBetweenCommands = 5f;  // Time in seconds between each command
    [SerializeField] float timeBetweenFootsteps = 0.5f;  // Time in seconds between each footstep sound

    // Two separate AudioSources so a voice line and a footstep can play
    // at the same time without cutting each other off.
    [SerializeField] private AudioSource audioSource;  // Reference to the AudioSource component for playing sounds
    [SerializeField] private AudioSource footstepAudioSource;  // Reference to the AudioSource component for playing footstep sounds

    // Lists of clips to pick from at random so the sounds don't get repetitive.
    [SerializeField] private List<AudioClip> footsteps;
    [SerializeField] private List<AudioClip> commands;

    // Timers count UP from 0 using Time.deltaTime. When a timer passes its
    // limit, we do the action and reset it to 0.
    private float commandTimer = 0f;
    private float commandTimerOffset = 0f;  // Random head start for the command timer, so two chasers don't shout at the same moment
    private float footstepTimer = 0f;

    // When above 0, the chaser stands still. Counts DOWN to 0.
    private float pauseTimer = 0f;
    private float distance = 100f;  // Initialize distance to a large value to ensure the enemy starts chasing the player immediately

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();  // Grab the NavMeshAgent on this same GameObject
        footstepTimer = 0f;
        commandTimerOffset = Random.Range(0f, timeBetweenCommands);  // Randomize the initial command timer offset to avoid synchronized commands from multiple enemies
        commandTimer = commandTimerOffset;  // Start partway through the timer so each chaser's first command comes at a different time
    }

    // Update is called once per frame
    void Update()
    {
        // Only chase while the level is being played. This stops the chaser
        // from "catching" the player after the level is already won, and
        // stops a second chaser from calling GameOver again in the same frame.
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        // --- Sound timers ---
        commandTimer += Time.deltaTime;  // Increment the timer by the time elapsed since the last frame
        footstepTimer += Time.deltaTime;  // Increment the timer by the time elapsed since the last frame

        if (footstepTimer >= timeBetweenFootsteps)  // If the footstep timer has reached the specified time between footsteps, play a random footstep sound
        {
            footstepTimer = 0f;  // Reset the footstep timer
            if (footsteps.Count > 0)  // Check if there are any footstep sounds in the list
            {
                int randomIndex = Random.Range(0, footsteps.Count);  // Get a random index from the list (the max value is exclusive for ints)
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

        // --- Pause after catching ---
        if (pauseTimer > 0f)  // If the pause timer is greater than 0, decrement it by the time elapsed since the last frame
        {
            pauseTimer -= Time.deltaTime;
            navMeshAgent.isStopped = true;  // Stop the NavMeshAgent from moving while paused
            return;  // Exit the Update method early to prevent the enemy from chasing the player while paused
        }

        navMeshAgent.isStopped = false;  // Resume the NavMeshAgent's movement if the pause timer has expired


        // --- Chase ---
        // SetDestination asks the NavMesh for a path to the player's current
        // position. Doing this every frame keeps the path up to date as the
        // player moves.
        navMeshAgent.SetDestination(playerTransform.position);  // Set the destination of the NavMeshAgent to the player's position, causing the enemy to chase the player


        distance = Vector3.Distance(transform.position, playerTransform.position);  // Calculate the distance between the enemy and the player

        if (distance < 1f)  // If the enemy is within 1 unit of the player, pause the enemy's movement for 2 seconds
        {
            pauseTimer = 2f;  // Set the pause timer to 2 seconds, causing the enemy to stop moving for that duration
            Caught();  // Call the Caught method to handle the event when the enemy is close enough to the player
        }
    }

    // Called when the chaser reaches the player.
    // GameManager.Instance is the singleton, so any script can reach it.
    private void Caught()
    {
        GameManager.Instance.GameOver(true);  // Call the GameOver method on the GameManager instance, passing true to indicate that the player was caught by the enemy
    }

}
