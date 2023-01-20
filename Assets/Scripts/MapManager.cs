using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance; // creates a static instance of the MapManager class
    public static MapManager Instance { get { return _instance; } } // creates a public static instance of the MapManager class

    public OverlayTile overlayTilePrefab; // creates a public variable for the overlay tile prefab
    public GameObject overlayContainer; // creates a public variable for the overlay container

    public Dictionary<Vector2Int, OverlayTile> map; // creates a public dictionary for the overlay tiles

    private void Awake()
    {
        if (_instance != null && _instance != this) // if there is already an instance of the MapManager class
        {
            Destroy(this.gameObject); // destroys the current instance of the MapManager class
        }
        else
        {
            _instance = this; // sets the current instance of the MapManager class to the static instance
        }
    }

    void Start()
    {
        var tileMap = gameObject.GetComponentInChildren<Tilemap>(); // gets the tilemap component
        map = new Dictionary<Vector2Int, OverlayTile>(); // creates a new dictionary
        BoundsInt bounds = tileMap.cellBounds; // gets the bounds of the tilemap

        for (int z = bounds.max.z; z > bounds.min.z; z--) // loops through the z axis of the tilemap
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++) // loops through the y axis of the tilemap
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++) // loops through the x axis of the tilemap
                {
                    var tileLocation = new Vector3Int(x, y, z); // creates a new vector3int for the tile location
                    var tileKey = new Vector2Int(x, y); // creates a new vector2int for the tile key

                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey)) // if the tilemap has a tile at the tile location and the map does not contain the tile key
                    {
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform); // creates a new overlay tile
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation); // gets the world position of the tile

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1); // sets the position of the overlay tile
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder; // sets the sorting order of the overlay tile
                        map.Add(tileKey, overlayTile); // adds the tile key and overlay tile to the map
                    }
                }
            }
        }
    }
}
