using UnityEngine;
using System.Collections;

public class VanishingEvent : MonoBehaviour
{
    private bool isActive = false;
    private float fadeDuration = 5f; // Tiempo para volverse transparente
    private float invisibleDuration = 5f; // Tiempo invisible
    private GameObject[] minerals;

    public void TriggerVanishing()
    {
        if (isActive) return; // Evitar que se active dos veces
        isActive = true;

        minerals = GameObject.FindGameObjectsWithTag("Mineral");
        if (minerals.Length == 0)
        {
            isActive = false;
            return;
        }

        StartCoroutine(VanishMinerals());
    }

    IEnumerator VanishMinerals()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetMineralAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetMineralAlpha(0f); // Asegurar invisibilidad total
        yield return new WaitForSeconds(invisibleDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetMineralAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetMineralAlpha(1f); // Restaurar visibilidad
        isActive = false;
    }

    private void SetMineralAlpha(float alpha)
    {
        foreach (GameObject mineral in minerals)
        {
            if (mineral != null)
            {
                SpriteRenderer renderer = mineral.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    Color color = renderer.color;
                    color.a = alpha;
                    renderer.color = color;
                }
            }
        }
    }
}
