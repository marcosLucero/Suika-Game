using System.Collections;
using UnityEngine;

public class FusionCooldown : MonoBehaviour
{
    public float cooldownDuration = 0.5f;
    private bool _canFuse = false;
    public bool CanFuse => _canFuse;

    private void Awake()
    {
        StartCoroutine(ActivateFusion());
    }

    private IEnumerator ActivateFusion()
    {
        yield return new WaitForSeconds(cooldownDuration);
        _canFuse = true;
    }
}
