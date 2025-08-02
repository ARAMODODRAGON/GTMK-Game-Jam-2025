using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
	[Header("Timer Variables")]
	//Inspector variables
	[SerializeField]
	bool isLooping = false;

	[SerializeField]
	float totalTime;

	public float currentTime;

	//Events
	[Header("Events")]
	public UnityEvent onTimerFinished;
	public UnityEvent<float> onTimerUpdated;

	//Private Variables
	bool isRunning = false;


	void Update()
	{
		if (isRunning)
		{
			currentTime -= Time.deltaTime;

			onTimerUpdated.Invoke(currentTime);

			if (currentTime <= 0)
			{
				isRunning = false;
				onTimerFinished.Invoke();
				Debug.Log("Timer ended");
				if (isLooping)
				{
					StartTimer(totalTime);
				}
			}
		}
	}

	public void StartTimer(float time_)
	{
		//Core assumption with this class is that we only fire the events once and then the object needs to subscribe again, unless we've specified looping.
		totalTime = time_;
		currentTime = time_;
		isRunning = true;
	}

	public void StopTimer()
	{
		isRunning = false;
		onTimerFinished.RemoveAllListeners();
		onTimerUpdated.RemoveAllListeners();
	}


}
