using TMPro;
using UnityEngine;

// This class stores code needed for UI components

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    
    public TextMeshProUGUI tilesPressText; // The text for the tiles pressed button
    public TextMeshProUGUI timerText; // The text for the timer button
    public GameObject endScreen; // The end screen
    public TextMeshProUGUI endScreenText; // The text for the end screen

    private void Awake()
    {
        // Initialize the instance
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        // Update the text for the tiles pressed button and the timer button
        tilesPressText.text = "Tiles Remaining: " + LevelManager.Instance.tilesCanPress;
        timerText.text = "" + string.Format("{0:00.00}", LevelManager.Instance.timer);
    }

    public void StartLevel()
    {
        // When the play button is pressed, start the level
        LevelManager.Instance.isRunning = true;
    }
}
