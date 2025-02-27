using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;

public class LeaderboardManager : MonoBehaviour
{
    public List<TextMeshProUGUI> nombres;      // Asigna en el Inspector
    public List<TextMeshProUGUI> puntuaciones; // Asigna en el Inspector

    private DatabaseReference dbReference;

    void Start()
    {
        string databaseUrl = "https://cozy-forge-default-rtdb.firebaseio.com/";
        dbReference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance, databaseUrl).RootReference;
        CargarMejoresPuntuaciones();
    }


    public void CargarMejoresPuntuaciones()
    {
        
        // Aquí se asume que en Firebase los datos se guardan con los mismos campos: "playerName" y "score"
        dbReference.Child("players").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<PlayerData> topPlayers = new List<PlayerData>();

                foreach (DataSnapshot player in snapshot.Children)
                {
                    // Utilizamos los mismos campos que PlayerData (definido en FirebaseManager)
                    string playerName = player.Child("playerName").Value.ToString();
                    int score = int.Parse(player.Child("score").Value.ToString());
                    topPlayers.Add(new PlayerData(playerName, score));
                }

                // Ordenar por puntuación descendente
                topPlayers.Sort((x, y) => y.score.CompareTo(x.score));

                for (int i = 0; i < nombres.Count; i++)
                {
                    if (i < topPlayers.Count)
                    {
                        nombres[i].text = topPlayers[i].playerName;
                        puntuaciones[i].text = topPlayers[i].score.ToString();
                    }
                    else
                    {
                        nombres[i].text = "-";
                        puntuaciones[i].text = "-";
                    }
                }
            }
            else
            {
                Debug.LogError("Error al obtener los datos: " + task.Exception);
            }
        });
    }
}
