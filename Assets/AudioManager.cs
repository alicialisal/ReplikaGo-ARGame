using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public bool IsMuted = false;

    [Header("Background Music")]
    [SerializeField] private AudioSource backgroundMusicSource; // Assign di inspector
    [SerializeField] private AudioClip backgroundMusicClip;     // Assign clip di inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (backgroundMusicSource != null && backgroundMusicClip != null)
            {
                backgroundMusicSource.clip = backgroundMusicClip;
                backgroundMusicSource.loop = false; // biar terus main
                backgroundMusicSource.Play();
                backgroundMusicSource.mute = IsMuted;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ToggleMute()
    {
        IsMuted = !IsMuted;
        SetMute(IsMuted);
        return IsMuted;
    }

    public void SetMute(bool mute)
    {
        IsMuted = mute;

        if (backgroundMusicSource != null)
            backgroundMusicSource.mute = mute;

        OnMuteChanged?.Invoke(mute);
    }

    public delegate void MuteChanged(bool isMuted);
    public event MuteChanged OnMuteChanged;
}
