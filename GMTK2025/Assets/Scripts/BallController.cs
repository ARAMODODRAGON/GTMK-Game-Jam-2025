using UnityEngine;
using UnityEngine.Splines;

public class BallController : MonoBehaviour
{
	[SerializeField]
	float ballStopTime;
	[SerializeField]
	bool shouldBallDamageFor1Frame;
	[SerializeField]
	float hitStopTime;
	bool canDamage = false;

	float currentTimer;
	bool isRunningTimer = false;

	SplineAnimate spline;
	CircleCollider2D circleCollider;

	SpriteRenderer spriteRenderer;

	HitStop hitStop;

	Timer timer;

	private void Start()
	{
		spline = GetComponent<SplineAnimate>();
		circleCollider = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		hitStop = GetComponent<HitStop>();
		timer = GetComponent<Timer>();

		circleCollider.enabled = false;
		timer.onTimerFinished.AddListener(DisableBallDamage);

		spriteRenderer.color = Color.blue;
	}

	public void StopBall()
	{
		if (!isRunningTimer)
		{
			canDamage = true;
			spriteRenderer.color = Color.red;
			currentTimer = ballStopTime;
			isRunningTimer = true;
			spline.Pause();
			circleCollider.enabled = true;

			if (shouldBallDamageFor1Frame)
			{
				timer.StartTimer(0.02f);
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
			hitStop.StartHitStop(hitStopTime);
		}
	}

	private void DisableBallDamage()
	{
		canDamage = false;
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
