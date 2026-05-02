using System.Collections.Generic;
using UnityEngine;

public class ThemeController : MonoBehaviour
{
    public static ThemeController Instance;

    private Color background;
    private Color panel;
    private Color box;
    private Color title;
    private Color subtitle;
    private Color text;
    private Color infoText;
    private Color specialText;
    private Color symbol;
    private Color accent;
    private Color hover;
    private Color pressed;

    private int currentTheme = 0;

    private List<ThemedElement> elements = new();

    private const string THEME_KEY = "theme";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentTheme = PlayerPrefs.GetInt(THEME_KEY, 0);
        ApplyTheme();
    }

    public void Register(ThemedElement element)
    {
        if (!elements.Contains(element))
            elements.Add(element);

        element.Apply(background, panel, box, title, subtitle, text, infoText, specialText, symbol, accent, hover, pressed);
    }

    private void SetDark()
    {
        background = new Color32(15, 23, 42, 255);
        panel = new Color32(30, 41, 59, 255);
        box = new Color32(51, 65, 85, 255);
        title = new Color32(226, 232, 240, 255);
        subtitle = new Color32(148, 163, 184, 255);
        text = new Color32(203, 213, 225, 255);
        infoText = new Color32(125, 211, 252, 255);
        specialText = new Color32(74, 222, 128, 255);
        symbol = new Color32(148, 163, 184, 255);
        accent = new Color32(99, 85, 247, 255);
        hover = new Color32(65, 82, 108, 255);
        pressed = new Color32(79, 100, 131, 255);
    }

    private void SetLight()
    {
        background = new Color32(255, 248, 231, 255);
        panel = new Color32(250, 245, 235, 255);
        box = new Color32(241, 237, 225, 255);
        title = new Color32(28, 25, 23, 255);
        subtitle = new Color32(87, 83, 78, 255);
        text = new Color32(60, 56, 52, 255);
        infoText = new Color32(45, 115, 231, 255);
        specialText = new Color32(202, 138, 4, 255);
        symbol = new Color32(28, 25, 23, 255);
        accent = new Color32(192, 38, 211, 255);
        hover = new Color32(226, 217, 193, 255);
        pressed = new Color32(211, 195, 161, 255);
    }

    public void SetTheme(int index)
    {
        currentTheme = index;
        PlayerPrefs.SetInt(THEME_KEY, index);

        ApplyTheme();
    }

    private void ApplyTheme()
    {
        switch (currentTheme)
        {
            case 0: SetDark(); break;
            case 1: SetLight(); break;
            default: SetDark(); break;
        }

        foreach (ThemedElement element in elements)
            element.Apply(background, panel, box, title, subtitle, text, infoText, specialText, symbol, accent, hover, pressed);
    }
}