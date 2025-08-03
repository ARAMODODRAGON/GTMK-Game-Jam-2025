using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	[SerializeField]
	int player1LivesTotal;
	[SerializeField]
	int player2LivesTotal;
	[SerializeField]
	float roundTime;
	[SerializeField]
	float hitStopTime;

	int player1Lives;
	int player2Lives;

	[SerializeField]
	GameObject player1Prefab;
	[SerializeField]
	GameObject player2Prefab;

	[SerializeField]
	GameObject leftSpikes;
	Vector2 leftSpikesPos;
	[SerializeField]
	GameObject rightSpikes;
	Vector2 rightSpikesPos;

	PlayerController player1Ref;
	PlayerController player2Ref;

	[SerializeField]
	Transform spawnLoc1;

	[SerializeField]
	Transform spawnLoc2;

	[SerializeField]
	Timer respawnTimer;
	[SerializeField]
	Timer roundTimer;

	HitStop hitStop;

	bool canDamagePlayers = true;

	[SerializeField]
	GameObject mainCamera;
	ScreenShake screenShake;

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
		EventBus.gameEnd.AddListener(GameEnd);

		hitStop = GetComponent<HitStop>();
		screenShake = mainCamera.GetComponent<ScreenShake>();

		respawnTimer.onTimerFinished.AddListener(ResetPlayerPositions);
		roundTimer.onTimerFinished.AddListener(MoveSpikes);

		leftSpikesPos = leftSpikes.transform.position;
		rightSpikesPos = rightSpikes.transform.position;

		SetupGame();
	}

	public void TakeDamage(PlayerController player_)
	{
		
		if (player_ == null)
		{
			Debug.Log("Player ref was null, couldn't assign damage");
			return;
		}

		if (canDamagePlayers == false)
		{
			return;
		}

		if (player_.UID == 1)
		{
			player1LivesTotal--;
			EventBus.updatePlayerHealth.Invoke(player_.UID, player1LivesTotal);

			if (player1LivesTotal <= 0)
			{
				EventBus.announceWinner.Invoke(2);
				roundTimer.StopTimer();
			}
		}
		else
		{
			player2LivesTotal--;
			EventBus.updatePlayerHealth.Invoke(player_.UID, player2LivesTotal);

			if (player2LivesTotal <= 0)
			{
				EventBus.announceWinner.Invoke(1);
				roundTimer.StopTimer();
			}
		}

		canDamagePlayers = false;

		player_.gameObject.SetActive(false);
		StartHitStop();
		if (player1LivesTotal <= 0 || player2LivesTotal <= 0) return;
		respawnTimer.StartTimer(2); // only respawn if the game is not over
	}

	private void GameEnd() 
	{
		SceneManager.LoadScene(0); // loads the main menu
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

		EventBus.updatePlayerHealth.Invoke(player1Ref.UID, player1Lives);

		spawnedObject = Instantiate(player2Prefab.gameObject, spawnLoc2.position, spawnLoc2.rotation);
		if (spawnedObject == null)
		{
			Debug.LogError("Player 2 Couldn't be spawned");
			return;
		}

		player2Ref = spawnedObject.GetComponent<PlayerController>();
		player2Ref.UID = 2;

		EventBus.updatePlayerHealth.Invoke(player2Ref.UID, player2Lives);

		roundTimer.StartTimer(roundTime);
		roundTimer.onTimerUpdated.AddListener(EventBus.updateGameTimer.Invoke);
	}

	void ResetPlayerPositions()
	{
		leftSpikes.transform.position = leftSpikesPos;
		leftSpikes.GetComponent<Mover>().SetDirection(new Vector2(0, 0));
		rightSpikes.transform.position = rightSpikesPos;
		rightSpikes.GetComponent<Mover>().SetDirection(new Vector2(0, 0));

		roundTimer.StartTimer(roundTime);


		player1Ref.transform.position = new Vector2 (spawnLoc1.position.x, spawnLoc1.position.y);
		player2Ref.transform.position = new Vector2 (spawnLoc2.position.x, spawnLoc2.position.y);

		player1Ref.gameObject.SetActive(true);
		player2Ref.gameObject.SetActive(true);

		canDamagePlayers = true;
	}

	void MoveSpikes()
	{
		leftSpikes.GetComponent<Mover>().SetDirection(new Vector2(1, 0));
		rightSpikes.GetComponent<Mover>().SetDirection(new Vector2(-1, 0));
	}

	public void StartHitStop()
	{
		hitStop.StartHitStop(hitStopTime);
	}

	public void ScreenShake()
	{
		screenShake.StartScreenShake(mainCamera);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
