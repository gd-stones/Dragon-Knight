
using UnityEngine;

public class MoveObject2D : MonoBehaviour
{
    public Transform targetPosition;
    public float speed = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = targetPosition.position - transform.position;
        print("direction " + direction);
        print("direction.normalized " + direction.normalized);
        rb.velocity = direction.normalized * speed;
    }
}
