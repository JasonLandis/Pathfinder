using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A* algorithm implementation

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        // Finds the path from the start tile to the end tile
        List<OverlayTile> openList = new();
        List<OverlayTile> closedList = new();
        openList.Add(start);
        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();
            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);
            if (currentOverlayTile == end)
            {
                return GetFinishedList(start, end);
            }
            foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
            {
                if (tile.isBlocked || closedList.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                {                    
                    continue;
                }                
                tile.G = GetManhattanDistance(start, tile);
                tile.H = GetManhattanDistance(end, tile);
                tile.Previous = currentOverlayTile;                
                if (!openList.Contains(tile))
                {                   
                    openList.Add(tile);
                }
            }
        }
        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        // Returns the finished list of overlay tiles in the path
        List<OverlayTile> finishedList = new();
        OverlayTile currentTile = end;
        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }
        finishedList.Reverse();
        return finishedList;
    }

    private int GetManhattanDistance(OverlayTile start, OverlayTile tile)
    {
        // Manhattan distance
        return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
    }

    private List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile)
    {
        // Evaluates the neighbor tiles of the current tile
        var map = MapManager.Instance.map;
        List<OverlayTile> neighbours = new();

        Vector2Int locationToCheck = new(currentOverlayTile.gridLocation.x + 1,currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }
}
