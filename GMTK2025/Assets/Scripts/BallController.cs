using UnityEngine;
using UnityEngine.Splines;

public class BallController : MonoBehaviour
{
	[SerializeField]
	float BallStopTime;
	float currentTimer;
	bool isRunningTimer = false;

	SplineAnimate spline;
	CircleCollider2D circleCollider;

	SpriteRenderer spriteRenderer;

	private void Start()
	{
		spline = GetComponent<SplineAnimate>();
		circleCollider = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		circleCollider.enabled = false;
		isRunningTimer = false;

		spriteRenderer.color = Color.blue;
	}

	public void StopBall()
	{
		if (!isRunningTimer)
		{
			spriteRenderer.color = Color.red;
			currentTimer = BallStopTime;
			isRunningTimer = true;
			spline.Pause();
			circleCollider.enabled = true;
		}

	}

	void StartBall()
	{
		spriteRenderer.color = Color.blue;
		isRunningTimer = false;
		circleCollider.enabled = false;
		spline.Play();
	}

	void Update()
	{
		if (currentTimer > 0)
		{
			currentTimer -= Time.deltaTime;
			if(currentTimer <= 0)
			{
				StartBall();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Damageable"))
		{
			Debug.Log("Damage Dealt");
		}
	}
}
