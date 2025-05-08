using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectFirstButtonOnReturn : MonoBehaviour
{
    [SerializeField] private Button defaultButton;
    private static bool isFirstLoad = true;

    void Start()
    {
        if (isFirstLoad)
        {
            // Es la primera vez que se carga la escena, no seleccionamos nada
            isFirstLoad = false;
            return;
        }

        // Si no es la primera vez, seleccionamos el botón
        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
    }

    void OnEnable()
    {
        if (isFirstLoad)
        {
            return;
        }

        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
    }
}
