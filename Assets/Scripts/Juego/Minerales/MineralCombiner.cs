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

    // Se utiliza OnCollisionEnter2D y OnCollisionStay2D para detectar contacto real
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryFusion(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryFusion(collision);
    }

    private void TryFusion(Collision2D collision)
    {
        if (_hasFused)
            return; // Si ya se fusionó, salimos

        if (collision.gameObject.layer != _layerIndex)
            return;

        // Comprobamos el cooldown en este mineral
        FusionCooldown myCooldown = GetComponent<FusionCooldown>();
        if (myCooldown != null && !myCooldown.CanFuse)
            return;

        // Comprobamos el cooldown en el objeto colisionado
        FusionCooldown otherCooldown = collision.gameObject.GetComponent<FusionCooldown>();
        if (otherCooldown != null && !otherCooldown.CanFuse)
            return;

        MineralInfo otherInfo = collision.gameObject.GetComponent<MineralInfo>();
        MineralCombiner otherCombiner = collision.gameObject.GetComponent<MineralCombiner>();

        if (otherCombiner != null && otherCombiner._hasFused)
            return;

        if (otherInfo != null && otherInfo.MineralIndex == _info.MineralIndex)
        {
            int thisID = gameObject.GetInstanceID();
            int otherID = collision.gameObject.GetInstanceID();

            // Se usa el InstanceID para evitar fusiones duplicadas
            if (thisID > otherID)
            {
                StartCoroutine(FusionProcess(collision, otherCombiner));
            }
        }
    }

    private IEnumerator FusionProcess(Collision2D collision, MineralCombiner otherCombiner)
    {
        GameManager.Instance.IncreaseScore(_info.PuntosCuandoAniquilados);

        // Si estamos en el índice máximo, no se fusiona
        if (_info.MineralIndex == MineralesSelector.Instance.Minerales.Length - 1)
        {
            yield break;
        }

        _hasFused = true;
        if (otherCombiner != null)
        {
            otherCombiner._hasFused = true;
        }

        // Desactivamos temporalmente las colisiones de ambos minerales
        Collider2D colThis = GetComponent<Collider2D>();
        Collider2D colOther = collision.gameObject.GetComponent<Collider2D>();

        if (colThis) colThis.enabled = false;
        if (colOther) colOther.enabled = false;

        // Calculamos la posición media de ambos minerales
        Vector3 middlePosition = (transform.position + collision.transform.position) / 2f;
        middlePosition += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
        middlePosition = AdjustFusionPosition(middlePosition);

        // Instanciamos el nuevo mineral fusionado
        GameObject fusedMineral = Instantiate(SpamCombinedMineral(_info.MineralIndex), GameManager.Instance.transform);
        fusedMineral.transform.position = middlePosition;

        // Agregamos un cooldown al nuevo mineral (0.5 segundos)
        FusionCooldown cooldown = fusedMineral.GetComponent<FusionCooldown>();
        if (cooldown == null)
        {
            cooldown = fusedMineral.AddComponent<FusionCooldown>();
            cooldown.cooldownDuration = 0.5f;
        }

        // Reproducimos la animación de fusión
        Fusion fusionScript = fusedMineral.GetComponent<Fusion>();
        if (fusionScript != null)
        {
            fusionScript.PlayFusionEffect();
        }

        // Reproducimos el sonido de fusión
        AudioSource audioSource = fusedMineral.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Marcamos la fusión (esto permite que, desde ColiderInformer, se gestione el spawn del siguiente mineral)
        ColiderInformer informer = fusedMineral.GetComponent<ColiderInformer>();
        if (informer != null)
        {
            informer.WasCombinedIn = true;
        }

        // Destruimos los objetos originales (solo se fusionan cuando realmente se tocan)
        Destroy(collision.gameObject);
        Destroy(gameObject);

        // Esperamos 0.3 segundos para dar tiempo a que se vea la animación
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
