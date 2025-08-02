using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float moveSpeed;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir_)
    {
        rb.linearVelocity = new Vector2(dir_.x * moveSpeed * Time.fixedDeltaTime, dir_.y * moveSpeed * Time.fixedDeltaTime);
    }
}
