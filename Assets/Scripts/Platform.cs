using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    public Vector3 GetStartPoint() { return new Vector3(startPoint.position.x, 0, 0); }
    public Vector3 GetEndPoint() { return new Vector3(endPoint.position.x, 0, 0); }

    void Start()
    {

    }

    void Update()
    {

    }
}
