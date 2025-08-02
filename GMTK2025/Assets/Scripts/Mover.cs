using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float moveSpeed;
    Vector2 direction;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (direction.magnitude > Util.very_small)
		{
			rb.linearVelocity = new Vector2(direction.x * moveSpeed * Time.fixedDeltaTime, direction.y * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetDirection(Vector2 dir_)
    {
        Debug.Log("Direction set");
        direction = dir_;
    }
}
