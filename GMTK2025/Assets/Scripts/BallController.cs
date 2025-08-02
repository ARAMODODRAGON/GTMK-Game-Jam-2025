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

	Timer timer;

	[SerializeField]
	float moveSpeed;

	float angle = 0;
	[SerializeField]
	float rotationScale;

	private void Start()
	{
		spline = GetComponent<SplineAnimate>();
		circleCollider = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		timer = GetComponent<Timer>();

		circleCollider.enabled = false;
		timer.onTimerFinished.AddListener(DisableBallDamage);

		spriteRenderer.color = Color.blue;
		canRotate = true;
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
				timer.StartTimer(0.1f); ;
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
		canDamage = false;
	}

	public void FlipRotationScale()
	{
		rotationScale *= -1;
	}

	void Update()
	{
		if (currentTimer > 0)
		{
			//Debug.Log("CountingDown");
			currentTimer -= Time.deltaTime;
			if (currentTimer <= 0)
			{
				StartBall();
			}
		}
	}
}
