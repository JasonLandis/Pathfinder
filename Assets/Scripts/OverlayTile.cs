using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    void Update() // when the left mouse button is clicked, the HideTile function is run
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }
    
    public void ShowTile() // gets the spriterenderer component and sets the opacity to 1
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void HideTile() // gets the spriterenderer component and sets the opacity to 0
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
