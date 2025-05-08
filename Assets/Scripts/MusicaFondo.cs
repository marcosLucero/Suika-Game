using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicaFondo : MonoBehaviour
{
    private static MusicaFondo instance;

    void Awake()
    {
        // Solo se debe crear si no existe otro objeto MusicaFondo
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    void OnEnable()
    {
        // Nos suscribimos al evento de carga de una nueva escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Nos desuscribimos cuando este objeto se destruya
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destruye la música si no es una de las escenas deseadas
        if (scene.name != "EscenaPrincipal" && scene.name != "Leaderboard"  && scene.name != "Guia")
        {
            Destroy(gameObject);
        }
    }
}
