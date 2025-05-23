using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SimpleLocalization : MonoBehaviour
{
    public static SimpleLocalization Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.HasKey("IdiomaSeleccionado"))
        {
            currentLanguageIndex = PlayerPrefs.GetInt("IdiomaSeleccionado");
        }
        else
        {
            currentLanguageIndex = 0; // Español por defecto
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [System.Serializable]
    public class LocalizedText
    {
        public string key; // Ej: "title"
        public TextMeshProUGUI tmpText;
        public Text legacyText;
    }

    public LocalizedText[] textsToChange;
    private int currentLanguageIndex = 0;

    private Dictionary<string, string[]> translations = new Dictionary<string, string[]>()
    {
        { "Bienvenida", new string[] { "Bienvenido a Cozy Forge", "Welcome to Cozy Forge" } },
        { "Bienvenida2", new string[] { "Puedes usar el teclado o el mando para jugar a mi juego.\n\nEspero que lo disfrutes.", "You can use the keyboard or controller to play my game.\n\nI hope you enjoy it." } },
        { "Jugar", new string[] { "Jugar", "Play" } },
        { "LeaderBoard", new string[] { "Puntuacion", "Leaderboard" } },
        { "Guia", new string[] { "Guia", "Guide" } },
        { "Salir", new string[] { "Salir", "Exit" } },
        { "ModoNormal", new string[] { "Modo Normal", "Normal Mode" } },
        { "ModoEventos", new string[] { "Modo Eventos", "Events Mode" } },
        { "BotonContinuar", new string[] { "Continuar", "Continue" } },
        //EscenaLeaderboard
        { "ModEven", new string[] { "Modo con eventos", "Events Mode" } },
        { "ModNormal", new string[] { "Modo normal", "Normal Mode" } },
        // EscenaGuia
        { "Guia0", new string[] { "Bienvenido al tutorial\n\nDonde podras ver las funciones del juego explicadas\n\nPudes usar A/D o ←→", "Welcome to the tutorial\n\nHere you can see the game functions explained\n\nYou can use A/D or ←→" } },
        { "Guia1", new string[] { "Aqui puedes ver la puntuacion actual", "Here you can see your current score" } },
        { "Guia2", new string[] { "Aqui podras ver un top 5 mejores puntuaciones", "Here you can see the top 5 best scores" } },
        { "Guia3", new string[] { "Este sera tu personaje", "This will be your character" } },
        { "Guia4", new string[] { "Esta sera la zona donde trascurre el juego", "This will be the area where the game takes place" } },
        { "Guia5", new string[] { "Aqui puedes desactivar/activar los sonidos y musica del juego", "Here you can turn the game sounds and music on/off" } },
        { "Guia6", new string[] { "Aqui puedes ver el siguiente mineral que podras lanzar", "Here you can see the next mineral you will be able to throw" } },
        { "Guia7", new string[] { "Aqui puedes ver el flujo de la forja", "Here you can see the forge flow" } },
        { "Guia8", new string[] { "Tu objetivo es hacer puntos combinando minerales que sean iguales.", "Your goal is to score points by combining matching minerals." } },
        // EscenaModoNormal
        { "BienvenidoModNormal", new string[] { "Bienvenido al Modo Normal", "Welcome to Normal Mode" } },
        { "LoreModNormal", new string[] { "Tus controles son\n\nPara el movimiento:\nPuedes usar tanto \"A\" y \"D\" o las flechas ←→\n\nPara lanzar minerales:\nUsas la \"barra espaciadora\" o la \"A\" en Mando", "Your controls are\n\nFor movement:\nYou can use either \"A\" and \"D\" or the arrow keys ←→\n\nTo throw minerals:\nUse the \"space bar\" or \"A\" on the controller" } },
        { "ModNormalCont", new string[] { "Continuar", "Continue" } },
        { "ModNormalTunombre", new string[] { "Escribe tu nombre", "Enter your name" } },
        { "ModNormalEnviar", new string[] { "Enviar", "Submit" } },
        { "ModNormalConfirmartexto", new string[] { "Nombre enviado correctamente", "Name submitted successfully" } },
        { "ModNormalBotonVolver", new string[] { "Volver al menu", "Back to menu" } },
        { "ModNormalHasperdido", new string[] { "HAS PERDIDO", "YOU LOST" } },
        { "ModNormalPuntuacion", new string[] { "PUNTUACIÓN", "SCORE" } },
        // EscenaModoEventos
        { "BienvenidoModEventos", new string[] { "Bienvenido al Modo Eventos", "Welcome to Events Mode" } },
        { "LoreModEventos", new string[] { "Tus controles son\n\nPara el movimiento:\nPuedes usar tanto \"A\" y \"D\" o las flechas ←→\n\nPara lanzar minerales:\nUsas la \"barra espaciadora\" o la \"A\" en Mando", "Your controls are\n\nFor movement:\nYou can use either \"A\" and \"D\" or the arrow keys ←→\n\nTo throw minerals:\nUse the \"space bar\" or \"A\" on the controller" } },
        { "ModEventosCont", new string[] { "Continuar", "Continue" } },
        { "ModEventosPuntuacion", new string[] { "PUNTUACIÓN", "SCORE" } },
        { "ModEventosTunombre", new string[] { "Escribe tu nombre", "Enter your name" } },
        { "ModEventosEnviar", new string[] { "Enviar", "Submit" } },
        { "ModEventosConfirmartexto", new string[] { "Nombre enviado correctamente", "Name submitted successfully" } },
        { "ModEventosBotonVolver", new string[] { "Volver al menu", "Back to menu" } },
        { "ModEventosHasperdido", new string[] { "HAS PERDIDO", "YOU LOST" } },
        { "ModEventosTuPuntuacion", new string[] { "Tu puntuacion: 0", "Your score: 0" } }
    };

    public void SetLanguage(int langIndex) // 0 = español, 1 = inglés
    {
        currentLanguageIndex = langIndex;
        PlayerPrefs.SetInt("IdiomaSeleccionado", langIndex);
        PlayerPrefs.Save();
        foreach (var item in textsToChange)
        {
            if (translations.ContainsKey(item.key))
            {
                string newText = translations[item.key][langIndex];
                if (item.tmpText != null)
                    item.tmpText.text = newText;
                if (item.legacyText != null)
                    item.legacyText.text = newText;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Busca todos los LocalizedTextComponent en la escena
        var localizedComponents = FindObjectsOfType<LocalizedTextComponent>();
        textsToChange = new LocalizedText[localizedComponents.Length];
        for (int i = 0; i < localizedComponents.Length; i++)
        {
            textsToChange[i] = new LocalizedText
            {
                key = localizedComponents[i].key,
                tmpText = localizedComponents[i].tmpText,
                legacyText = localizedComponents[i].legacyText
            };
        }
        // Aplica el idioma actual
        SetLanguage(currentLanguageIndex);
    }

    public void UpdateSingleText(LocalizedTextComponent comp)
    {
        if (translations.ContainsKey(comp.key))
        {
            string newText = translations[comp.key][currentLanguageIndex];
            if (comp.tmpText != null)
                comp.tmpText.text = newText;
            if (comp.legacyText != null)
                comp.legacyText.text = newText;
        }
    }

    void Update()
    {
        // Solo cambiar el idioma en la escena principal
        if (SceneManager.GetActiveScene().name == "EscenaPrincipal")
        {
            // Check for Start button (English)
            if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
            {
                SetLanguage(1); // Switch to English
            }
            // Check for Select button (Spanish)
            else if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
            {
                SetLanguage(0); // Switch to Spanish
            }
        }
    }
}