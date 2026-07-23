using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Rotate(45f * Time.deltaTime, 0f, 10f * Time.deltaTime);
    }
}
