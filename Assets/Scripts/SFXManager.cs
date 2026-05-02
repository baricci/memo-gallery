using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("SFX Clips")]
    public AudioClip buttonClick;
    public AudioClip cardFlip;
    public AudioClip match;
    public AudioClip mismatch;
    public AudioClip victory;
    public AudioClip confirm;

    private const string SFX_VOLUME_KEY = "sfxVolume";

    private float sfxVolume = 1f;

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
    }

    private void LoadSettings()
    {
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        sfxSource.volume = sfxVolume;
    }

    public void SetVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = value;

        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
    }

    public void ApplySettings()
    {
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        sfxSource.volume = sfxVolume;
    }

    private void Play(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayButton() => Play(buttonClick);
    public void PlayFlip() => Play(cardFlip);
    public void PlayMatch() => Play(match);
    public void PlayMismatch() => Play(mismatch);
    public void PlayVictory() => Play(victory);
    public void PlayConfirm() => Play(confirm);
}
