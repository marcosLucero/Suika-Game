using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderInformer : MonoBehaviour
{
    public bool WasCombinedIn { get; set; }
    private bool _hasCollided;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasCollided && !WasCombinedIn)
        {
            _hasCollided = true;

            if (ThrowMineralController.instance != null)
            {
                ThrowMineralController.instance.CanThrow = true;

                if (MineralesSelector.Instance.NextFruit != null)
                {
                    ThrowMineralController.instance.SpamMinerles(MineralesSelector.Instance.NextFruit);
                    MineralesSelector.Instance.SiguienteMineral();
                }
                else
                {
                    Debug.LogError("❌ Error: NextFruit es null. No se puede generar el siguiente mineral.");
                }
            }
            else
            {
                Debug.LogError("❌ Error: ThrowMineralController.instance es null. ¿Está en la escena?");
            }

            Destroy(this);
        }
    }
}
