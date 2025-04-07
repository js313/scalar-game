using System.Linq;
using UnityEngine;

public class MovingTrap : Trap
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3[] movePoints;
    private int nextIndex = 0;

    private void Start()
    {
        movePoints = transform.GetComponentsInChildren<Transform>().Where(t => t != transform).Select(t => t.position).ToArray();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[nextIndex], moveSpeed * Time.deltaTime);
        if (Vector3.SqrMagnitude(transform.position - movePoints[nextIndex]) < 0.01f)
            nextIndex = (nextIndex + 1) % movePoints.Length;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - rotationSpeed * Time.deltaTime);
    }
}
