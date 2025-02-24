using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    private float _screenBottomY;

    private void Start()
    {
        // Calculamos la posición mínima en Y que es visible en la cámara
        _screenBottomY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 1f;
    }

    private void Update()
    {
        // Si el objeto baja más allá del límite de la pantalla, se destruye
        if (transform.position.y < _screenBottomY)
        {
            Destroy(gameObject);
        }
    }
}
