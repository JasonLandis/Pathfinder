using TMPro;
using UnityEngine;

// This class stores code needed for UI components

public class UIManager : MonoBehaviour
{
    public static bool isRunning; // Has the play button been pressed
    public TextMeshProUGUI tilesPressText; // The text for the tiles pressed button
    public TextMeshProUGUI timerText; // The text for the timer button

    private void Start()
    {
        isRunning = false; // Set the play pressed to false
    }

    private void Update()
    {
        tilesPressText.text = "Tiles Remaining: " + LevelManager.tilesRemaining; // Set the text for the tiles pressed button
        timerText.text = "" + string.Format("{0:00.00}", LevelManager.staticTimer); // Set the text for the timer button
    }

    public void Play()
    {
        isRunning = true; // Set the play pressed to true
    }
}
