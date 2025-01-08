using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 10;         // Number of rows (can be set in the inspector)
    public int columns = 10;      // Number of columns (can be set in the inspector)
    public float cellSize = 1f;   // Size of each cell
    public GameObject cellPrefab; // Assign a square sprite prefab in Unity Editor

    private Dictionary<Vector2Int, GameObject> gridCells = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Calculate the center position (0,0)
        Vector2 center = Vector2.zero;

        // Loop through each row and column, starting from the center (0, 0)
        for (int x = -columns / 2; x < columns / 2; x++)  // Centered horizontally
        {
            for (int y = -rows / 2; y < rows / 2; y++)  // Centered vertically
            {
                // Calculate the world position for each cell
                Vector2 position = new Vector2(x * cellSize, y * cellSize) + center;

                // Instantiate the cell at the calculated position
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{y}";

                // Store reference for easy lookup
                gridCells[new Vector2Int(x, y)] = cell;
            }
        }
    }

    public int GetColumns()
    {
        return columns;
    }

    public int GetRows()
    {
        return rows;
    }

    public GameObject GetCell(Vector2Int position)
    {
        return gridCells.ContainsKey(position) ? gridCells[position] : null;
    }

    // Method to change the color of a specific cell
    public void ChangeCellColor(Vector2Int position, Color color)
    {
        if (gridCells.ContainsKey(position))
        {
            Renderer cellRenderer = gridCells[position].GetComponent<Renderer>();
            if (cellRenderer != null)
            {
                cellRenderer.material.color = color;
            }
        }
    }
}
