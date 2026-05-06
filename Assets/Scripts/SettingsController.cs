using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static SettingsController Instance;

    [Header("Audio")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle musicTrackToggle;

    [Header("Gameplay")]
    public Toggle vibrationToggle;

    [Header("Appearance")]
    public Toggle themeToggle;

    [Header("System")]
    public TextMeshProUGUI versionText;
    public TMP_Dropdown languageDropdown;

    public GameObject settingsPanel;
    private bool isOpen = false;
    private bool hasVibration = true;

    private const string VIBRATION_KEY = "vibration";
    private const string THEME_KEY = "theme";

    private readonly string VERSION = "1.1.0";

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
        StartSystemSection();
        ApplySettings();
    }

    public void ToggleSettings()
    {
        isOpen = !isOpen;
        settingsPanel.GetComponent<Animator>().SetBool("IsOpen", isOpen);

        if (isOpen)
            UIController.Instance.SetPrevious();
    }

    public void ToggleVibration()
    {
        hasVibration = !hasVibration;
        PlayerPrefs.SetInt(VIBRATION_KEY, hasVibration ? 1 : 0);
    }

    public void SetVibration(bool value)
    {
        PlayerPrefs.SetInt(VIBRATION_KEY, value ? 1 : 0);
    }

    public bool IsVibrationEnabled()
    {
        return PlayerPrefs.GetInt(VIBRATION_KEY, 1) == 1;
    }

    public void SetTheme(int value)
    {
        ThemeController.Instance.SetTheme(value);
    }

    public int GetTheme()
    {
        return PlayerPrefs.GetInt(THEME_KEY, 0);
    }

    private void StartSystemSection()
    {
        versionText.text = VERSION;

        languageDropdown.ClearOptions();
        List<string> options = new() { "English", "Italiano", "Español", "Français", "Português" };
        languageDropdown.AddOptions(options);

        int savedLanguage = PlayerPrefs.GetInt("language", 0);
        languageDropdown.value = savedLanguage;
        languageDropdown.RefreshShownValue();

        languageDropdown.onValueChanged.AddListener(SetLanguage);
    }

    public void SetLanguage(int index)
    {
        PlayerPrefs.SetInt("language", index);
        PlayerPrefs.Save();

        LocalizationController.Instance.SetLanguage(index);
    }

    public void RestoreDefaultsSettings()
    {
        AudioManager.Instance.SetVolume(1f);
        SFXManager.Instance.SetVolume(1f);
        AudioManager.Instance.SetTrack(0);
        AudioManager.Instance.SetShuffle(false);

        SetVibration(true);
        SetTheme(0);
        SetLanguage(0);

        PlayerPrefs.Save();
        ApplySettings();
        RestoreDefautsUI();
    }

    private void RestoreDefautsUI()
    {
        musicSlider.value = 1f;
        sfxSlider.value = 1f;
        musicTrackToggle.isOn = true;

        vibrationToggle.isOn = true;

        themeToggle.isOn = true;

        languageDropdown.value = 0;
    }

    public void ApplySettings()
    {
        AudioManager.Instance.ApplySettings();
        SFXManager.Instance.ApplySettings();

        hasVibration = IsVibrationEnabled();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
