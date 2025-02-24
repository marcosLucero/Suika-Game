using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralCombiner : MonoBehaviour
{
    private int _layerIndex;
    private MineralInfo _info;
    private bool _hasFused = false; // Evita fusiones múltiples simultáneas

    private void Awake()
    {
        _info = GetComponent<MineralInfo>();
        _layerIndex = gameObject.layer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasFused)
            return; // Si este objeto ya está en proceso de fusión, salimos

        // Comprobamos que el objeto colisionado está en la misma layer
        if (collision.gameObject.layer == _layerIndex)
        {
            MineralInfo otherInfo = collision.gameObject.GetComponent<MineralInfo>();
            MineralCombiner otherCombiner = collision.gameObject.GetComponent<MineralCombiner>();

            // Si el otro objeto ya está en proceso de fusión, salimos
            if (otherCombiner != null && otherCombiner._hasFused)
                return;

            // Comprobamos que ambos minerales tienen el mismo índice
            if (otherInfo != null && otherInfo.MineralIndex == _info.MineralIndex)
            {
                int thisID = gameObject.GetInstanceID();
                int otherID = collision.gameObject.GetInstanceID();

                // Solo el objeto con ID mayor se encargará de la fusión (evitando duplicados)
                if (thisID > otherID)
                {
                    GameManager.Instance.IncreaseScore(_info.PuntosCuandoAniquilados);
                    // Si es el último mineral, no se fusiona (se pueden dejar en escena)
                    if (_info.MineralIndex == MineralesSelector.Instance.Minerales.Length - 1)
                    {
                        return;
                    }
                    else
                    {
                        // Marcamos ambos objetos como ya fusionados para evitar fusiones múltiples
                        _hasFused = true;
                        if (otherCombiner != null)
                        {
                            otherCombiner._hasFused = true;
                        }

                        // Calculamos la posición media de los dos minerales
                        Vector3 middlePosition = (transform.position + collision.transform.position) / 2f;
                        // Añadimos un pequeño offset aleatorio para evitar solapamientos exactos
                        middlePosition += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);

                        // Ajustamos la posición para que no quede dentro de una pared
                        middlePosition = AdjustFusionPosition(middlePosition);

                        // Instanciamos el mineral fusionado (el siguiente en el índice)
                        GameObject fusedMineral = Instantiate(SpamCombinedMineral(_info.MineralIndex), GameManager.Instance.transform);
                        fusedMineral.transform.position = middlePosition;

                        // Marcamos, si es necesario, al nuevo mineral (por ejemplo, con un indicador de fusión)
                        ColiderInformer informer = fusedMineral.GetComponent<ColiderInformer>();
                        if (informer != null)
                        {
                            informer.WasCombinedIn = true;
                        }

                        // Destruimos los dos minerales originales
                        Destroy(collision.gameObject);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }


    // Devuelve el mineral del siguiente índice (el que resulta de la fusión)
    private GameObject SpamCombinedMineral(int index)
    {
        return MineralesSelector.Instance.Minerales[index + 1];
    }

    // Ajusta la posición de fusión para evitar que el mineral quede dentro de una pared
    private Vector3 AdjustFusionPosition(Vector3 fusionPos)
    {
        int wallLayer = LayerMask.NameToLayer("Wall");
        // Usamos OverlapPoint para comprobar si el punto está dentro de un collider de pared
        Collider2D wallCollider = Physics2D.OverlapPoint(fusionPos, 1 << wallLayer);
        Vector3 adjustedPos = fusionPos;
        if (wallCollider != null)
        {
            // Obtenemos el punto más cercano fuera de la pared
            Vector2 closestPoint = wallCollider.ClosestPoint(fusionPos);
            // Calculamos la dirección para salir de la pared
            Vector2 pushDirection = ((Vector2)fusionPos - closestPoint).normalized;
            if (pushDirection == Vector2.zero)
            {
                pushDirection = Vector2.up;
            }
            // Empujamos la posición hacia afuera; ajusta pushDistance según convenga
            float pushDistance = 1.5f;
            adjustedPos = fusionPos + (Vector3)(pushDirection * pushDistance);
        }
        return adjustedPos;
    }
}
