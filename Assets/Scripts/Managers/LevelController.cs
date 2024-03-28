using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using TMPro;

// Controls objects associated with the level during runtime
// Also controls specific level settings

public class LevelController : MonoBehaviour
{
    [Header("UI Properties")]
    public GameObject endScreen; // The end screen
    public TextMeshProUGUI endScreenText; // The text for the end screen
    public TextMeshProUGUI tilesPressText; // The text for the tiles pressed button
    public TextMeshProUGUI timerText; // The text for the timer button

    [Header("Level Properties")]
    public GameObject cursor; // The cursor icon
    public GameObject character; // The character object
    public int tilesCanPress; // The number of tiles the player can press for the specific level
    public float timer; // the timer for the character to move
    private bool isRunning = false; // Is the level running
    private bool succeeded = true; // Did the player succeed in the level

    [Header("Design Properties")]
    public Sprite cursorSprite; // The cursor sprite
    public Sprite previewRed; // The preview sprite for the red wall
    public Sprite previewBlue; // The preview sprite for the blue wall
    public Sprite previewYellow; // The preview sprite for the yellow wall

    private PathFinder pathFinder; // the path finder
    private List<OverlayTile> path; // the path the character is currently following
    private MapManager map; // the map manager
    private int tileSelected = 0; // the tile selected

    private void Start()
    {
        // Initialize path and path finder
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
        map = MapManager.Instance;

        character.GetComponent<CharacterInfo>().standingOnTile = map.startOverlayTile;
    }

    private void Update()
    {
        // Update UI
        tilesPressText.text = "Tiles Remaining: " + tilesCanPress;
        timerText.text = "" + string.Format("{0:00.00}", timer);

        // Get the tile the cursor is focused on
        RaycastHit2D? hit = GetFocusedOnTile();

        // If the timer has ended or the player has reached the destination, stop time
        if (timer <= 0 || character.GetComponent<CharacterInfo>().standingOnTile == map.endOverlayTile)
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            isRunning = false;
            Time.timeScale = 0;
            if (timer > 0)
            {
                succeeded = false;
            }
            if (succeeded)
            {
                endScreenText.text = "You won!";
            }
            else
            {
                endScreenText.text = "You lost with " + string.Format("{0:00.00}", timer) + " seconds left";
            }
            endScreen.SetActive(true);
        }

        // If a tile is focused on
        else if (hit.HasValue)
        {
            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            cursor.transform.position = tile.transform.position;

            if (tilesCanPress == 0)
            {
                cursor.GetComponent<SpriteRenderer>().sprite = cursorSprite;
            }
            
            if (tile == map.startOverlayTile || tile == map.endOverlayTile)
            {
                cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
            
            // If the player presses the left mouse button and can place a tile down
            else if (Input.GetMouseButtonDown(0) && tilesCanPress != 0 && (tile != map.endOverlayTile && tile != map.startOverlayTile)
                && (tile.GetComponent<OverlayTile>().isBlocked == false && tileSelected != 0))
            {
                tile.isBlocked = true;
                tilesCanPress -= 1;

                switch (tileSelected)
                {
                    case 1:
                        tile.ShowRedTile();
                        break;
                    case 2:
                        tile.ShowBlueTile();
                        break;
                    case 3:
                        tile.ShowYellowTile();
                        break;
                }

                // If the game is running, reveal the path
                if (isRunning)
                {
                    RevealPath();
                }
            }
        }
        
        // If the cursor is not hovering over a tile, hide it
        else
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Hide the cursor
        }

        // If the player has pressed the play button, start the timer and move the character
        if (isRunning)
        {
            timer -= Time.deltaTime;
            if (path.Count > 0)
            {
                MoveAlongPath();
            }
        } 
        else
        {
            RevealPath();
        }        
    }
    
    private static RaycastHit2D? GetFocusedOnTile()
    {
        // Determines if the cursor is hovering over a tile
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new(mousePos.x, mousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }

    private void MoveAlongPath()
    {
        // Move the character along the path
        var step = character.GetComponent<CharacterInfo>().characterSpeed * Time.deltaTime;
        float zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);
        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            // Hide the tile when the character crosses it and remove it from the path
            path[0].HideTile();
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        // Position the character on the tile
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<CharacterInfo>().standingOnTile = tile;
    }

    private void RevealPath()
    {
        // Shows and reveals tiles in the path as well as generates the path
        foreach (OverlayTile tile in path)
        {
            if (tile.GetComponent<SpriteRenderer>().sprite != tile.redWallSprite && tile.GetComponent<SpriteRenderer>().sprite != tile.blueWallSprite && tile.GetComponent<SpriteRenderer>().sprite != tile.yellowWallSprite)
            {
                tile.HideTile();
            }
        }
        path = pathFinder.FindPath(character.GetComponent<CharacterInfo>().standingOnTile, map.endOverlayTile);
        foreach (OverlayTile tile in path)
        {
            tile.ShowPathTile();
        }
    }
    
    public void StartLevel()
    {
        // When the play button is pressed, start the level
        isRunning = true;
    }

    public void SetCursorRed()
    {
        // Set the cursor to red
        cursor.GetComponent<SpriteRenderer>().sprite = previewRed;
        tileSelected = 1;
    }
    
    public void SetCursorBlue()
    {
        // Set the cursor to blue
        cursor.GetComponent<SpriteRenderer>().sprite = previewBlue;
        tileSelected = 2;        
    }

    public void SetCursorYellow()
    {
        // Set the cursor to yellow
        cursor.GetComponent<SpriteRenderer>().sprite = previewYellow;
        tileSelected = 3;     
    }
}
