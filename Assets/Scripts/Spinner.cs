using UnityEngine;

public class Spinner : MonoBehaviour
{

    private bool isSpinning = false;

    void OnMouseDown()
    {
        if (isSpinning == true)
        {
            isSpinning = false;
        }
        else
        {
            isSpinning = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
        transform.Rotate(45f * Time.deltaTime, 0f, 10f * Time.deltaTime);
        }
    }
}
