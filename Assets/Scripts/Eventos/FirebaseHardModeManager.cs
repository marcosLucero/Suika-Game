using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Auth;
using System.Collections;
using TMPro;

public class FirebaseHardModeManager : MonoBehaviour
{
    private DatabaseReference reference;
    private bool isFirebaseInitialized = false;

    public TMP_InputField playerNameInput;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                string databaseUrl = "https://cozy-forge-default-rtdb.firebaseio.com/";

                FirebaseDatabase firebaseDatabase = FirebaseDatabase.GetInstance(app, databaseUrl);
                reference = firebaseDatabase.RootReference;

                isFirebaseInitialized = true;
                Debug.Log("Firebase para Modo Difícil está listo.");
            }
            else
            {
                Debug.LogError("Firebase no pudo inicializarse.");
            }
        });
    }

    public void SaveHardModePlayerData(string playerName, int score)
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase no está inicializado.");
            return;
        }

        string playerId = System.Guid.NewGuid().ToString();
        HardModePlayerData playerData = new HardModePlayerData(playerName, score);

        string json = JsonUtility.ToJson(playerData);
        reference.Child("hard_mode_players").Child(playerId).SetRawJsonValueAsync(json);
        Debug.Log("Guardando en Firebase (Modo Difícil): " + playerName + " | Puntuación: " + score);
    }

    public void LoadAllHardModePlayers()
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase no está inicializado.");
            return;
        }

        reference.Child("hard_mode_players").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener los datos del Modo Difícil.");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot playerSnapshot in snapshot.Children)
                {
                    string playerName = playerSnapshot.Child("playerName").Value.ToString();
                    int score = int.Parse(playerSnapshot.Child("score").Value.ToString());
                    Debug.Log($"Jugador (Modo Difícil): {playerName} | Puntuación: {score}");
                }
            }
        });
    }
}

[System.Serializable]
public class HardModePlayerData
{
    public string playerName;
    public int score;

    public HardModePlayerData(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}
