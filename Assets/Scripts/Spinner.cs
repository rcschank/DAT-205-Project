using UnityEngine;

// Spinner
// Rotates an object continuously while isSpinning is true.
// Used on the GoldCoinBlank and Capsule prefabs.
// ChangeState and GetIsSpinning are left over from the Playground week,
// where a click toggled the spin. Nothing calls them in the final game.
public class Spinner : MonoBehaviour
{

    [SerializeField] private bool isSpinning = false;  // Variable to track whether the spinner is currently spinning or not
    [SerializeField] private Vector3 rotationDirection = new Vector3(0f, 45f, 0f); // Degrees per second around each axis. Default is 45 degrees per second around Y.
    public void ChangeState() // Method to toggle the spinning state of the spinner
    {
        if (isSpinning == true) // If the spinner is currently spinning, set isSpinning to false to stop it
        {
            isSpinning = false; // Set isSpinning to false to stop the spinner
        }
        else // If the spinner is not currently spinning, set isSpinning to true to start it
        {
            isSpinning = true; // Set isSpinning to true to start the spinner
        }
    }


    public bool GetIsSpinning() // Method to get the current spinning state of the spinner
    {
        return isSpinning;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning) // If the spinner is currently spinning, rotate it by rotationDirection
        {
            // Space.World spins around the world's axes, so a tilted coin
            // still spins around straight up
            transform.Rotate(rotationDirection * Time.deltaTime, Space.World); // Rotate by rotationDirection degrees per second (45 around Y by default)
        }
    }
}
