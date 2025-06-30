using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("Audio")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip knifeChopSFX;
    [SerializeField] private AudioClip knifeBlockChopSFX;

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool playBGMOnStart;
    [SerializeField] private float bgmVolume = 1f;

    private bool isMuted;
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
        
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (playBGMOnStart)
        {
            PlayBackgroundMusic();
        }
        
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        sfxAudioSource.mute = isMuted;
        bgmAudioSource.mute = isMuted;
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        bgmAudioSource.mute = isMuted;
        sfxAudioSource.mute = isMuted;

        PlayerPrefs.SetInt("Mute", isMuted ? 1 : 0);
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void PlayKnifeChopSFX()
    {
        if (knifeChopSFX != null)
            sfxAudioSource.PlayOneShot(knifeChopSFX);
    }

    public void PlayBlockChopSFX()
    {
        if (knifeBlockChopSFX != null)
            sfxAudioSource.PlayOneShot(knifeBlockChopSFX);
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && bgmAudioSource != null)
        {
            bgmAudioSource.clip = backgroundMusic;
            bgmAudioSource.loop = true;
            bgmAudioSource.volume = bgmVolume;
            bgmAudioSource.Play();
        }
    }
}
