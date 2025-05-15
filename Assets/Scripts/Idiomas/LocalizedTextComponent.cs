using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LocalizedTextComponent : MonoBehaviour
{
    public string key;
    public TextMeshProUGUI tmpText;
    public Text legacyText;

    void OnEnable()
    {
        var manager = FindObjectOfType<SimpleLocalization>();
        if (manager != null)
        {
            manager.UpdateSingleText(this);
        }
    }
} 