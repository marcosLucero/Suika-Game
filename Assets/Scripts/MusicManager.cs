using UnityEngine;
using UnityEngine.UI;  // Necesario para Image

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;
    private bool isMuted = false;

    // Variables para cambiar la imagen y el color
    public Image buttonImage;  // La imagen del bot�n
    public Sprite musicOnSprite;  // Imagen cuando la m�sica est� activada
    public Sprite musicOffSprite; // Imagen cuando la m�sica est� desactivada
    private Color activeColor, mutedColor = Color.red; // Colores de los botones

    void Awake()
    {
        // Singleton para evitar duplicados entre escenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // Aseguramos que la imagen del bot�n y el color est�n correctamente al iniciar
        ColorUtility.TryParseHtmlString("#B4B4B4", out activeColor); // Color activo (por defecto)
        UpdateButtonImage(); // Para actualizar imagen y color
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted; // Solo mutea la m�sica

        // Actualizamos la imagen del bot�n y el color
        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? musicOffSprite : musicOnSprite;
            buttonImage.color = isMuted ? mutedColor : activeColor; // Cambia el color seg�n est� apagado o encendido
        }
    }
}
