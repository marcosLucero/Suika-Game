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
        if (Input.GetKeyDown(KeyCode.Escape) ||
            (UnityEngine.InputSystem.Gamepad.current != null && UnityEngine.InputSystem.Gamepad.current.buttonNorth.wasPressedThisFrame))
        {
            VolverAEscenaPrincipal();
        }
    }
}
