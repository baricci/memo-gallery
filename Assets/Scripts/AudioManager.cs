using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip[] musicTracks;

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    private int currentTrack = 0;
    private bool shuffle = false;

    private const string VOLUME_KEY = "musicVolume";
    private const string TRACK_KEY = "musicTrack";
    private const string SHUFFLE_KEY = "musicShuffle";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        LoadSettings();
        PlaySelectedTrack();
    }

    private void Update()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
            ApplySettings();
    }

    private void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        currentTrack = PlayerPrefs.GetInt(TRACK_KEY, 0);
        shuffle = PlayerPrefs.GetInt(SHUFFLE_KEY, 0) == 1;

        musicSource.volume = musicVolume;
    }

    private void PlaySelectedTrack()
    {
        if (shuffle)
            currentTrack = Random.Range(0, musicTracks.Length);

        musicSource.clip = musicTracks[currentTrack];
        musicSource.Play();
    }

    public void SetVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = value;

        PlayerPrefs.SetFloat(VOLUME_KEY, value);
    }

    public void SetTrack(int index)
    {
        currentTrack = index;
        shuffle = false;

        PlayerPrefs.SetInt(TRACK_KEY, index);
        PlayerPrefs.SetInt(SHUFFLE_KEY, 0);

        PlaySelectedTrack();
    }

    public void SetShuffle(bool value)
    {
        shuffle = value;

        PlayerPrefs.SetInt(SHUFFLE_KEY, value ? 1 : 0);

        PlaySelectedTrack();
    }

    public void ApplySettings()
    {
        musicVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        currentTrack = PlayerPrefs.GetInt(TRACK_KEY, 0);
        shuffle = PlayerPrefs.GetInt(SHUFFLE_KEY, 0) == 1;

        musicSource.volume = musicVolume;
        PlaySelectedTrack();
    }
}