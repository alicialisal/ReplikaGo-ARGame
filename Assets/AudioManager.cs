using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Background Music")]
    [SerializeField] AudioSource backgroundMusicSource;

    [Header("Music Clips")]
    [SerializeField] AudioClip startmusic;

    private void Start()
    {
        Debug.Log("Starting background music.");
        backgroundMusicSource.clip = startmusic;
        backgroundMusicSource.Play();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ToggleMute()
    {
        backgroundMusicSource.mute = !backgroundMusicSource.mute;
        return backgroundMusicSource.mute;
    }

    public void SetMute(bool mute)
    {
        backgroundMusicSource.mute = mute;
    }
}
