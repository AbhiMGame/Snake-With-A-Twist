using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public float speed = 100f; // Movement speed
    private Vector2 direction; // Movement direction
    private bool isControlled = false; // Flag to check if the player is controlling
    public Camera mainCamera;  // Camera reference for following the snake
    public float smoothSpeed = 0.125f;  // Smoothness of camera movement
    public GridManager gridManager; // Reference to the GridManager to interact with grid cells

    private Vector2 gridBounds; // Store grid bounds (in world space)
    private float objectWidth, objectHeight;

    void Start()
    {
        // Get object size
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectWidth = renderer.bounds.extents.x;
            objectHeight = renderer.bounds.extents.y;
        }
        else
        {
            objectWidth = 0.5f;
            objectHeight = 0.5f;
        }

        // Choose a random initial direction (either left-right or up-down)
        if (Random.value > 0.5f)
            direction = Vector2.right;  // Move horizontally
        else
            direction = Vector2.up;     // Move vertically

        // Calculate grid bounds in world space (this is the visible area)
        gridBounds = new Vector2(gridManager.GetColumns() * gridManager.cellSize, gridManager.GetRows() * gridManager.cellSize);
    }

    void Update()
    {
        // Get player input for controlling snake movement
        float horizontal = Input.GetAxisRaw("Horizontal"); // -1 (Left), 1 (Right)
        float vertical = Input.GetAxisRaw("Vertical");     // -1 (Down), 1 (Up)

        // Change direction based on input
        if (horizontal != 0)
        {
            direction = (horizontal > 0) ? Vector2.right : Vector2.left;
            isControlled = true;
        }

        if (vertical != 0)
        {
            direction = (vertical > 0) ? Vector2.up : Vector2.down;
            isControlled = true;
        }

        // Move the snake
        MoveSnake();

        // Make the camera follow the snake
        FollowSnake();

        // Mark the tile where the snake is passing to change its color
        MarkCurrentTile();
    }

    void MoveSnake()
    {
        // Only move if the snake is controlled by the player
        if (isControlled)
        {
            Vector3 movement = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
            transform.position += movement; // Apply movement
        }

        // Check for grid bounds and rebound if necessary
        CheckBounds();
    }

    // Function to check grid bounds and bounce if necessary
    void CheckBounds()
    {
        Vector3 pos = transform.position;

        if (direction == Vector2.right || direction == Vector2.left)
        {
            if (pos.x + objectWidth > gridBounds.x / 2 || pos.x - objectWidth < -gridBounds.x / 2)
            {
                direction = (direction == Vector2.right) ? Vector2.up : Vector2.down; // Switch to vertical movement
            }
        }
        else if (direction == Vector2.up || direction == Vector2.down)
        {
            if (pos.y + objectHeight > gridBounds.y / 2 || pos.y - objectHeight < -gridBounds.y / 2)
            {
                direction = (direction == Vector2.up) ? Vector2.left : Vector2.right; // Switch to horizontal movement
            }
        }
    }

    void FollowSnake()
    {
        // If no camera is assigned, use the main camera
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Calculate the desired camera position to follow the snake
        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

        // Smoothly interpolate between the current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed);
        mainCamera.transform.position = smoothedPosition;
    }

    // Mark the current tile that the snake is passing over
    void MarkCurrentTile()
    {
        // Get the current position of the snake
        Vector2Int gridPosition = new Vector2Int(Mathf.FloorToInt(transform.position.x / gridManager.cellSize), Mathf.FloorToInt(transform.position.y / gridManager.cellSize));

        // Get the cell from the GridManager
        GameObject cell = gridManager.GetCell(gridPosition);

        if (cell != null)
        {
            // Turn the cell green to indicate the snake passed over it
            cell.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}
