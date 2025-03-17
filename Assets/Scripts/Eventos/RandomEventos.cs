using UnityEngine;
using System.Collections;

public class RandomEvents : MonoBehaviour
{
    public PhysicsMaterial2D normalMaterial; // Material normal de los minerales
    public PhysicsMaterial2D bouncyMaterial; // Material de rebote alto
    private bool isBouncyActive = false;

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

        // Buscar todos los minerales y cambiar su material físico
        foreach (var mineral in GameObject.FindGameObjectsWithTag("Mineral"))
        {
            Rigidbody2D rb = mineral.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.sharedMaterial = bouncyMaterial;
            }
        }

        yield return new WaitForSeconds(duration);

        // Restaurar el material original
        foreach (var mineral in GameObject.FindGameObjectsWithTag("Mineral"))
        {
            Rigidbody2D rb = mineral.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.sharedMaterial = normalMaterial;
            }
        }

        isBouncyActive = false;
    }
}
