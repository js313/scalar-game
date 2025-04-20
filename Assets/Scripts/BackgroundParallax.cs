using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    Camera cam;
    SpriteRenderer sr;

    [SerializeField] float parallaxSpeed;
    float xPosition;
    float len;


    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        xPosition = transform.position.x;
        len = sr.bounds.size.x;
    }

    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxSpeed);
        float distanceToMove = cam.transform.position.x * parallaxSpeed;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y, transform.position.z);
        if (distanceMoved > len + xPosition)
        {
            xPosition += len;
        }
    }
}
