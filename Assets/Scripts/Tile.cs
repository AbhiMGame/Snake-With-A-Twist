using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool isHealthy = false; // Track the tile's state

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsHealthy => isHealthy; // Read-only property

    public void MakeHealthy()
    {
        if (!isHealthy)  // Prevent redundant updates
        {
            isHealthy = true;
            SetColor(Color.green); // Change the tile color to green
        }
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
