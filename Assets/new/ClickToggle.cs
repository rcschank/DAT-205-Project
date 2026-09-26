using UnityEngine;

public class ClickToggle : MonoBehaviour
{

    public Color clickedColor = Color.red;

    private Renderer myRenderer;
    private Color originalColor;
    private bool isClicked = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        originalColor = myRenderer.material.color;
    }

    void OnMouseDown()
    {
        isClicked = !isClicked;

        if (isClicked)
        {
            myRenderer.material.color = clickedColor;
        }
        else
        {
            myRenderer.material.color = originalColor;
        }
    }
}
