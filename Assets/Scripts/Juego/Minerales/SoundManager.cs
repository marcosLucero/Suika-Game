using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private List<AudioSource> effectAudioSources = new List<AudioSource>();  // Lista de sonidos de efectos
    private bool isMuted = false;  // Estado de mute

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persistente entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Método para registrar un AudioSource nuevo (cuando un mineral es instanciado)
    public void RegisterAudioSource(AudioSource source)
    {
        if (source != null && !effectAudioSources.Contains(source))
        {
            effectAudioSources.Add(source);  // Añadir el AudioSource a la lista
        }
    }

    // Método para alternar el sonido (activar/desactivar)
    public void ToggleSound()
    {
        isMuted = !isMuted;

        // Mute o desmute los sonidos de todos los `AudioSource` registrados
        foreach (AudioSource audioSource in effectAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.mute = isMuted;
            }
        }

        // También puedes asegurarte de que el estado de todos los nuevos AudioSource se aplique al instante
        foreach (var mineral in FindObjectsOfType<MineralInfo>())
        {
            AudioSource audioSource = mineral.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.mute = isMuted;
            }
        }
    }


    // Método para obtener el estado actual del sonido
    public bool IsMuted()
    {
        return isMuted;
    }
}
