using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    GameObject cameraRef;
    [SerializeField]
    Timer timer;

    [SerializeField]
    float screenShakeDuration;

    Vector3 startPos;

    [SerializeField]
    AnimationCurve animCurve;

	void Start()
    {
        timer.onTimerUpdated.AddListener(ScreenShakeEffect);
        timer.onTimerFinished.AddListener(ResetShakeEffect);
    }

    public void StartScreenShake(GameObject camera_)
    {
		cameraRef = camera_;
		startPos = cameraRef.transform.position;
        timer.StartTimer(screenShakeDuration);
	}

    void ScreenShakeEffect(float currentTime_)
    {
        float strenght = animCurve.Evaluate(currentTime_ / screenShakeDuration);
		cameraRef.transform.position = startPos + Random.insideUnitSphere * strenght;
    }

    void ResetShakeEffect()
    {
		cameraRef.transform.position = startPos;
	}


}
