using UnityEngine;
using System.Collections;

public class BouncyEffect : MonoBehaviour
{
    public PhysicsMaterial2D normalMaterial;  // Material normal
    public PhysicsMaterial2D bouncyMaterial;  // Material de rebote
    private bool isBouncyActive = false;

    public bool IsBouncyActive => isBouncyActive;

    public void ActivateBouncyEffect(float duration)
    {
        if (!isBouncyActive)
        {
            StartCoroutine(BouncyEffectCoroutine(duration));
        }
    }

    private IEnumerator BouncyEffectCoroutine(float duration)
    {
        isBouncyActive = true;
        Debug.Log("🟢 Evento: Rebote activado por " + duration + " segundos");

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            ApplyBouncyMaterialToAllMinerals(); // Aplicar a todos los minerales activos
            yield return new WaitForSeconds(0.5f); // Espera medio segundo antes de aplicar de nuevo
            elapsedTime += 0.5f;
        }

        isBouncyActive = false;
        Debug.Log("🔴 Evento: Rebote desactivado");
        RestoreNormalMaterialToAllMinerals();
    }

    private void ApplyBouncyMaterialToAllMinerals()
    {
        foreach (var mineral in GameObject.FindGameObjectsWithTag("Mineral"))
        {
            Collider2D col = mineral.GetComponent<Collider2D>();
            if (col != null)
            {
                col.sharedMaterial = bouncyMaterial;
            }
        }
    }

    private void RestoreNormalMaterialToAllMinerals()
    {
        foreach (var mineral in GameObject.FindGameObjectsWithTag("Mineral"))
        {
            Collider2D col = mineral.GetComponent<Collider2D>();
            if (col != null)
            {
                col.sharedMaterial = normalMaterial;
            }
        }
    }
}
