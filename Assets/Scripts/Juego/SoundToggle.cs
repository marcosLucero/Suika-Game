using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public Image buttonImage; // Imagen del botón
    public Sprite soundOnSprite; // Icono de sonido activado
    public Sprite soundOffSprite; // Icono de sonido desactivado

    private Color activeColor, mutedColor = Color.red; // Color rojo cuando está apagado

    private void Start()
    {
        activeColor = Color.white; // Color blanco cuando está activo
        UpdateButton();
    }

    private void Update()
    {
        // Detectar el botón Start del mando (JoystickButton7)
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            ToggleSound();
        }
    }

    public void ToggleSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ToggleSound();  // Cambiar el estado de sonido
            UpdateButton();  // Actualizar la imagen del botón
        }
    }

    private void UpdateButton()
    {
        if (buttonImage != null && SoundManager.Instance != null)
        {
            bool isMuted = SoundManager.Instance.IsMuted();  // Obtener el estado de mutado
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
            buttonImage.color = isMuted ? mutedColor : activeColor;
        }
    }
}
