using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    GameObject soundEffectPrefab;

	public AudioClip attackSound;
	public AudioClip damageSound;
	public AudioClip jumpSound;
	public AudioClip uiConfirm;
	public AudioClip uiScroll;

	private void Awake()
	{
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
	}

    public void PlaySound(AudioClip audio_, Vector3 position_, float volume_, float duration_,  bool persistent_ = false)
    {
        AudioSource audio = Instantiate(soundEffectPrefab, position_, Quaternion.identity).GetComponent<AudioSource>();

        if (audio == null)
        {
            Debug.LogError("Couldn't spawn in audio object");
            return;
        }

        audio.clip = audio_;
        audio.volume = volume_;
        audio.Play();

        if (persistent_ == false)
        {
			Destroy(audio.gameObject, duration_);
		}

    }
}
