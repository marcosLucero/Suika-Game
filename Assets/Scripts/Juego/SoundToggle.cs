using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public Image buttonImage; // Imagen del botón
    public Sprite soundOnSprite; // Icono sonido ON
    public Sprite soundOffSprite; // Icono sonido OFF

    private bool isMuted = false;
    private Color activeColor, mutedColor = Color.red; // Color rojo cuando está apagado

    // Lista para almacenar los AudioSource de los efectos de sonido
    private List<AudioSource> effectAudioSources = new List<AudioSource>();

    void Start()
    {
        // Convierte #B4B4B4 a Color
        ColorUtility.TryParseHtmlString("#B4B4B4", out activeColor);
        UpdateButton();
    }

    // Método para agregar AudioSource a la lista (lo llamarás desde otros scripts donde se crean los sonidos)
    public void AddAudioSource(AudioSource audioSource)
    {
        if (audioSource != null && !effectAudioSources.Contains(audioSource))
        {
            effectAudioSources.Add(audioSource);
        }
    }

    // Método para alternar entre sonido activado y desactivado
    public void ToggleSound()
    {
        isMuted = !isMuted;

        // Mute o desmute los efectos de sonido
        foreach (AudioSource audioSource in effectAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.mute = isMuted; // Mute o desmute el AudioSource
            }
        }

        UpdateButton(); // Actualiza la imagen y el color del botón
    }

    // Método para actualizar la imagen y color del botón según el estado del sonido
    private void UpdateButton()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
            buttonImage.color = isMuted ? mutedColor : activeColor;
        }
    }
}
