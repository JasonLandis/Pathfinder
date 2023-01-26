using UnityEngine;

// Properties for each overlay tile generated on the map

public class OverlayTile : MonoBehaviour
{
    [Header("Pathfinding Properties")]
    public int G; // The distance from the start tile
    public int H; // The distance from the end tile
    public int F { get { return G + H; } } // The total distance cost
    public bool isBlocked = false; // Is the tile blocked
    public OverlayTile Previous; // The previous tile
    public Vector3Int gridLocation; // The grid location of the tile

    [Header("Tile Properties")]
    public Sprite wallSprite; // The sprite for the wall
    public Sprite pathSprite; // The sprite for the path
    public static Sprite staticWallSprite; // The static sprite for the wall

    private void Start()
    {
        staticWallSprite = wallSprite;
    }

    public void ShowWallTile()
    {
        // Shows the red tile
        gameObject.GetComponent<SpriteRenderer>().sprite = wallSprite;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    public void HideWallTile()
    {
        // Hides the red tile
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0);
    }

    public void ShowPathTile()
    {
        // Shows the white tile
        gameObject.GetComponent<SpriteRenderer>().sprite = pathSprite;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void HidePathTile()
    {
        // Hides the white tile
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}