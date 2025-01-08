using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SnakeController snakeController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        Debug.Log("Game Restarted!");
       
    }
}
