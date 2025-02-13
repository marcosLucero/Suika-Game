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
            ThrowMineralController.instance.CanThrow = true;
            ThrowMineralController.instance.SpamMinerles(MineralesSelector.Instance.NextFruit);
            MineralesSelector.Instance.SiguienteMineral();
            Destroy(this);
        }

        
    }
}
