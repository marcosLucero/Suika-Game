using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoss : MonoBehaviour
{
    private float _timer = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.gameObject.layer == 7)
        {
            _timer += Time.deltaTime;
            if (_timer > GameManager.Instance.TimeTillGamerOver)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.gameObject.layer == 7)
        {
            _timer = 0f;
        }
    }
}
