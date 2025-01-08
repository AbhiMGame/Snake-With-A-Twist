using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public float speed = 100f; // Movement speed
    private Vector2 direction; // Movement direction
    private bool isControlled = false; // Flag to check if the player is controlling
    public Camera mainCamera;  // Camera reference for following the snake
    public float smoothSpeed = 0.125f;  // Smoothness of camera movement
    public GridManager gridManager; // Reference to the GridManager to interact with grid cells
    private float objectWidth, objectHeight;
    public ScoreManager scoreManager;

    // For tracking the game win
    private int totalCells;
    private int greenCells;

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

        // Calculate the total number of cells in the grid
        totalCells = gridManager.GetColumns() * gridManager.GetRows();
        greenCells = 0;  // Initially, no cells are green
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

        // Check if all the cells are green and if the game is won
        if (greenCells == totalCells)
        {
            // Pause the game and show a message
            PauseGame();
        }
    }

    void MoveSnake()
    {
        // Only move if the snake is controlled by the player
        if (isControlled)
        {
            Vector3 movement = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
            transform.position += movement; // Apply movement

            // Rotate the snake based on the direction it is moving
            RotateSnake();
        }

        // Check for bounds and bounce if necessary
        CheckBounds();
    }

    void RotateSnake()
    {
        // Rotate the snake based on the movement direction
        if (direction == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right (no rotation)
        }
        else if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180); // Face left (rotate 180 degrees)
        }
        else if (direction == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90); // Face up (rotate 90 degrees)
        }
        else if (direction == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90); // Face down (rotate -90 degrees)
        }
    }

    // Function to check grid bounds and bounce if necessary
    void CheckBounds()
    {
        Vector3 pos = transform.position;

        // Get the number of rows and columns from the GridManager
        int columns = gridManager.GetColumns();
        int rows = gridManager.GetRows();

        // Grid dimensions based on cell size and grid size
        float gridWidth = columns * gridManager.cellSize;  // Total width of the grid
        float gridHeight = rows * gridManager.cellSize;    // Total height of the grid

        // Correcting bounds check by considering half of the grid size
        float halfGridWidth = gridWidth / 2;
        float halfGridHeight = gridHeight / 2;

        // Check for horizontal bounds (left and right grid edges)
        if (pos.x + objectWidth > halfGridWidth || pos.x - objectWidth < -halfGridWidth)
        {
            // Reverse the direction horizontally if it goes out of bounds
            direction = (direction == Vector2.right) ? Vector2.left : Vector2.right;
        }

        // Check for vertical bounds (top and bottom grid edges)
        if (pos.y + objectHeight > halfGridHeight || pos.y - objectHeight < -halfGridHeight)
        {
            // Reverse the direction vertically if it goes out of bounds
            direction = (direction == Vector2.up) ? Vector2.down : Vector2.up;
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
            SpriteRenderer cellRenderer = cell.GetComponent<SpriteRenderer>();
            if (cellRenderer.color != Color.green) // Check if the cell is already green
            {
                cellRenderer.color = Color.green;
                greenCells++; // Increase green cell count
            }
        }
    }


    public void IncreaseSnakeSize(float sizeIncrease)
    {
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x + sizeIncrease, currentScale.y + sizeIncrease, currentScale.z);
    }


    // Pause the game and show a "You Won!" message
    void PauseGame()
    {
        scoreManager.ShowWin();
        Time.timeScale = 0; // Freeze the game
        Debug.Log("You Won!"); // You can replace this with a UI element to show the message
      
    }
}
