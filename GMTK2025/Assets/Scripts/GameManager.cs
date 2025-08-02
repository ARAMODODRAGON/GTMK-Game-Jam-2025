using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	[SerializeField]
	int player1LivesTotal;
	[SerializeField]
	int player2LivesTotal;
	[SerializeField]
	float gameTimer;
	[SerializeField]
	float hitStopTime;

	int player1Lives;
	int player2Lives;

	[SerializeField]
	GameObject player1Prefab;
	[SerializeField]
	GameObject player2Prefab;

	PlayerController player1Ref;
	PlayerController player2Ref;

	[SerializeField]
	Transform spawnLoc1;

	[SerializeField]
	Transform spawnLoc2;

	Timer timer;

	HitStop hitStop;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		timer = GetComponent<Timer>();
		hitStop = GetComponent<HitStop>();

		timer.onTimerFinished.AddListener(ResetPlayerPositions);

		SetupGame();
	}

	public void TakeDamage(PlayerController player_)
	{
		if (player_ == null)
		{
			Debug.Log("Player ref was null, couldn't assign damage");
			return;

		}

		if (player_.UID == 1)
		{
			player1LivesTotal--;
			Debug.Log("Player 1 health dropped");

			if (player2LivesTotal <= 0)
			{
				Debug.Log("Game End");
			}
		}
		else
		{
			player2LivesTotal--;
			Debug.Log("Player 2 health dropped");

			if (player2LivesTotal <= 0)
			{
				Debug.Log("Game End");
			}
		}



		player_.gameObject.SetActive(false);
		StartHitStop();
		timer.StartTimer(2);
	}

	void SetupGame()
	{

		player1Lives = player1LivesTotal;
		player2Lives = player2LivesTotal;

		GameObject spawnedObject;

		spawnedObject = Instantiate(player1Prefab.gameObject, spawnLoc1.position, spawnLoc1.rotation);
		if (spawnedObject == null)
		{
			Debug.LogError("Player 1 Couldn't be spawned");
			return;
		}

		player1Ref = spawnedObject.GetComponent<PlayerController>();
		player1Ref.UID = 1;

		spawnedObject = Instantiate(player2Prefab.gameObject, spawnLoc2.position, spawnLoc2.rotation);
		if (spawnedObject == null)
		{
			Debug.LogError("Player 2 Couldn't be spawned");
			return;
		}

		player2Ref = spawnedObject.GetComponent<PlayerController>();
		player2Ref.UID = 2;
	}

	void ResetPlayerPositions()
	{
		player1Ref.transform.position = new Vector2 (spawnLoc1.position.x, spawnLoc1.position.y);
		player2Ref.transform.position = new Vector2(spawnLoc2.position.x, spawnLoc2.position.y);

		player1Ref.gameObject.SetActive(true);
		player2Ref.gameObject.SetActive(true);
	}

	public void StartHitStop()
	{
		hitStop.StartHitStop(hitStopTime);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
