using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Splines;


public interface IDamageable
{
	void OnTakeDamage();
}

public class PlayerController : MonoBehaviour, IDamageable {

	// input
	[Header("Input")]
	public InputActionReference moveAction;
	public InputActionReference jumpAction;
	public InputActionReference damageAction;
	//PlayerInput playerInput;

	// components
	private BoxCollider2D m_boxCollider;
	private Rigidbody2D m_rigidbody;
	//private SplineAnimate m_spline;

	// physics
	[Header("Physics")]
	[SerializeField] private PlayerStats m_stats;
	[SerializeField] private LayerMask m_solidLayer;
	[SerializeField] private float m_touchingDistance;

	// references
	[Header("References")]
	[SerializeField] private BallController m_ball;

	// private variables
	private bool m_lastjumpinput = true;
	private bool m_touchingGround = false;
	private bool m_touchingLeftWall = false;
	private bool m_touchingRightWall = false;

	[HideInInspector]
	public int UID;

	// used to get components
	private void Awake() {
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_boxCollider = GetComponent<BoxCollider2D>();

		//if (m_ball != null) {
		//	m_spline = m_ball.gameObject.GetComponent<SplineAnimate>();
		//}
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

		// calls to handle physics
		SurfaceCheck();
		HandlePhysics(Time.fixedDeltaTime, _inputDirection, _inputJump, _inputPressedJump);

		// store jump input for next step
		m_lastjumpinput = _inputJump;
	}

	private void HandlePhysics(float delta, Vector2 inputDirection, bool inputJumpHeld, bool inputPressedJump) {
		// get velocity
		Vector2 _velocity = m_rigidbody.linearVelocity;

		// handle horizontal
		float _targetMaxSpeed = m_stats.maxSpeed * inputDirection.x;
		_velocity.x = Util.MoveToward(
			_velocity.x,
			_targetMaxSpeed,
			m_stats.acceleration * delta
		);

		// handle gravity 
		float _targetFallMultiplier = 1.0f;
		if (!inputJumpHeld && _velocity.y > 0.0f) 
			_targetFallMultiplier = m_stats.fallMultiplier;

		float _targetFallAcceleration = m_stats.fallAcceleration * _targetFallMultiplier;
		float _targetMaxFallSpeed = m_stats.maxFallSpeed;

		if (!m_touchingGround && (_velocity.y < Util.very_small)) {
			if ((inputDirection.x < 0.0f) && m_touchingLeftWall) {
				_targetMaxFallSpeed = m_stats.maxSlideSpeed;
				_targetFallAcceleration = m_stats.slideAcceleration;
			}
			if ((inputDirection.x > 0.0f) && m_touchingRightWall) {
				_targetMaxFallSpeed = m_stats.maxSlideSpeed;
				_targetFallAcceleration = m_stats.slideAcceleration;
			}
		}

		_velocity.y -= _targetFallAcceleration * delta;
		_velocity.y = Mathf.Max(_velocity.y, -_targetMaxFallSpeed);

		// handle jump

		// touching ground
		if (inputPressedJump && m_touchingGround) {
			_velocity.y = m_stats.jumpInitialSpeed;
		}
		// else touching either wall (using != garuntees its one *or* the other)
		else if (inputPressedJump && (m_touchingLeftWall != m_touchingRightWall)) {
			float direction = (m_touchingLeftWall ? 1.0f : -1.0f);

			_velocity.y = m_stats.wallJumpInitialSpeed;
			_velocity.x = direction * m_stats.wallJumpLaunchSpeed;
		}

		// set velocity
		m_rigidbody.linearVelocity = _velocity;
	}

	// wasnt sure what to name this lol

	//Fixed it
	void SurfaceCheck() {
		m_touchingGround = IsTouchingSurface(Vector2.down);
		m_touchingLeftWall = IsTouchingSurface(Vector2.left);
		m_touchingRightWall = IsTouchingSurface(Vector2.right);
	}

	bool IsTouchingSurface(Vector2 direction) {
		RaycastHit2D _hit = Physics2D.BoxCast(
			m_boxCollider.bounds.center,
			m_boxCollider.bounds.size,
			0f,
			direction,
			m_touchingDistance,
			m_solidLayer
		);

		return _hit;
	}

	public void OnTakeDamage()
	{
		// Filler function for now, will add proper damage logic later.

		Debug.Log("Damage Taken");

		GameManager.instance.TakeDamage(this);
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
