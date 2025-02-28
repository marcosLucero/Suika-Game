using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;

public class LeaderboardDisplayManager : MonoBehaviour
{
    public Transform contentPanel;  // Contenedor para instanciar las filas del leaderboard
    public GameObject rowPrefab;    // Prefab de la fila del leaderboard

    private DatabaseReference dbRef;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                dbRef = FirebaseDatabase.GetInstance(app, "https://cozy-forge-default-rtdb.firebaseio.com/").RootReference;

                Debug.Log("Firebase inicializado correctamente en LeaderboardDisplayManager.");
                LoadLeaderboard();
            }
            else
            {
                Debug.LogError("No se pudieron resolver las dependencias de Firebase: " + task.Result);
            }
        });
    }



    public void LoadLeaderboard()
    {
        // Consultamos el nodo "players" donde se guardan los datos de cada jugador
        dbRef.Child("players").OrderByChild("score").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<PlayerScore> entries = new List<PlayerScore>();

                foreach (DataSnapshot child in snapshot.Children)
                {
                    string name = child.Child("playerName").Value != null ? child.Child("playerName").Value.ToString() : "Unknown";
                    int score = child.Child("score").Value != null ? int.Parse(child.Child("score").Value.ToString()) : 0;

                    entries.Add(new PlayerScore(name, score));
                }


                // Ordenamos de mayor a menor puntuación
                entries.Sort((a, b) => b.score.CompareTo(a.score));

                // Limpiar contenido anterior
                foreach (Transform child in contentPanel)
                {
                    Destroy(child.gameObject);
                }

                // Instanciar una fila para cada entrada
                foreach (PlayerScore entry in entries)
                {
                    GameObject newRow = Instantiate(rowPrefab, contentPanel); // Crear instancia
                    RowDisplay rowDisplay = newRow.GetComponent<RowDisplay>(); // Obtener componente

                    if (rowDisplay != null)
                    {
                        rowDisplay.SetRow(entry.name, entry.score); // Asignar valores
                    }
                    else
                    {
                        Debug.LogError("No se encontró RowDisplay en el prefab instanciado.");
                    }
                }

            }
            else
            {
                Debug.LogError("Error al cargar el leaderboard: " + task.Exception);
            }
        });
    }
}

[System.Serializable]
public class PlayerScore
{
    public string name;
    public int score;

    public PlayerScore(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
