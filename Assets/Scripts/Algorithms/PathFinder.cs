using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A* algorithm implementation

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> openList = new List<OverlayTile>(); // Tiles to be evaluated
        List<OverlayTile> closedList = new List<OverlayTile>(); // Tiles that have been evaluated

        openList.Add(start); // Add the starting tile to the open list

        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First(); // Get the tile with the lowest F value

            openList.Remove(currentOverlayTile); // Remove the current tile from the open list
            closedList.Add(currentOverlayTile); // Add the current tile to the closed list

            if (currentOverlayTile == end)
            {
                return GetFinishedList(start, end); // Return the finished list
            }

            foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
            {
                // If the tile is blocked, is already in the closed list or is too high or too low, skip it
                if (tile.isBlocked || closedList.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                {                    
                    continue;
                }

                tile.G = GetManhattenDistance(start, tile); // Calculate the G value
                tile.H = GetManhattenDistance(end, tile); // Calculate the H value
                tile.Previous = currentOverlayTile; // Set the previous tile

                if (!openList.Contains(tile)) // If the tile is not in the open list, add it
                {                   
                    openList.Add(tile);
                }
            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>(); // The finished list
        OverlayTile currentTile = end; // The current tile

        while (currentTile != start)
        {
            finishedList.Add(currentTile); // Add the current tile to the finished list
            currentTile = currentTile.Previous; // Set the current tile to the previous tile
        }

        finishedList.Reverse(); // Reverse the list so it starts at the start tile

        return finishedList;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile tile)
    {
        return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y); // Return the manhatten distance
    }

    private List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map; // Get the map

        List<OverlayTile> neighbours = new List<OverlayTile>(); // The list of neighbours

        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1,currentOverlayTile.gridLocation.y); // Checks right tile

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // Add the right tile to the list
        }

        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y); // Checks left tile

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // Add the left tile to the list
        }

        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1); // Checks top tile

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // Add the top tile to the list
        }

        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1); // Checks bottom tile

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // Add the bottom tile to the list
        }

        return neighbours;
    }
}
