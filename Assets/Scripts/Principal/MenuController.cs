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

    private bool menuVisible = false;
    private Button primerBoton;
    private Button segundoBoton;

    private void Start()
    {
        panelOscuro.SetActive(false);
        botonesExtra.SetActive(false);
        panelCanvasGroup.alpha = 0f;

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
        if (menuVisible && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartCoroutine(FadeOutAndDisable());
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

        // ✅ Seleccionar el primer botón del menú principal si está asignado
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
