using UnityEngine;

public class MineralSound : MonoBehaviour
{
    private AudioSource audioSource;
    private SoundManager soundManager;

    void Start()
    {
        // Obtén la referencia al SoundManager
        soundManager = FindObjectOfType<SoundManager>();

        if (soundManager != null)
        {
            // Obtener el AudioSource del mineral
            audioSource = GetComponent<AudioSource>();

            // Agregar este AudioSource al SoundManager
            soundManager.AddAudioSource(audioSource);
        }
    }

    // Aquí irían los otros métodos que gestionan la lógica de los sonidos, como el de la fusión.
}
