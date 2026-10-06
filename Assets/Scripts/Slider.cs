using UnityEngine;

// Slider
// Moves an object back and forth in a straight line, forever.
// Used on the Cylinder prefab and the moving Platform in Level 3.
// It starts where the object was placed and slides "distance" units in the
// direction the object's blue Z arrow (transform.forward) points, then
// slides back, and repeats.
//
// Note: Unity also has a UI component named Slider (UnityEngine.UI.Slider).
// This script doesn't conflict with it unless a file uses
// "using UnityEngine.UI;" AND tries to use the word Slider.
public class Slider : MonoBehaviour
{

    private Vector3 startPosition; // Variable to store the starting position of the slider
    private Vector3 endPosition; // Variable to store the ending position of the slider
    private bool movingForward = true;  // true is forward, false is backward

    [SerializeField] private float speed = 1.5f; // Variable to control the speed of the slider's movement, in units per second. Default value is 1.5f
    [SerializeField] private float distance = 1.5f; // Variable to control the distance the slider moves from its starting position, default value is 1.5f


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Remember both ends of the path once, so the object always travels
        // between the same two points
        startPosition = transform.position;
        endPosition = startPosition + (transform.forward * distance); // Calculate the end position based on the starting position and the specified distance
    }

    // Update is called once per frame
    void Update()
    {
        // MoveTowards moves from the current position toward a target by at most
        // the given step, and never overshoots. Once it is close enough it
        // returns the target exactly, which is why the == checks below work.
        // speed * Time.deltaTime turns "units per second" into "units this frame".
        if (movingForward) // If the slider is moving forward, move it towards the end position
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime); // Move the slider towards endPosition

            if (transform.position == endPosition) // If the slider has reached the end position, change direction to move backward
            {
                movingForward = false;
            }
        }
        else // If the slider is moving backward, move it towards the start position
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime); // Move the slider towards startPosition

            if (transform.position == startPosition) // If the slider has reached the start position, change direction to move forward
            {
                movingForward = true;
            }
        }

    }
}
