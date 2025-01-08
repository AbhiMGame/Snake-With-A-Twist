using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isHealthy = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsHealthy => isHealthy;

    public void MakeHealthy()
    {
        if (!isHealthy)
        {
            isHealthy = true;
            SetColor(Color.green);
        }
    }

    private void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
