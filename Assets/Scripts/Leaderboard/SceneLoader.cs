using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void VolverAEscenaPrincipal()
    {
        SceneManager.LoadScene("EscenaPrincipal");
    }

    void Update()
    {
        // Si se presiona la tecla ESC, carga la escena principal
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            VolverAEscenaPrincipal();
        }
    }
}
