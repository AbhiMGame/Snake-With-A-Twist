using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GridData gridSettings;
    public GameObject tilePrefab;
    public GameObject foodPrefab;
    private Tile[,] gridTiles;
    private List<Vector2Int> nonHealthyTiles = new List<Vector2Int>();
    private float tileSize;
    private Vector3 originalTileScale;

    private void Start()
    {
        // Store the original tile scale before any modifications
        originalTileScale = tilePrefab.transform.localScale;
        CalculateTileSize();
        GenerateGrid();
    }

    private void CalculateTileSize()
    {
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Add some padding (90% of screen size)
        screenHeight *= 0.9f;
        screenWidth *= 0.9f;

        float heightBasedSize = screenHeight / gridSettings.height;
        float widthBasedSize = screenWidth / gridSettings.width;
        tileSize = Mathf.Min(heightBasedSize, widthBasedSize);
    }

    public void GenerateGrid()
    {
        gridTiles = new Tile[gridSettings.width, gridSettings.height];
        nonHealthyTiles.Clear();

        float totalWidth = gridSettings.width * tileSize;
        float totalHeight = gridSettings.height * tileSize;
        float startX = -totalWidth / 2f;
        float startY = totalHeight / 2f;

        // Calculate scale factor for tiles only
        float scaleFactor = tileSize / originalTileScale.x;

        for (int x = 0; x < gridSettings.width; x++)
        {
            for (int y = 0; y < gridSettings.height; y++)
            {
                Vector3 position = new Vector3(
                    startX + (x * tileSize) + (tileSize / 2f),
                    startY - (y * tileSize) - (tileSize / 2f),
                    0
                );

                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity);
                tileObj.transform.SetParent(transform);

                // Scale only the tile objects
                tileObj.transform.localScale = originalTileScale * scaleFactor;

                Tile tile = tileObj.GetComponent<Tile>();
                gridTiles[x, y] = tile;
                nonHealthyTiles.Add(new Vector2Int(x, y));
            }
        }
        SpawnFood();
    }

    public bool IsTileHealthy(Vector2Int position)
    {
        return gridTiles[position.x, position.y] != null && gridTiles[position.x, position.y].IsHealthy;
    }

    public void SetTileHealthy(Vector2Int position)
    {
        if (gridTiles[position.x, position.y] != null && !IsTileHealthy(position))
        {
            gridTiles[position.x, position.y].MakeHealthy();
            nonHealthyTiles.Remove(position);
            if (nonHealthyTiles.Count == 0)
            {
                Debug.Log("Game Over! All tiles are green!");
                // Add game over logic here
            }
        }
    }

    public void SpawnFood()
    {
        if (nonHealthyTiles.Count > 0)
        {
            Vector2Int randomPos = nonHealthyTiles[Random.Range(0, nonHealthyTiles.Count)];

            float totalWidth = gridSettings.width * tileSize;
            float totalHeight = gridSettings.height * tileSize;
            float startX = -totalWidth / 2f;
            float startY = totalHeight / 2f;

            Vector3 spawnPosition = new Vector3(
                startX + (randomPos.x * tileSize) + (tileSize / 2f),
                startY - (randomPos.y * tileSize) - (tileSize / 2f),
                0
            );

            // Spawn food without scaling it
            Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        }
    }
}