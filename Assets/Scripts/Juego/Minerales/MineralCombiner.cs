using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralCombiner : MonoBehaviour
{

    private int _layerIndex;

    private MineralInfo _info;

    private void Awake()
    {
        _info = GetComponent<MineralInfo>();
        _layerIndex = gameObject.layer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _layerIndex)
        {
            MineralInfo info = collision.gameObject.GetComponent<MineralInfo>();
            if (info != null)
            {
                if (info.MineralIndex == _info.MineralIndex)
                {
                    int thisID = gameObject.GetInstanceID();
                    int otherID = collision.gameObject.GetInstanceID();

                    if (thisID > otherID) 
                    {
                        if (_info.MineralIndex == MineralesSelector.Instance.Minerales.Length - 1)
                        {
                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }

                        else
                        {
                            Vector3 middlePosition = (transform.position + collision.transform.position) / 2f;
                            GameObject go = Instantiate(SpamCombinedMineral(_info.MineralIndex), GameManager.Instance.transform);
                            go.transform.position = middlePosition;

                             ColiderInformer informer = go.GetComponent<ColiderInformer>();
                            if (informer != null)
                            {
                                informer.WasCombinedIn = true;
                            }

                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
    private GameObject SpamCombinedMineral(int index)
    {
        GameObject go = MineralesSelector.Instance.Minerales[index + 1];
        return go;
    }
}
// problemas actuales:
/*
 * - Si se mezclan minerales muy cerca de la pared pueden quedarse pillasdos y no caer.
 * - Si se mezclan 3 minerales muy cerca se sumaran dos veces el indice lo cual dara un indice incorrecto ya que solo queremos que se mezcle con uno y no con los dos a la vez
 * - Si se mezclan muy cercas grandes con pequeños aveces se puden quedar unos detro de otros.
 * - Hacer que los ultimos minerales no desaparezcan/no se combinen y se queden en la escena.
 */