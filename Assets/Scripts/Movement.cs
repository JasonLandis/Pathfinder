using System.Collections.Generic;
using UnityEngine;

// Controls the movement of the character when the mouse is clicked on a tile

public class Movement : MonoBehaviour
{
    public float speed; // the speed of the character

    private PathFinder pathFinder; // the path finder
    private List<OverlayTile> path; // the path the character is currently following
    private GameObject character; // the character 
    private OverlayTile destinationTile; // the destination tile

    private void Start()
    {
        pathFinder = new PathFinder(); // Initialize the path finder
        path = new List<OverlayTile>(); // Initialize the path
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            character = MapManager.Instance.character; // Get the character
            destinationTile = MapManager.Instance.endOverlayTile; // get the destination tile

            path = pathFinder.FindPath(character.GetComponent<CharacterInfo>().standingOnTile, destinationTile); // Find the path
        }
        if (path.Count > 0)
        {
            MoveAlongPath(); // Move along the path
        }             
    }
    
    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime; // the step
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
        character.GetComponent<CharacterInfo>().standingOnTile = tile; // Set the tile the character is standing on
    }
}
