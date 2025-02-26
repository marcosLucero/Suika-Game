using UnityEngine;
using UnityEngine.UI;  // Necesario para Image

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;
    private bool isMuted = false;

    // Variables para cambiar la imagen
    public Image buttonImage;  // La imagen del botón
    public Sprite musicOnSprite;  // Imagen cuando la música está activada
    public Sprite musicOffSprite; // Imagen cuando la música está desactivada

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

        // Aseguramos que la imagen del botón esté correctamente al iniciar
        UpdateButtonImage();
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted; // Solo mutea la música

        // Actualizamos la imagen del botón
        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? musicOffSprite : musicOnSprite;
        }
    }
}
