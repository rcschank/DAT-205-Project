using UnityEngine;

public class Spinner : MonoBehaviour
{

    private bool isSpinning = false;  // Variable to track whether the spinner is currently spinning or not

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
        transform.Rotate(45f * Time.deltaTime, 0f, 10f * Time.deltaTime);
        }
    }
}
