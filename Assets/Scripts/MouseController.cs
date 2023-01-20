using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile(); // gets the tile the mouse is hovering over

        if (focusedTileHit.HasValue) // if the mouse is hovering over a tile
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // shows the cursor tile
            GameObject overlayTile = focusedTileHit.Value.collider.gameObject; // gets the tile the mouse is hovering over
            transform.position = overlayTile.transform.position; // moves the overlay tile to the tile the mouse is hovering over
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0)) // if left mouse button is clicked
            {
                overlayTile.GetComponent<OverlayTile>().ShowTile(); // shows the tile
            }
        } else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // hides the cursor tile
        }
    }

    public RaycastHit2D? GetFocusedOnTile() // gets the tile that the mouse is currently over
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // gets the mouse position
        Vector2 mousePos2D = new(mousePos.x, mousePos.y); // converts the mouse position to a 2D vector
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero); // gets all the tiles that the mouse is over

        if (hits.Length > 0) // if there are tiles that the mouse is over
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First(); // returns the tile that is closest to the camera
        }

        return null;
    }
}
