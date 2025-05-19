using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CambiarTextoYFlipImagen : MonoBehaviour
{
    [SerializeField] private Button miBoton;
    [SerializeField] private GameObject textoTMPActivo;    // Activo al iniciar
    [SerializeField] private GameObject textoTMPAActivar;  // Se activa al pulsar el botón
    [SerializeField] private Image imagenObjetivo;

    private bool mostrandoPrimero = true;

    private void Start()
    {
        if (miBoton != null && textoTMPActivo != null && textoTMPAActivar != null && imagenObjetivo != null)
        {
            miBoton.onClick.AddListener(ToggleTextosYFlipImagen);
            textoTMPActivo.SetActive(true);      // Activo al iniciar
            textoTMPAActivar.SetActive(false);   // Desactivado al iniciar
            mostrandoPrimero = true;
        }
    }

    private void ToggleTextosYFlipImagen()
    {
        mostrandoPrimero = !mostrandoPrimero;
        textoTMPActivo.SetActive(mostrandoPrimero);
        textoTMPAActivar.SetActive(!mostrandoPrimero);

        // Voltear imagen horizontalmente
        Vector3 escalaActual = imagenObjetivo.rectTransform.localScale;
        escalaActual.x *= -1;
        imagenObjetivo.rectTransform.localScale = escalaActual;
    }
}