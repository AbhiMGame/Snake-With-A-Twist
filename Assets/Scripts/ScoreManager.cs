using UnityEngine;
using TMPro; // Import the TextMesh Pro namespace

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // The current score
    public TextMeshProUGUI scoreText; // Reference to TextMeshProUGUI component for score
    public TextMeshProUGUI winText;
    public SnakeController snakeController;
    public GameObject panel;


    void Start()
    {
        UpdateScoreUI(); // Initialize the score UI at the start
    }

    // Function to increase score
    public void IncreaseScore()
    {
        score++;
        UpdateScoreUI();
        snakeController.IncreaseSnakeSize(0.2f);
    }

    // Function to update the score UI
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Update the TextMeshProUGUI component with the score
        }
    }

    public void ShowWin()
    {
        winText.gameObject.SetActive(true);
        panel.SetActive(true);
    }
}
