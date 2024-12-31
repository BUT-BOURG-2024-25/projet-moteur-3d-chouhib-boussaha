using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOver; // Reference to the death pop-up canvas

    [SerializeField]
    private Text survivalTimeText; // Reference to the text displaying survival time

    private bool isGameOver = false;

    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure the game-over canvas is hidden on start
        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }
    }

    public void ShowDeathPopup(float survivalTime)
    {
        // Prevent duplicate calls to the game-over logic
        if (isGameOver)
            return;

        // Pause the game
        Time.timeScale = 0f;
        isGameOver = true;

        // Activate the death pop-up canvas
        if (gameOver != null)
        {
            gameOver.SetActive(true);
        }

        // Display the survival time
        if (survivalTimeText != null)
        {
            survivalTimeText.text = $"{FormatTime(survivalTime)}";
        }
        else
        {
            Debug.LogWarning("Survival time text is not assigned in the GameOverManager.");
        }
    }

    public void ReplayGame()
    {
        // Unpause the game
        Time.timeScale = 1f;
        isGameOver = false;

        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:D2}:{seconds:D2}"; // Format as MM:SS
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
