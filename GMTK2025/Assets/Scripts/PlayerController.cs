using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour {

	// input
	[SerializeField] private InputActionReference moveAction;
	[SerializeField] private InputActionReference jumpAction;
	[SerializeField] private InputActionReference damageAction;
	//PlayerInput playerInput;

	// components
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb;
	private SplineAnimate spline;

	// physics
	[SerializeField] private PlayerStats m_stats;
	[SerializeField] private LayerMask m_solidLayer;
	[SerializeField] private float m_touchingDistance;

	// references
	[SerializeField] private BallController m_ball;

	// private variables
	private bool m_lastjumpinput = true;

	// used to get components
	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();

		if (m_ball != null) {
			spline = m_ball.gameObject.GetComponent<SplineAnimate>();
		}
	}

	// initialization
	private void Start() {
		if (m_stats == null) {
			enabled = false;
			Debug.LogError("stats was not set on player \"" + name + "\"");
			return;
		}
	}

	// update is called once per frame
	void Update() {
		// send input to ball
		if (damageAction.action.WasPressedThisFrame()) {
			m_ball.StopBall();
		}
	}

	// used to handle physics
	void FixedUpdate() {
		if (!m_stats) return; // just make sure we can actually update physics lol

		// get our input
		Vector2 _inputDirection = moveAction.action.ReadValue<Vector2>();
		bool _inputJump = jumpAction.action.IsPressed();
		bool _inputPressedJump = (_inputJump == true) && (m_lastjumpinput == false);

		// call to handle physics
		HandlePhysics(Time.fixedDeltaTime, _inputDirection, _inputJump);

		// store jump input for next step
		m_lastjumpinput = _inputJump;
	}

	private void HandlePhysics(float delta, Vector2 inputDirection, bool inputPressedJump) {
		Vector2 velocity = rb.linearVelocity;

		// handle horizontal
		velocity.x = inputDirection.x * m_stats.moveSpeed;

		// handle gravity
		velocity.y -= m_stats.fallAcceleration * delta;

		// handle jump
		if (inputPressedJump && IsTouchingGround()) {
			Debug.Log("Jump");
			velocity.y = m_stats.jumpInitialSpeed;
		}

		// apply velocity
		rb.linearVelocity = velocity;
		//rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + velocity * delta);
	}

	bool IsTouchingGround() {
		return IsTouchingSurface(Vector2.down);
	}

	bool IsTouchingSurface(Vector2 direction) {
		RaycastHit2D _hit = Physics2D.BoxCast(
			boxCollider.bounds.center,
			boxCollider.bounds.size,
			0f,
			direction,
			m_touchingDistance,
			m_solidLayer
		);

		return _hit;
	}

	//bool CheckGround() {
	//	RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundCheckDistance, groundLayer);
	//
	//	Color colour;
	//
	//	if (hit.collider != null) {
	//		colour = Color.green;
	//		Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
	//		Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
	//		Debug.DrawRay(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y), Vector2.right * (boxCollider.bounds.extents.x), colour);
	//
	//		return true;
	//	} else {
	//		colour = Color.red;
	//	}
	//
	//	Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
	//	Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckDistance), colour);
	//	Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + groundCheckDistance), Vector2.right * (boxCollider.bounds.extents.x), colour);
	//
	//
	//	return false;
	//}


}
