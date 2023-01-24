using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

// This class is used to generate overlay tiles on the map when the game starts

public class MapManager : MonoBehaviour
{
    private static MapManager _instance; // Instance of the class
    public static MapManager Instance { get { return _instance; } } // Public access to the instance

    [Header("Setup Properties")]
    public GameObject overlayPrefab; // Prefab for the overlay tiles
    public GameObject overlayContainer; // Container for the overlay tiles
    public TileBase startTile; // the start tile
    public TileBase endTile; // the end tile
    public GameObject character; // the character

    [Header("Map Properties")]
    public OverlayTile startOverlayTile; // the start overlay tile
    public OverlayTile endOverlayTile; // the end overlay tile
    public Vector3Int startTileLocation; // the start tile location
    public Vector3Int endTileLocation; // the end tile location
                                       // 
    public Dictionary<Vector2Int, OverlayTile> map; // Dictionary for the overlay tiles
    public bool ignoreBottomTiles; // Ignore the bottom tiles

    private void Awake() // If there is no instance of the class, set it to this
    {        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        // Get all the tilemaps in the scene and order them by their sorting order
        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);

        map = new Dictionary<Vector2Int, OverlayTile>(); // Initialize the dictionary

        foreach (var tm in tileMaps) // Loops through to get all of the tiles in the tilemap
        {
            BoundsInt bounds = tm.cellBounds; // Get the bounds of the tilemap

            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        if (z == 0 && ignoreBottomTiles) // If the tile is on the bottom layer and we want to ignore it, skip it
                            return;

                        if (tm.HasTile(new Vector3Int(x, y, z))) // If the tilemap has a tile at this location
                        {                            
                            if (!map.ContainsKey(new Vector2Int(x, y))) // if the tile is not in the map
                            {                                
                                if (tm.GetTile(new Vector3Int(x, y, z)) == startTile)
                                {
                                    startTileLocation = new Vector3Int(x, y, z); // Set the start tile location                                 
                                }
                                else if (tm.GetTile(new Vector3Int(x, y, z)) == endTile)
                                {
                                    endTileLocation = new Vector3Int(x, y, z); // Set the end tile location
                                }

                                var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform); // Instantiate the overlay tile
                                var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z)); // Get the world position of the tile
                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1); // Set the position of the overlay tile
                                overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder; // Set the sorting order of the overlay tile
                                overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z); // Set the grid location of the overlay tile
                                map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>()); // Add the overlay tile to the map
                            }
                        }
                    }
                }
            }
        }
        startOverlayTile = map[new Vector2Int(startTileLocation.x, startTileLocation.y)]; // Set the start overlay tile
        endOverlayTile = map[new Vector2Int(endTileLocation.x, endTileLocation.y)]; // Set the end overlay tile
        character.GetComponent<CharacterInfo>().standingOnTile = startOverlayTile; // Set the character's standing on tile to the start tile
    }
}
