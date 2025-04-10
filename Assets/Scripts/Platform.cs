using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform lowestPoint;
    [SerializeField] Transform highestPoint;
    [SerializeField] Transform playerRef;

    private void Start()
    {
        if(playerRef != null) Destroy(playerRef.gameObject);
    }

    public Vector3 GetStartPoint() { return new Vector3(startPoint.position.x, 0, 0); }
    public Vector3 GetEndPoint() { return new Vector3(endPoint.position.x, 0, 0); }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(-1000, 0, 0), new Vector3(1000, 0, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
        Gizmos.DrawLine(new Vector3(startPoint.position.x, -1000, 0), new Vector3(startPoint.position.x, 1000, 0));
        Gizmos.DrawLine(new Vector3(endPoint.position.x, -1000, 0), new Vector3(endPoint.position.x, 1000, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-1000, lowestPoint.position.y, 0), new Vector3(1000, lowestPoint.position.y, 0));
        Gizmos.DrawLine(new Vector3(-1000, highestPoint.position.y, 0), new Vector3(1000, highestPoint.position.y, 0));
    }
}
