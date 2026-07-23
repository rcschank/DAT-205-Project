using UnityEngine;

public class ClickForce : MonoBehaviour
{


    [SerializeField] private float force = 10f;

    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

}
