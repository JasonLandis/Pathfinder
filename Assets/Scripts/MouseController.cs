using UnityEngine;
using System.Linq;

// Controls cursor icon and reveals overlay tile when clicked on

public class MouseController : MonoBehaviour
{
    public GameObject cursor; // The cursor icon

    private void Update()
    {
        RaycastHit2D? hit = GetFocusedOnTile(); // Get the tile the cursor is focused on

        if (hit.HasValue)
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // Show the cursor
            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>(); // If the cursor is focused on a tile, show the cursor icon and reveal the tile
            cursor.transform.position = tile.transform.position; // Set the position of the cursor icon to the position of the tile
            cursor.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                if (tile.isBlocked)
                {
                    tile.HideTile(); // Hide the tile
                    tile.isBlocked = false; // Set the tile to not be blocked
                }
                else
                {
                    tile.ShowTile(); // Show the tile
                    tile.isBlocked = true; // Set the tile to be blocked
                }                
            }
        }
        else
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Hide the cursor
        }
    }
    
    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y); // Convert the mouse position to a Vector2

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero); // Get all the tiles the mouse is focused on

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First(); // Return the tile with the highest z index
        }

        return null;
    }
}
