using UnityEngine;

public class MineralInfo : MonoBehaviour
{
    public int MineralIndex = 0;  // El índice del mineral
    public int PuntosCuandoAniquilados = 1;  // Puntos cuando se aniquila

    public float MineralMassa = 1f;  // Masa del mineral

    private Rigidbody2D _rb;
    private AudioSource _audioSource;  // AudioSource para el mineral

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.mass = MineralMassa;

        _audioSource = GetComponent<AudioSource>();  // Obtener el AudioSource

        if (_audioSource != null)
        {
            SoundManager.Instance.RegisterAudioSource(_audioSource);  // Registrar el AudioSource en el SoundManager

            // Establecer el estado del sonido de inmediato (según el estado actual)
            _audioSource.mute = SoundManager.Instance.IsMuted();  // Asegurarse de que el sonido esté muteado o no
        }
    }

}
