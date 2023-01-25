using UnityEngine;
using System.Linq;
using System.Collections.Generic;

// Controls objects associated with the level during runtime
// Also controls specific level settings

public class LevelManager : MonoBehaviour
{
    [Header("LevelProperties")]
    public float speedOfCharacter; // the speed of the character
    public float timer; // the timer for the character to move   
    public GameObject cursor; // The cursor icon
    public int tilesCanPress; // The number of tiles the player can press
    public int tilesPressed; // The number of tiles pressed

    private PathFinder pathFinder; // the path finder
    private List<OverlayTile> path; // the path the character is currently following
    private GameObject character; // the character 
    private OverlayTile destinationTile; // the destination tile

    // static variables
    public static float staticTimer;
    public static int tilesRemaining;

    private void Start()
    {
        pathFinder = new PathFinder(); // Initialize the path finder
        path = new List<OverlayTile>(); // Initialize the path
        staticTimer = timer; // Set the static timer
        tilesRemaining = tilesCanPress; // Set the static tiles can press to the tiles can press
    }

    private void Update()
    {
        if (UIManager.isRunning) // If the play button has been pressed
        {
            staticTimer -= Time.deltaTime; // Subtract the time from the timer
            character = MapManager.Instance.character; // Get the character
            destinationTile = MapManager.Instance.endOverlayTile; // get the destination tile

            path = pathFinder.FindPath(character.GetComponent<CharacterManager>().standingOnTile, destinationTile); // Find the path

            if (path.Count > 0)
            {
                MoveAlongPath(); // Move along the path
            }
        }

        if (staticTimer <= 0) // If the timer is 0
        {
            UIManager.isRunning = false; // Set the play pressed to false
            staticTimer = 0; // Set the timer to 0
        }

        RaycastHit2D? hit = GetFocusedOnTile(); // Get the tile the cursor is focused on

        if (hit.HasValue)
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // Show the cursor
            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>(); // If the cursor is focused on a tile, show the cursor icon and reveal the tile
            cursor.transform.position = tile.transform.position; // Set the position of the cursor icon to the position of the tile
            cursor.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                if (tilesPressed < tilesCanPress)
                {
                    tile.ShowTile(); // Show the tile
                    tile.isBlocked = true; // Set the tile to be blocked
                    tilesPressed += 1; // Add 1 to the number of tiles pressed
                    tilesRemaining -= 1; // Subtract 1 from the number of tiles that can be pressed
                }
            }
        }
        else
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Hide the cursor
        }
    }
    
    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y); // Convert the mouse position to a Vector2

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero); // Get all the tiles the mouse is focused on

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First(); // Return the tile with the highest z index
        }

        return null;
    }

    private void MoveAlongPath()
    {
        var step = speedOfCharacter * Time.deltaTime; // the step
        float zIndex = path[0].transform.position.z; // the z index of the tile the character is moving to
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step); // Move the character towards the tile
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex); // Set the z index of the character to the z index of the tile the character is moving to

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]); // Position the character on the line
            path.RemoveAt(0); // Remove the tile from the path
        }

    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        // Position the character on the line
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.GetComponent<CharacterManager>().standingOnTile = tile; // Set the tile the character is standing on
    }
}
