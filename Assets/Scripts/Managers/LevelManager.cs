using UnityEngine;
using System.Linq;
using System.Collections.Generic;

// Controls objects associated with the level during runtime
// Also controls specific level settings

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance; // Instance of the class
    public static LevelManager Instance { get { return _instance; } } // Public access to the instance 

    [Header("Level Properties")]
    public float speedOfCharacter; // the speed of the character
    public float timer; // the timer for the character to move
    public int tilesCanPress; // The number of tiles the player can press for the specific level
    public bool isRunning = false; // Is the level running
    public bool succeeded = true; // Did the player succeed in the level
    public bool endScreenActive; // Is the end screen active

    [Header("Design Properties")]
    public GameObject cursor; // The cursor icon
    public Sprite cursorSprite; // The cursor sprite
    public Sprite wallSprite; // The sprite for the wall
    public Sprite previewWall; // The sprite for the preview wall
    public Sprite pathSprite; // The sprite for the path
    public Sprite previewPath; // The sprite for the preview path

    private PathFinder pathFinder; // the path finder
    private List<OverlayTile> path; // the path the character is currently following

    private void Awake()
    {
        // Initialize the instance
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // Initialize path and path finder
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
    }

    private void Update()
    {        
        // If the timer has ended or the player has reached the destination, stop time
        if (timer <= 0 || MapManager.Instance.character.GetComponent<CharacterManager>().standingOnTile == MapManager.Instance.endOverlayTile)
        {
            isRunning = false;
            Time.timeScale = 0;
            if (timer > 0)
            {
                succeeded = false;
            }
            if (succeeded)
            {
                UIManager.Instance.endScreenText.text = "You won!";
            }
            else
            {
                UIManager.Instance.endScreenText.text = "You lost with " + string.Format("{0:00.00}", timer) + " seconds left";
            }
            UIManager.Instance.endScreen.SetActive(true);
            endScreenActive = true;
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

        // Get the tile the cursor is focused on
        RaycastHit2D? hit = GetFocusedOnTile();

        // If a tile is focused on
        if (hit.HasValue && endScreenActive == false)
        {
            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            cursor.GetComponent<SpriteRenderer>().sprite = previewWall;
            cursor.GetComponent<SpriteRenderer>().sortingOrder = 2;
            cursor.transform.position = tile.transform.position;

            if (tilesCanPress == 0)
            {
                cursor.GetComponent<SpriteRenderer>().sprite = cursorSprite;
                cursor.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }

            // If the player presses the left mouse button and can place a tile down
            if (Input.GetMouseButtonDown(0) && tilesCanPress != 0 && (tile != MapManager.Instance.endOverlayTile && tile != MapManager.Instance.startOverlayTile)
                && (tile.GetComponent<OverlayTile>().isBlocked == false))
            {
                // Block the tile and allocate tiles pressed and remaining
                tile.ShowWallTile();
                tile.isBlocked = true;
                tilesCanPress -= 1;
                
                // If the game is running, reveal the path
                if (isRunning) // If the play button has been pressed
                {
                    RevealPath(); // Reveal the path
                }               
            }
        }
        
        // If the cursor is not hovering over a tile, hide it
        else
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Hide the cursor
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
        var step = speedOfCharacter * Time.deltaTime;
        float zIndex = path[0].transform.position.z;
        MapManager.Instance.character.transform.position = Vector2.MoveTowards(MapManager.Instance.character.transform.position, path[0].transform.position, step);
        MapManager.Instance.character.transform.position = new Vector3(MapManager.Instance.character.transform.position.x, MapManager.Instance.character.transform.position.y, zIndex);
        if (Vector2.Distance(MapManager.Instance.character.transform.position, path[0].transform.position) < 0.00001f)
        {
            // Hide the tile when the character crosses it and remove it from the path
            path[0].HidePathTile();
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        // Position the character on the tile
        MapManager.Instance.character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        MapManager.Instance.character.GetComponent<CharacterManager>().standingOnTile = tile;
    }

    private void RevealPath()
    {
        // Shows and reveals tiles in the path as well as generates the path
        foreach (OverlayTile tile in path)
        {
            if (tile.GetComponent<SpriteRenderer>().sprite != OverlayTile.staticWallSprite)            
            {
                tile.HidePathTile();
            }
        }
        path = pathFinder.FindPath(MapManager.Instance.character.GetComponent<CharacterManager>().standingOnTile, MapManager.Instance.endOverlayTile);
        foreach (OverlayTile tile in path)
        {
            tile.ShowPathTile();
        }
    }
}
