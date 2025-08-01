using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{

    bool isFrozen;

    float currentFreezeDuration = 0f;

    public void StartHitStop(float duration_)
    {
        currentFreezeDuration = duration_;
        StartCoroutine(HitStopEffect());
	}

    IEnumerator HitStopEffect()
    {
        float originalTime = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(currentFreezeDuration);

		Time.timeScale = originalTime;
        currentFreezeDuration = 0;
	}
}
