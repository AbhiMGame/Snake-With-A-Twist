using UnityEngine;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour
{
    public float moveSpeed = 0.2f; // Speed at which the snake moves
    private Queue<Vector2Int> snakeBody = new Queue<Vector2Int>(); // Stores snake body segments
    private Vector2Int currentPosition; // Head position of the snake
    private Vector2Int direction = Vector2Int.right; // Snake's initial movement direction
    private float moveTimer; // Timer to track movement interval
    private GridManager gridManager; // Reference to the GridManager script

    private void Start()
    {
        currentPosition = new Vector2Int(5, 5); // Start at the middle of the grid (adjust size as needed)
        snakeBody.Enqueue(currentPosition);
        gridManager = FindObjectOfType<GridManager>(); // Find the GridManager instance
        UpdateWorldPosition();
    }

    private void Update()
    {
        // Change direction based on player input
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2Int.down)
        {
            direction = Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2Int.up)
        {
            direction = Vector2Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2Int.right)
        {
            direction = Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2Int.left)
        {
            direction = Vector2Int.right;
        }

        // Continuously move the snake at a fixed interval
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveSpeed)
        {
            MoveSnake();
            moveTimer = 0f; // Reset the move timer
        }
    }

    private void MoveSnake()
    {
        Vector2Int newHeadPosition = currentPosition + direction;

        // Wrap the snake around the grid if it goes out of bounds (adjust grid size as needed)
        newHeadPosition.x = Mathf.Clamp(newHeadPosition.x, 0, gridManager.gridSettings.width - 1);
        newHeadPosition.y = Mathf.Clamp(newHeadPosition.y, 0, gridManager.gridSettings.height - 1);

        // Check if the snake collides with itself (excluding the head)
        if (snakeBody.Contains(newHeadPosition))
        {
            Debug.Log("Game Over! Snake collided with itself.");
            // Add game over logic here
            return;
        }

        // Add the new head to the snake body
        snakeBody.Enqueue(newHeadPosition);

        // Check if the snake is eating food
        if (gridManager.IsTileHealthy(newHeadPosition))
        {
            // If eating food, don't remove the tail 
            gridManager.SetTileHealthy(currentPosition);
        }
        else
        {
            // If not eating food, remove the tail 
            snakeBody.Dequeue();
        }

        currentPosition = newHeadPosition;
        UpdateWorldPosition();
    }

    private void UpdateWorldPosition()
    {
        // Assume each tile is 1 unit in world space (you can adjust this if needed)
        float tileSize = 1f;
        Vector3 worldPosition = new Vector3(
            currentPosition.x * tileSize,
            currentPosition.y * tileSize,
            0
        );

        transform.position = worldPosition; // Move the snake object
    }
}