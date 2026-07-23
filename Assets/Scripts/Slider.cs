using UnityEngine;
using UnityEngine.AI;

public class Slider : MonoBehaviour
{

    private float startZ;
    private float direction = 1f;  //1f is forward, -1f is backward

    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float distance = 1.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.z >= startZ + distance)
        {
            direction = -1f;
        }
        else if (transform.position.z <= startZ)
        {
            direction = 1f;
        
        }

        transform.Translate(0f, 0f, speed * Time.deltaTime * direction);
    }
}
