using UnityEngine;

public class FadeOutAudio : MonoBehaviour
{
    public bool canFadeOut;
    AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}


	// Update is called once per frame
	void Update()
    {
        if (canFadeOut)
        {
			audioSource.volume = Util.MoveToward(audioSource.volume, 0f, -0.01f);
        }
    }
}
