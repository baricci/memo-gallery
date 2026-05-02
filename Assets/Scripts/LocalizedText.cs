using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;

    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        if (textComponent == null) return;
    }

    private void Start()
    {
        LocalizationController.Instance.Register(this);
        UpdateText();
    }

    public void UpdateText()
    {
        textComponent.text = LocalizationController.Instance.Get(key);
    }

}
