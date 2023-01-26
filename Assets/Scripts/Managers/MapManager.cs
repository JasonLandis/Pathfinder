using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

// This class is used to generate overlay tiles on the map when the game starts
// It also collects the start and end tiles on the map

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

    private void Awake()
    {
        // Initialize the instance
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
        // Initialize the map
        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        map = new Dictionary<Vector2Int, OverlayTile>();
        foreach (var tm in tileMaps)
        {
            BoundsInt bounds = tm.cellBounds;
            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        if (z == 0 && ignoreBottomTiles)
                            return;

                        if (tm.HasTile(new Vector3Int(x, y, z)))
                        {                            
                            if (!map.ContainsKey(new Vector2Int(x, y)))
                            {                                
                                if (tm.GetTile(new Vector3Int(x, y, z)) == startTile)
                                {
                                    startTileLocation = new Vector3Int(x, y, z);                                 
                                }
                                else if (tm.GetTile(new Vector3Int(x, y, z)) == endTile)
                                {
                                    endTileLocation = new Vector3Int(x, y, z);
                                }
                                var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z));
                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder;
                                overlayTile.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z);
                                map.Add(new Vector2Int(x, y), overlayTile.GetComponent<OverlayTile>());
                            }
                        }
                    }
                }
            }
        }
        // Gets the start and end overlay tiles
        startOverlayTile = map[new Vector2Int(startTileLocation.x, startTileLocation.y)];
        endOverlayTile = map[new Vector2Int(endTileLocation.x, endTileLocation.y)];
        character.GetComponent<CharacterManager>().standingOnTile = startOverlayTile;
    }
}
