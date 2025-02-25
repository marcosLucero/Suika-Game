using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralCombiner : MonoBehaviour
{
    private int _layerIndex;
    private MineralInfo _info;
    private bool _hasFused = false;

    private void Awake()
    {
        _info = GetComponent<MineralInfo>();
        _layerIndex = gameObject.layer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasFused)
            return; // 🔹 Si ya se fusionó, salimos

        if (collision.gameObject.layer == _layerIndex)
        {
            MineralInfo otherInfo = collision.gameObject.GetComponent<MineralInfo>();
            MineralCombiner otherCombiner = collision.gameObject.GetComponent<MineralCombiner>();

            if (otherCombiner != null && otherCombiner._hasFused)
                return;

            if (otherInfo != null && otherInfo.MineralIndex == _info.MineralIndex)
            {
                int thisID = gameObject.GetInstanceID();
                int otherID = collision.gameObject.GetInstanceID();

                if (thisID > otherID)
                {
                    StartCoroutine(FusionProcess(collision, otherCombiner));
                }
            }
        }
    }

    private IEnumerator FusionProcess(Collision2D collision, MineralCombiner otherCombiner)
    {
        GameManager.Instance.IncreaseScore(_info.PuntosCuandoAniquilados);

        if (_info.MineralIndex == MineralesSelector.Instance.Minerales.Length - 1)
        {
            yield break;
        }

        _hasFused = true;
        if (otherCombiner != null)
        {
            otherCombiner._hasFused = true;
        }

        // 🔹 🔥 **Desactivamos las colisiones de ambos minerales por 0.3s**
        Collider2D colThis = GetComponent<Collider2D>();
        Collider2D colOther = collision.gameObject.GetComponent<Collider2D>();

        if (colThis) colThis.enabled = false;
        if (colOther) colOther.enabled = false;

        // 🔹 Calculamos la posición media de los dos minerales
        Vector3 middlePosition = (transform.position + collision.transform.position) / 2f;
        middlePosition += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
        middlePosition = AdjustFusionPosition(middlePosition);

        GameObject fusedMineral = Instantiate(SpamCombinedMineral(_info.MineralIndex), GameManager.Instance.transform);
        fusedMineral.transform.position = middlePosition;

        Fusion fusionScript = fusedMineral.GetComponent<Fusion>();
        if (fusionScript != null)
        {
            fusionScript.PlayFusionEffect();
        }

        // 🔹 Reproducir el sonido de fusión
        AudioSource audioSource = fusedMineral.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        ColiderInformer informer = fusedMineral.GetComponent<ColiderInformer>();
        if (informer != null)
        {
            informer.WasCombinedIn = true;
        }

        Destroy(collision.gameObject);
        Destroy(gameObject);

        // 🔹 ⏳ **Esperamos 0.3s antes de reactivar colisiones**
        yield return new WaitForSeconds(0.3f);

        if (colThis) colThis.enabled = true;
        if (colOther) colOther.enabled = true;
    }


    private GameObject SpamCombinedMineral(int index)
    {
        return MineralesSelector.Instance.Minerales[index + 1];
    }

    private Vector3 AdjustFusionPosition(Vector3 fusionPos)
    {
        int wallLayer = LayerMask.NameToLayer("Wall");
        Collider2D wallCollider = Physics2D.OverlapPoint(fusionPos, 1 << wallLayer);
        Vector3 adjustedPos = fusionPos;
        if (wallCollider != null)
        {
            Vector2 closestPoint = wallCollider.ClosestPoint(fusionPos);
            Vector2 pushDirection = ((Vector2)fusionPos - closestPoint).normalized;
            if (pushDirection == Vector2.zero)
            {
                pushDirection = Vector2.up;
            }
            float pushDistance = 1.5f;
            adjustedPos = fusionPos + (Vector3)(pushDirection * pushDistance);
        }
        return adjustedPos;
    }
}
