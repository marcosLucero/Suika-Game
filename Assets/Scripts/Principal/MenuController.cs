using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelOscuro;  // Panel de fondo oscuro
    [SerializeField] private CanvasGroup panelCanvasGroup; // Para el fade
    [SerializeField] private GameObject botonesExtra; // Contenedor de botones
    [SerializeField] private Button botonPrincipal;   // Botón que activa el menú

    private bool menuVisible = false;

    private void Start()
    {
        panelOscuro.SetActive(false);
        botonesExtra.SetActive(false);
        panelCanvasGroup.alpha = 0f;

        botonPrincipal.onClick.AddListener(ToggleMenu);

        // Agregar EventTrigger para detectar clics fuera de los botones
        EventTrigger trigger = panelOscuro.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((data) => { CerrarMenu(); });
        trigger.triggers.Add(entry);
    }

    private void ToggleMenu()
    {
        menuVisible = !menuVisible;

        if (menuVisible)
        {
            panelOscuro.SetActive(true);
            botonesExtra.SetActive(true);
            StartCoroutine(FadeCanvasGroup(panelCanvasGroup, 0f, 1f, 0.3f)); // Fade-in
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
}
