using UnityEngine;
using UnityEngine.UI;

public class AsignarIdiomaBotones : MonoBehaviour
{
    public Button botonEspanol;
    public Button botonIngles;

    void Start()
    {
        if (botonEspanol != null)
            botonEspanol.onClick.AddListener(() => SimpleLocalization.Instance.SetLanguage(0));
        if (botonIngles != null)
            botonIngles.onClick.AddListener(() => SimpleLocalization.Instance.SetLanguage(1));
    }
}