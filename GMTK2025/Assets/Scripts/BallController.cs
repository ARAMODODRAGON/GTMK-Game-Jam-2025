using UnityEngine;
using UnityEngine.Splines;

public class BallController : MonoBehaviour
{
	[SerializeField]
	float ballStopTime;
	[SerializeField]
	bool shouldBallDamageFor1Frame;
	bool canDamage = false;
	bool canRotate;

	float currentTimer;
	bool isRunningTimer = false;

	SplineAnimate spline;
	CircleCollider2D circleCollider;

	SpriteRenderer spriteRenderer;

	[SerializeField]
	Timer ballDamageWindowTimer;
	[SerializeField]
	Timer ballFlipWindowTimer;

	[SerializeField]
	float moveSpeed;

	float angle = 0;
	[SerializeField]
	float rotationScale;
	bool canFlip;
	[SerializeField]
	float ballFlipTimer;

	private void Start()
	{
		spline = GetComponent<SplineAnimate>();
		circleCollider = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		ballDamageWindowTimer = GetComponent<Timer>();

		circleCollider.enabled = false;
		ballDamageWindowTimer.onTimerFinished.AddListener(DisableBallDamage);
		ballFlipWindowTimer.onTimerFinished.AddListener(ResetFlipWindow);

		spriteRenderer.color = Color.blue;
		canRotate = true;
		canFlip = true;
	}

	private void FixedUpdate()
	{
		
        if (canRotate)
        {
			angle += (moveSpeed * Time.fixedDeltaTime * rotationScale);
			transform.parent.localEulerAngles = new Vector3(transform.parent.localEulerAngles.x, transform.parent.localEulerAngles.y, angle);
		}

	}

	public void StopBall()
	{
		if (!isRunningTimer)
		{
			canDamage = true;
			spriteRenderer.color = Color.red;
			currentTimer = ballStopTime;
			isRunningTimer = true;
			canRotate = false;
			circleCollider.enabled = true;

			if (shouldBallDamageFor1Frame)
			{
				ballDamageWindowTimer.StartTimer(0.1f); ;
			}
		}

	}

	void StartBall()
	{
		//Debug.Log("Start Ball");

		spriteRenderer.color = Color.blue;
		isRunningTimer = false;
		circleCollider.enabled = false;
		spline.Play();
		canDamage = false;
		canRotate = true;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (canDamage == false || col == this)
		{
			return;
		}

		IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
		if (damageable != null)
		{
			damageable.OnTakeDamage();
		}
	}

	private void DisableBallDamage()
	{
		spriteRenderer.color = Color.lightBlue;
		canDamage = false;
	}

	public void FlipRotationScale()
	{
		if (canFlip)
		{
			rotationScale *= -1;
			canFlip = false;
			ballFlipWindowTimer.StartTimer(ballFlipTimer);
		}

	}

	void ResetFlipWindow()
	{
		canFlip = true;
	}

	void Update()
	{
		if (currentTimer > 0)
		{
			currentTimer -= Time.deltaTime;
			if (currentTimer <= 0)
			{
				StartBall();
			}
		}
	}
}
