using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con InputField
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Auth;
using System.Collections;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    private bool isFirebaseInitialized = false;

    public TMP_InputField playerNameInput;  // Ahora es compatible con TMP

    // Inicializamos Firebase
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Aseg�rate de tener la URL de tu base de datos de Firebase aqu�
                string databaseUrl = "https://cozy-forge-default-rtdb.firebaseio.com/";

                // Asegura que Firebase Database use la URL correcta
                FirebaseDatabase firebaseDatabase = FirebaseDatabase.GetInstance(app, databaseUrl);
                reference = firebaseDatabase.RootReference;

                isFirebaseInitialized = true;
                Debug.Log("Firebase est� listo.");
            }
            else
            {
                Debug.LogError("Firebase no pudo inicializarse. Int�ntalo m�s tarde.");
            }
        });
    }

    // Guardar el nombre y la puntuaci�n
    public void SavePlayerData(string playerName, int score)
    {
        if (!isFirebaseInitialized) // Verificamos si Firebase est� listo
        {
            Debug.LogError("Firebase no est� inicializado. Int�ntalo m�s tarde.");
            return;
        }

        string playerId = System.Guid.NewGuid().ToString();  // Crear un ID �nico para cada jugador
        PlayerData playerData = new PlayerData(playerName, score);

        string json = JsonUtility.ToJson(playerData);  // Convertir los datos a JSON
        reference.Child("players").Child(playerId).SetRawJsonValueAsync(json);  // Guardar los datos en Firebase
        Debug.Log("Guardando en Firebase: Nombre = " + playerName + ", Puntuaci�n = " + score);

    }


    // Cargar todos los jugadores
    public void LoadAllPlayers()
    {
        if (!isFirebaseInitialized) // Verificamos si Firebase est� listo
        {
            Debug.LogError("Firebase no est� inicializado. Int�ntalo m�s tarde.");
            return;
        }

        reference.Child("players").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener los datos");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot playerSnapshot in snapshot.Children)
                {
                    string playerName = playerSnapshot.Child("playerName").Value.ToString();
                    int score = int.Parse(playerSnapshot.Child("score").Value.ToString());
                    Debug.Log($"Jugador: {playerName} | Puntuaci�n: {score}");
                }
            }
        });
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int score;

    public PlayerData(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}
