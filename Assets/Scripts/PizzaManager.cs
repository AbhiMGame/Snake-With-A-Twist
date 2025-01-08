using UnityEngine;

public class PizzaManager : MonoBehaviour
{
    public GameObject pizzaPrefab; // Reference to the pizza prefab
    private GameObject pizza; // The pizza object that will be spawned
    private float spawnRangeX = 5f; // Horizontal spawn range for pizza
    private float spawnRangeY = 5f; // Vertical spawn range for pizza
    public ScoreManager scoreManager; // Reference to ScoreManager
    public float collisionDistance = 0.5f; // Distance threshold to consider "eating" the pizza

    void Start()
    {
        SpawnPizza(); // Spawn the first pizza
    }

    // Spawns a pizza at a random position
    void SpawnPizza()
    {
        // Generate a random position for the pizza
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomY = Random.Range(-spawnRangeY, spawnRangeY);

        // Instantiate pizza and set its position
        pizza = Instantiate(pizzaPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
    }

    void Update()
    {
        CheckPizzaCollision(); // Check for collision every frame
    }

    // Check if the snake's position overlaps with the pizza's position
    void CheckPizzaCollision()
    {
        if (pizza == null) return; // If no pizza, skip

        // Get the snake's position (assuming snake is managed by another script and has a reference)
        GameObject snake = GameObject.FindGameObjectWithTag("Snake");
        if (snake == null) return;

        Vector3 snakePosition = snake.transform.position;
        Vector3 pizzaPosition = pizza.transform.position;

        // Calculate the distance between the snake and the pizza
        float distance = Vector3.Distance(snakePosition, pizzaPosition);

        // If the distance is small enough, consider the pizza eaten
        if (distance <= collisionDistance)
        {
            // Increase the score
            scoreManager.IncreaseScore();

            // Destroy the pizza object after being eaten
            Destroy(pizza);

            // Spawn a new pizza after one is eaten
            SpawnPizza();
        }
    }
}
