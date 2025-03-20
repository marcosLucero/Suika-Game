using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VanishingEvent : MonoBehaviour
{
    private bool isActive = false;
    private float fadeDuration = 5f;
    private float invisibleDuration = 5f;
    private float currentAlpha = 1f; // Controla la transparencia actual
    private List<GameObject> activeMinerals = new List<GameObject>();

    public void TriggerVanishing()
    {
        if (isActive) return;
        isActive = true;
        StartCoroutine(VanishMinerals());
    }

    IEnumerator VanishMinerals()
    {
        float elapsedTime = 0f;

        // 🔴 Fase 1: Hacerse transparente en 5s
        while (elapsedTime < fadeDuration)
        {
            currentAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            ApplyTransparencyToAll(currentAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentAlpha = 0f;
        ApplyTransparencyToAll(currentAlpha);

        // 🔵 Fase 2: Mantener invisibles 5s
        yield return new WaitForSeconds(invisibleDuration);

        elapsedTime = 0f;

        // 🟢 Fase 3: Volver a ser visibles en 5s
        while (elapsedTime < fadeDuration)
        {
            currentAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            ApplyTransparencyToAll(currentAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentAlpha = 1f;
        ApplyTransparencyToAll(currentAlpha);
        isActive = false;
    }

    private void ApplyTransparencyToAll(float alpha)
    {
        activeMinerals.Clear();
        activeMinerals.AddRange(GameObject.FindGameObjectsWithTag("Mineral"));

        foreach (GameObject mineral in activeMinerals)
        {
            if (mineral != null)
            {
                ApplyTransparency(mineral, alpha);
            }
        }
    }

    private void ApplyTransparency(GameObject mineral, float alpha)
    {
        SpriteRenderer renderer = mineral.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }

    public void RegisterNewMineral(GameObject mineral)
    {
        if (mineral != null)
        {
            if (isActive)
            {
                // Si estamos en la fase de invisibilidad, hacer el mineral completamente invisible
                ApplyTransparency(mineral, currentAlpha);
            }
        }
    }

}
