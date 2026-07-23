using UnityEngine;

public class StartSpinner : MonoBehaviour
{
    [SerializeField] private Spinner spinner;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip stopSound;

    [SerializeField] private ParticleSystem clickEffect;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the Collider

    void OnMouseDown()
    {
        // Check state of spinner before changing the state and playing the click sound and particle effect

        if (!spinner.GetIsSpinning())  // If the spinner is not spinning, play the click sound and particle effect
        { 
            if (audioSource != null && clickSound != null) // Check if the AudioSource and clickSound are not null before playing the sound
            {
                audioSource.PlayOneShot(clickSound); // Play the click sound once
            }

            if (clickEffect != null) // Check if the clickEffect is not null before playing the particle effect
            {
                clickEffect.Play(); // Play the particle effect
            }
        }
        else  // If the spinner is spinning, play the stop sound
        { 
            if (audioSource != null && stopSound != null) // Check if the AudioSource and stopSound are not null before playing the sound
            {
                audioSource.PlayOneShot(stopSound); // Play the stop sound once
            }
        }

        spinner.ChangeState();  // Call the ChangeState method on the Spinner script to toggle the spinning state        
    }

}
