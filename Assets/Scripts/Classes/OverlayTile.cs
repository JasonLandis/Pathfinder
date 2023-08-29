using UnityEngine;
using UnityEngine.Tilemaps;

// Properties for each overlay tile generated on the map

public class OverlayTile : MonoBehaviour
{
    [Header("Pathfinding Properties")]
    public int G; // The distance from the start tile
    public int H; // The distance from the end tile
    public int F { get { return G + H; } } // The total distance cost
    public bool isBlocked; // Is the tile blocked
    public OverlayTile Previous; // The previous tile
    public Vector3Int gridLocation; // The grid location of the tile

    [Header("Tile Properties")]
    public Sprite redWallSprite; // The sprite for the red wall
    public Sprite blueWallSprite; // The sprite for the blue wall
    public Sprite yellowWallSprite; // The sprite for the yellow wall
    public Sprite pathSprite; // The sprite for the path

    public void ShowRedTile()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = redWallSprite;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    public void ShowBlueTile()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = blueWallSprite;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    public void ShowYellowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = yellowWallSprite;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    public void ShowPathTile()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = pathSprite;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0);
    }
}