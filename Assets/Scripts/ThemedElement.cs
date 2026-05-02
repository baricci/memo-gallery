using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemedElement : MonoBehaviour
{
    public ThemeType type;

    private Image image;
    private TextMeshProUGUI text;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        ThemeController.Instance.Register(this);
    }

    private void OnEnable()
    {
        if (ThemeController.Instance != null)
            ThemeController.Instance.Register(this);
    }

    public void Apply(Color bg, Color panel, Color box, Color title, Color subtitle, Color txt, Color info, Color special, Color symbol, Color accent, Color hover, Color pressed)
    {
        switch (type)
        {
            case ThemeType.Background:
                if (image != null) image.color = bg; break;
            case ThemeType.Panel:
                if (image != null) image.color = panel; break;
            case ThemeType.Box:
                if (image != null) image.color = box;
                if (button != null)
                {
                    ColorBlock colors = button.colors;
                    colors.highlightedColor = hover;
                    colors.pressedColor = pressed;

                    button.colors = colors;
                }
                break;
            case ThemeType.Title:
                if (text != null) text.color = title; break;
            case ThemeType.Subtitle:
                if (text != null) text.color = subtitle; break;
            case ThemeType.Text:
                if (text != null) text.color = txt; break;
            case ThemeType.InfoText:
                if (text != null) text.color = info; break;
            case ThemeType.SpecialText:
                if (text != null) text.color = special; break;
            case ThemeType.Symbol:
                if (image != null) image.color = symbol;
                if (text != null) text.color = symbol; break;
            case ThemeType.Accent:
                if (image != null) image.color = accent;
                if (text != null) text.color = accent; break;
        }
    }
}

public enum ThemeType
{
    Background, Panel, Box,
    Title, Subtitle, Text, InfoText, SpecialText,
    Symbol, Accent
}