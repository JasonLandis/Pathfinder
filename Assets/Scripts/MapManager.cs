using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // when the player clicks on the map, debug.Log the tile that was clicked
    public void OnMapClick(Vector3 position)
    {
        Debug.Log("Map Clicked at: " + position);
    }
}
