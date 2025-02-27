using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;  // Usamos TMP_InputField

    private int score = 0;
    private FirebaseManager firebaseManager;

    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
    }

    public void SaveData()
    {
        playerNameInput.ForceLabelUpdate(); // Forzar actualización visual del input
        string playerName = playerNameInput.text.Trim(); // Quitar espacios extra

        if (string.IsNullOrEmpty(playerName))
            playerName = "Jugador_" + UnityEngine.Random.Range(1000, 9999); // Nombre por defecto

        firebaseManager.SavePlayerData(playerName, score);
        Debug.Log("Nombre guardado: " + playerName); // Comprobar en la consola
        Debug.Log("Nombre enviado a FirebaseManager: " + playerName);


    }


}
