using UnityEngine;

public class MineralSound : MonoBehaviour
{
    private AudioSource audioSource;
    private SoundManager soundManager;

    void Start()
    {
        // Obt�n la referencia al SoundManager
        soundManager = FindObjectOfType<SoundManager>();

        if (soundManager != null)
        {
            // Obtener el AudioSource del mineral
            audioSource = GetComponent<AudioSource>();

            // Agregar este AudioSource al SoundManager
            soundManager.AddAudioSource(audioSource);
        }
    }

    // Aqu� ir�an los otros m�todos que gestionan la l�gica de los sonidos, como el de la fusi�n.
}
