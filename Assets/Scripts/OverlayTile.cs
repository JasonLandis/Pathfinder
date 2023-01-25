using System;
using UnityEngine;

// Properties for each overlay tile generated on the map

public class OverlayTile : MonoBehaviour
{
    [Header("Pathfinding Properties")]
    public int G; // The distance from the start tile
    public int H; // The distance from the end tile
    public int F { get { return G + H; } } // The total distance cost

    [Header("Tile Properties")]
    public bool isBlocked = false; // Is the tile blocked
    public OverlayTile Previous; // The previous tile
    public Vector3Int gridLocation; // The grid location of the tile

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // Show the overlay tile
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Hide the overlay tile
    }
}