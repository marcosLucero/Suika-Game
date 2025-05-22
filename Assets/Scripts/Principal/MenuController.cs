using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelOscuro;
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private GameObject botonesExtra;
    [SerializeField] private Button botonPrincipal;
    [SerializeField] private Button primerBotonMenuPrincipal; // ✅ Nuevo: primer botón del menú principal
    [SerializeField] private Button botonIdiomaEspanol;
    [SerializeField] private Button botonIdiomaIngles;
    [SerializeField] private SpriteRenderer imagenExtra1; // Cambiado a SpriteRenderer
    [SerializeField] private SpriteRenderer imagenExtra2; // Cambiado a SpriteRenderer

    private bool menuVisible = false;
    private Button primerBoton;
    private Button segundoBoton;
    private Color colorOriginal1;
    private Color colorOriginal2;

    private void Start()
    {
        panelOscuro.SetActive(false);
        botonesExtra.SetActive(false);
        panelCanvasGroup.alpha = 0f;

        // Guardar los colores originales
        if (imagenExtra1 != null) colorOriginal1 = imagenExtra1.color;
        if (imagenExtra2 != null) colorOriginal2 = imagenExtra2.color;

        botonPrincipal.onClick.AddListener(ToggleMenu);

        EventTrigger trigger = panelOscuro.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((data) => { CerrarMenu(); });
        trigger.triggers.Add(entry);

        Button[] botones = botonesExtra.GetComponentsInChildren<Button>(true);
        if (botones.Length >= 2)
        {
            primerBoton = botones[0];
            segundoBoton = botones[1];
        }
    }

    private void Update()
    {
        if (menuVisible)
        {
            // Teclado
            if (Keyboard.current.escapeKey.wasPressedThisFrame ||
                // Mando: botón B (botón Este en el gamepad)
                (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                StartCoroutine(FadeOutAndDisable());
            }
        }
    }


    private void ToggleMenu()
    {
        menuVisible = !menuVisible;

        if (menuVisible)
        {
            panelOscuro.SetActive(true);
            botonesExtra.SetActive(true);
            StartCoroutine(FadeCanvasGroup(panelCanvasGroup, 0f, 1f, 0.3f));
            StartCoroutine(SeleccionarPrimerBotonConDelay());
            ConfigurarNavegacion();

            // Desactivar botones de idioma y hacer sprites semi-transparentes
            if (botonIdiomaEspanol != null) botonIdiomaEspanol.interactable = false;
            if (botonIdiomaIngles != null) botonIdiomaIngles.interactable = false;
            if (imagenExtra1 != null)
            {
                Color colorTransparente = colorOriginal1;
                colorTransparente.a = 0.5f;
                imagenExtra1.color = colorTransparente;
            }
            if (imagenExtra2 != null)
            {
                Color colorTransparente = colorOriginal2;
                colorTransparente.a = 0.5f;
                imagenExtra2.color = colorTransparente;
            }
        }
        else
        {
            StartCoroutine(FadeOutAndDisable());
        }
    }

    private void CerrarMenu()
    {
        if (menuVisible)
        {
            StartCoroutine(FadeOutAndDisable());
        }
    }

    private IEnumerator FadeOutAndDisable()
    {
        yield return FadeCanvasGroup(panelCanvasGroup, 1f, 0f, 0.3f);
        panelOscuro.SetActive(false);
        botonesExtra.SetActive(false);
        menuVisible = false;
        yield return null;

        // Activar botones de idioma y restaurar colores originales de sprites
        if (botonIdiomaEspanol != null) botonIdiomaEspanol.interactable = true;
        if (botonIdiomaIngles != null) botonIdiomaIngles.interactable = true;
        if (imagenExtra1 != null) imagenExtra1.color = colorOriginal1;
        if (imagenExtra2 != null) imagenExtra2.color = colorOriginal2;

        if (primerBotonMenuPrincipal != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(primerBotonMenuPrincipal.gameObject);
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    private IEnumerator SeleccionarPrimerBotonConDelay()
    {
        yield return null;
        if (primerBoton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(primerBoton.gameObject);
        }
    }

    private void ConfigurarNavegacion()
    {
        if (primerBoton != null && segundoBoton != null)
        {
            var nav1 = new Navigation { mode = Navigation.Mode.Explicit };
            nav1.selectOnDown = segundoBoton;
            nav1.selectOnUp = segundoBoton;
            primerBoton.navigation = nav1;

            var nav2 = new Navigation { mode = Navigation.Mode.Explicit };
            nav2.selectOnDown = primerBoton;
            nav2.selectOnUp = primerBoton;
            segundoBoton.navigation = nav2;
        }
    }
}
