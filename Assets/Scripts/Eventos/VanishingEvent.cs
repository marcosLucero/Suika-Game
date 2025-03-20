using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VanishingEvent : MonoBehaviour
{
    private bool isActive = false;
    private bool isInvisiblePhase = false;
    private float fadeDuration = 5f;
    private float invisibleDuration = 5f;
    private float currentAlpha = 1f; // Controla la transparencia actual
    private List<GameObject> activeMinerals = new List<GameObject>();

    public bool IsEventActive => isActive;
    public float CurrentAlpha => currentAlpha;

    public void TriggerVanishing()
    {
        if (isActive) return;
        isActive = true;
        isInvisiblePhase = false; // Se asegura de que solo sea true en la fase 2
        RegisterAllExistingMinerals();
        StartCoroutine(VanishMinerals());
    }

    private void RegisterAllExistingMinerals()
    {
        activeMinerals.Clear();
        GameObject[] minerals = GameObject.FindGameObjectsWithTag("Mineral");

        foreach (GameObject mineral in minerals)
        {
            RegisterNewMineral(mineral);
        }
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
        isInvisiblePhase = true; // Activamos el estado de invisibilidad

        // 🔵 Fase 2: Mantener invisibles 5s
        yield return new WaitForSeconds(invisibleDuration);

        elapsedTime = 0f;
        isInvisiblePhase = false; // Finaliza la fase de invisibilidad

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
        for (int i = activeMinerals.Count - 1; i >= 0; i--)
        {
            if (activeMinerals[i] == null)
            {
                activeMinerals.RemoveAt(i);
            }
            else
            {
                ApplyTransparency(activeMinerals[i], alpha);
            }
        }
    }

    private void ApplyTransparency(GameObject mineral, float alpha)
    {
        if (mineral == null) return;

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
        if (mineral != null && !activeMinerals.Contains(mineral))
        {
            activeMinerals.Add(mineral);

            // Si estamos en la fase de invisibilidad, el mineral debe ser transparente de inmediato
            if (isInvisiblePhase)
            {
                ApplyTransparency(mineral, 0f);
            }
            else
            {
                ApplyTransparency(mineral, currentAlpha);
            }
        }
    }
}
