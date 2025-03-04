using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GameAgent : Agent
{
    [Header("Referencias de la escena")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject box;
    [SerializeField] private ThrowMineralController throwMineralController; // Referencia al controlador

    // Almacena la puntuación previa para comparar
    private int previousScore;

    public override void Initialize()
    {
        previousScore = gameManager.CurrentScore;

        // Buscar referencias si no están asignadas en el Inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("? No se encontró el jugador. Asegúrate de asignarlo en el Inspector o ponerle el tag 'Player'.");
            }
        }

        if (throwMineralController == null)
        {
            throwMineralController = FindObjectOfType<ThrowMineralController>();
        }
    }

    public override void OnEpisodeBegin()
    {
        // Reiniciamos el entorno a través del GameManager
        gameManager.OnEpisodeBegin();
        previousScore = gameManager.CurrentScore;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1. Puntuación actual
        sensor.AddObservation(gameManager.CurrentScore);

        // 2. Posición X e Y del jugador
        sensor.AddObservation(player.transform.position.x);
        sensor.AddObservation(player.transform.position.y);

        // 3. Posición X e Y de la caja
        sensor.AddObservation(box.transform.position.x);
        sensor.AddObservation(box.transform.position.y);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(" OnActionReceived ha sido llamado");


        // Espacio de acciones discreto: 0 = Nada, 1 = Izquierda, 2 = Derecha, 3 = Lanzar mineral
        int action = actions.DiscreteActions[0];

        Debug.Log($" Acción recibida: {action}"); // Agregar esto para depuración


        switch (action)
        {
            case 0:
                break;
            case 1:
                MovePlayer(-1);
                break;
            case 2:
                MovePlayer(1);
                break;
            case 3:
                ThrowMineral();
                break;
        }

        // Recompensa basada en el incremento de puntuación
        int currentScore = gameManager.CurrentScore;
        if (currentScore > previousScore)
        {
            AddReward(1.0f * (currentScore - previousScore));
            previousScore = currentScore;
        }
        else
        {
            AddReward(-0.01f);
        }

        // Finaliza el episodio si _box o _player se han desactivado (indicando pérdida)
        if (!box.activeInHierarchy || !player.activeInHierarchy)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        Debug.Log(" Heuristic ha sido llamado");

        // Pruebas manuales con teclado
        if (Input.GetKey(KeyCode.A))
            discreteActions[0] = 1; // Mover a la izquierda
        else if (Input.GetKey(KeyCode.D))
            discreteActions[0] = 2; // Mover a la derecha
        else if (Input.GetKeyDown(KeyCode.Space))
            discreteActions[0] = 3; // Lanzar mineral
        else
            discreteActions[0] = 0; // No hacer nada
    }

    /// <summary>
    /// Método para mover el jugador.
    /// </summary>
    private void MovePlayer(int direction)
    {
        if (player == null)
        {
            Debug.LogError("? El jugador no está asignado. No se puede mover.");
            return;
        }

        float moveSpeed = 5f;
        Debug.Log($"?? Moviendo al jugador en dirección {direction}");

        // Movimiento en 2D (modifica solo X)
        player.transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Método para lanzar un mineral.
    /// Implementa la lógica para que el jugador lance el mineral.
    /// </summary>
    private void ThrowMineral()
    {
        if (throwMineralController != null && throwMineralController.CanThrow)
        {
            throwMineralController.CanThrow = false; // Evita lanzamientos repetidos
            throwMineralController.SpamMinerles(throwMineralController.MineralActual);
        }
    }
}
