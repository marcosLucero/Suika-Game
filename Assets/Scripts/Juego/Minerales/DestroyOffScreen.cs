using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    private float _screenBottomY;

    private void Start()
    {
        // Calculamos la posici�n m�nima en Y que es visible en la c�mara
        _screenBottomY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 1f;
    }

    private void Update()
    {
        // Si el objeto baja m�s all� del l�mite de la pantalla, se destruye
        if (transform.position.y < _screenBottomY)
        {
            Destroy(gameObject);
        }
    }
}
