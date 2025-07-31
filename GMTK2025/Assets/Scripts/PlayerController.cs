using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour
{

	//InputAction moveAction;

	public InputActionReference moveAction;
	public InputActionReference jumpAction;
	public InputActionReference damageAction;

	Vector2 moveDirection;

	PlayerInput playerInput;

	Rigidbody2D rb;

	[SerializeField]
	float moveSpeed;

	BoxCollider2D boxCollider;

	[SerializeField]
	LayerMask groundLayer;

	public float groundCheckDistance = 1f;

	public float jumpHeight;

	[SerializeField]
	BallController ball;

	SplineAnimate spline;

	// Start is called once before the first execution of Update after the MonoBehaviour is created

	private void Awake()
	{
	}

	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();

		if (ball != null)
		{
			spline = ball.gameObject.GetComponent<SplineAnimate>();
		}
	}

    // Update is called once per frame
    void Update()
    {
		UpdateMovementVector();
		CheckGround();
		HandleJumping();
		HandleDamage();
	}

	void UpdateMovementVector()
	{
		moveDirection = moveAction.action.ReadValue<Vector2>();

		//Debug.Log(moveDirection.x + " " + moveDirection.y);
	}

	bool CheckGround()
	{
		RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundCheckDistance, groundLayer);

		Color colour;

		if (hit.collider != null)
		{
			colour = Color.green;
			Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
			Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
			Debug.DrawRay(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y), Vector2.right * (boxCollider.bounds.extents.x), colour);

			return true;
		}
		else
		{
			colour = Color.red;
		}

		Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
		Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
		Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + groundCheckDistance), Vector2.right * (boxCollider.bounds.extents.x), colour);


		return false;
	}

	void HandleJumping()
	{
		if (jumpAction.action.WasPressedThisFrame() && CheckGround())
		{
			Debug.Log("Jump");
			rb.AddForceY(jumpHeight , ForceMode2D.Impulse);
		}
	}

	void HandleDamage()
	{
		if (damageAction.action.WasPressedThisFrame())
		{
			ball.StopBall();
		}
	}

	private void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, rb.linearVelocityY);

		//rb.AddForceX(moveDirection.x * moveSpeed, ForceMode2D.Force);
	}
}
