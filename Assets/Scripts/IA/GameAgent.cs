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

    // Almacena la puntuación previa para comparar
    private int previousScore;

    public override void Initialize()
    {
        // Guarda la puntuación inicial
        previousScore = gameManager.CurrentScore;
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

        // 2. Posición del jugador y del box
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(box.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Espacio de acciones discreto: 0 = Nada, 1 = Izquierda, 2 = Derecha, 3 = Lanzar mineral
        int action = actions.DiscreteActions[0];

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
        float moveSpeed = 5f;
        player.transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Método para lanzar un mineral.
    /// Implementa la lógica para que el jugador lance el mineral.
    /// </summary>
    private void ThrowMineral()
    {
        // Llama al método correspondiente del GameManager o controlador del jugador.
        // Ejemplo:
        // player.GetComponent<PlayerController>().ThrowMineral();
    }
}
