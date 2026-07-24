using UnityEngine;

public class Spinner : MonoBehaviour
{

    [SerializeField] private bool isSpinning = false;  // Variable to track whether the spinner is currently spinning or not
    [SerializeField] private Vector3 rotationDirection = new Vector3(0f, 45f, 0f); // Variable to control the direction of rotation, default is around the Y-axis
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
        if (isSpinning) // If the spinner is currently spinning, rotate it around the Y-axis and Z-axis
        {
            transform.Rotate(rotationDirection * Time.deltaTime); // Rotate the spinner around the specified rotation direction at a speed of 1 degree per second
        }
    }
}
