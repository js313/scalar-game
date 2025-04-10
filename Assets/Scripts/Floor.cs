using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private GameObject floorSurface;
    [SerializeField] private float floorSurfaceThickness;
    [SerializeField] private Color floorHighlightColor;
    SpriteRenderer floorRenderer;
    SpriteRenderer sr;

    private void Start()
    {
        floorRenderer = floorSurface.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        floorSurface.transform.parent = transform.parent;
        floorSurface.transform.localScale = new Vector3(floorSurface.transform.localScale.x, floorSurfaceThickness, floorSurface.transform.localScale.z);
        floorSurface.transform.position = new Vector3(
            floorSurface.transform.position.x, 
            transform.position.y + sr.bounds.size.y / 2 - floorRenderer.bounds.size.y / 2, 
            floorSurface.transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        floorRenderer.color = floorHighlightColor;
    }
}
