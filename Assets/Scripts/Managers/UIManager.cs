using TMPro;
using UnityEngine;

// This class stores code needed for UI components

public class UIManager : MonoBehaviour
{
    public static bool isRunning; // Has the play button been pressed
    public GameObject endScreen; // The end screen
    public TextMeshProUGUI tilesPressText; // The text for the tiles pressed button
    public TextMeshProUGUI timerText; // The text for the timer button
    public TextMeshProUGUI endScreenText; // The text for the end screen

    private void Start()
    {
        // Initializes variables
        isRunning = false;
    }

    private void Update()
    {
        // Update the text for the tiles pressed button and the timer button
        tilesPressText.text = "Tiles Remaining: " + LevelManager.tilesRemaining;
        timerText.text = "" + string.Format("{0:00.00}", LevelManager.staticTimer);
    }

    public void StartLevel()
    {
        // When the play button is pressed, start the level
        isRunning = true;
    }
}
