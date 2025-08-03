using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


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
	public InputActionReference flipAction;
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
	BallController m_ball;

	// private variables
	private bool m_lastjumpinput = true;
	private bool m_touchingGround = false;
	private bool m_touchingLeftWall = false;
	private bool m_touchingRightWall = false;

	SpriteRenderer spriteRenderer;

	[SerializeField]
	Sprite idleSprite;
	[SerializeField]
	Sprite walkSprite;
	[SerializeField]
	Sprite jumpSprite;
	[SerializeField]
	Sprite slideSprite;

	[SerializeField]
	float walkCycleSpeed;

	Timer timer;

	[HideInInspector]
	public int UID;

	// used to get components
	private void Awake() 
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_boxCollider = GetComponent<BoxCollider2D>();
		m_ball = GetComponentInChildren<BallController>();
		timer = GetComponent<Timer>();

		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// initialization
	private void Start() 
	{
		if (m_stats == null) {
			enabled = false;
			Debug.LogError("stats was not set on player \"" + name + "\"");
			return;
		}

		timer.onTimerFinished.AddListener(HandleWalkAnimation);
	}

	// update is called once per frame
	void Update() 
	{
		// send input to ball
		if (damageAction.action.WasPressedThisFrame()) 
		{
			m_ball.StopBall();
		}

		if (flipAction.action.WasPressedThisFrame())
		{
			m_ball.FlipRotationScale();
		}

		HandleAnimation();
	}

	// used to handle physics
	void FixedUpdate() 
	{
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

	private void HandlePhysics(float delta, Vector2 inputDirection, bool inputJumpHeld, bool inputPressedJump) 
	{
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

		if (!m_touchingGround && (_velocity.y < Util.very_small)) 
		{
			if ((inputDirection.x < 0.0f) && m_touchingLeftWall) 
			{
				_targetMaxFallSpeed = m_stats.maxSlideSpeed;
				_targetFallAcceleration = m_stats.slideAcceleration;
			}
			if ((inputDirection.x > 0.0f) && m_touchingRightWall) 
			{
				_targetMaxFallSpeed = m_stats.maxSlideSpeed;
				_targetFallAcceleration = m_stats.slideAcceleration;
			}
		}

		_velocity.y -= _targetFallAcceleration * delta;
		_velocity.y = Mathf.Max(_velocity.y, -_targetMaxFallSpeed);

		// handle jump

		// touching ground
		if (inputPressedJump && m_touchingGround) 
		{
			AudioManager.instance.PlaySound(AudioManager.instance.jumpSound, transform.position, 1.0f, 0.0f, false);
			_velocity.y = m_stats.jumpInitialSpeed;
		}
		// else touching either wall (using != garuntees its one *or* the other)
		else if (inputPressedJump && (m_touchingLeftWall != m_touchingRightWall)) 
		{

			AudioManager.instance.PlaySound(AudioManager.instance.jumpSound, transform.position, 1.0f, 0.0f, false);
			float direction = (m_touchingLeftWall ? 1.0f : -1.0f);

			_velocity.y = m_stats.wallJumpInitialSpeed;
			_velocity.x = direction * m_stats.wallJumpLaunchSpeed;
		}

		// set velocity
		m_rigidbody.linearVelocity = _velocity;
	}

	// wasnt sure what to name this lol

	//Fixed it
	void SurfaceCheck() 
	{
		m_touchingGround = IsTouchingSurface(Vector2.down);
		m_touchingLeftWall = IsTouchingSurface(Vector2.left);
		m_touchingRightWall = IsTouchingSurface(Vector2.right);
	}

	bool IsTouchingSurface(Vector2 direction) 
	{
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
		AudioManager.instance.PlaySound(AudioManager.instance.damageSound, transform.position, 0.4f, 1.0f, false);
		GameManager.instance.TakeDamage(this);
		GameManager.instance.ScreenShake();

	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col != null)
		{
			if (col.CompareTag("Spikes"))
			{
				OnTakeDamage();
			}
		}
	}

	void HandleAnimation()
	{
		Vector2 inputDirection = moveAction.action.ReadValue<Vector2>();


		if (inputDirection.x > 0)
		{
			spriteRenderer.flipX = false;
		}
		else if (inputDirection.x < 0)
		{
			spriteRenderer.flipX = true;
		}

		if (m_touchingLeftWall || m_touchingRightWall)
		{
			spriteRenderer.sprite = slideSprite;
			timer.Pause();
			return;
		}
		else if (inputDirection.x != 0 && m_touchingGround)
		{
			//Walk Anim
			if (timer.IsRunning() == false)
			{
				timer.StartTimer(walkCycleSpeed);
			}
			return;
		}
		else if (m_touchingGround == false)
		{
			spriteRenderer.sprite = jumpSprite;
			timer.Pause();
			return;
		}
		else if (inputDirection.x == 0 && m_touchingGround)
		{
			spriteRenderer.sprite = idleSprite;
			timer.Pause();
			return;
		}
	}

	void HandleWalkAnimation()
	{
		if (spriteRenderer.sprite == idleSprite)
		{
			spriteRenderer.sprite = walkSprite;
		}
		else
		{
			spriteRenderer.sprite = idleSprite;
		}
	}

}
