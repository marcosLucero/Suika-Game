using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CambiarTextoYFlipImagen : MonoBehaviour
{
    [SerializeField] private Button miBoton;
    [SerializeField] private TextMeshProUGUI textoObjetivo;
    [SerializeField] private Image imagenObjetivo;
    [SerializeField] private string texto1 = "Modo sin eventos";
    [SerializeField] private string texto2 = "Modo con eventos";

    private bool usandoTexto1 = true;

    private void Start()
    {
        if (miBoton != null && textoObjetivo != null && imagenObjetivo != null)
        {
            miBoton.onClick.AddListener(ToggleTextoYImagen);
            textoObjetivo.text = texto1; // Inicializa con texto1
        }
    }

    private void ToggleTextoYImagen()
    {
        // Cambiar texto
        textoObjetivo.text = usandoTexto1 ? texto2 : texto1;

        // Voltear imagen horizontalmente
        Vector3 escalaActual = imagenObjetivo.rectTransform.localScale;
        escalaActual.x *= -1;
        imagenObjetivo.rectTransform.localScale = escalaActual;

        usandoTexto1 = !usandoTexto1;
    }
}
