using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

public class GameAgent : Agent
{
    [Header("Referencias de la escena")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject box;
    [SerializeField] private ThrowMineralController throwMineralController; // Referencia al controlador
    [SerializeField] private PlayerInput playerInput; // Referencia al componente PlayerInput (asignar en Inspector)

    // Almacena la puntuaci�n previa para comparar
    private int previousScore;

    void Start()
    {
        Debug.Log(" GameAgent Start() ejecutado.");

        // Buscar PlayerInput si no est� asignado en el Inspector
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null && player != null)
            {
                playerInput = player.GetComponent<PlayerInput>();
            }
            if (playerInput == null)
            {
                Debug.LogError("No se encontr� PlayerInput en este GameObject ni en el jugador.");
            }
        }
    }

    public override void Initialize()
    {
        Debug.Log(" Mover a la izquierda.");

        previousScore = gameManager.CurrentScore;

        // Buscar referencias si no est�n asignadas en el Inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("? No se encontr� el jugador. Aseg�rate de asignarlo en el Inspector o ponerle el tag 'Player'.");
            }
        }

        if (throwMineralController == null)
        {
            throwMineralController = FindObjectOfType<ThrowMineralController>();
        }
    }

    public override void OnEpisodeBegin()
    {
        // Reiniciamos el entorno a trav�s del GameManager
        gameManager.OnEpisodeBegin();
        previousScore = gameManager.CurrentScore;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1. Puntuaci�n actual
        sensor.AddObservation(gameManager.CurrentScore);

        // 2. Posici�n X e Y del jugador
        sensor.AddObservation(player.transform.position.x);
        sensor.AddObservation(player.transform.position.y);

        // 3. Posici�n X e Y de la caja
        sensor.AddObservation(box.transform.position.x);
        sensor.AddObservation(box.transform.position.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(" OnActionReceived ha sido llamado");

        // Espacio de acciones discreto: 0 = Nada, 1 = Izquierda, 2 = Derecha, 3 = Lanzar mineral
        int action = actions.DiscreteActions[0];

        Debug.Log($" Acci�n recibida: {action}"); // Agregar esto para depuraci�n

        switch (action)
        {
            case 0:
                Debug.Log(" No hacer nada.");
                break;
            case 1:
                Debug.Log(" Mover a la izquierda.");
                MovePlayer(-1);
                break;
            case 2:
                MovePlayer(1);
                break;
            case 3:
                ThrowMineral();
                break;
        }

        // Recompensa basada en el incremento de puntuaci�n
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

        // Finaliza el episodio si _box o _player se han desactivado (indicando p�rdida)
        if (!box.activeInHierarchy || !player.activeInHierarchy)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        Debug.Log("Heuristic ha sido llamado con Input System. Tiempo: " + Time.time);

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput no est� asignado. No se pueden procesar acciones.");
            discreteActions[0] = 0; // Acci�n por defecto (nada)
            return;
        }

        Debug.Log("PlayerInput encontrado, accediendo a acciones...");
        float move = playerInput.actions["Move"].ReadValue<float>(); // -1 (izquierda: A o flecha izquierda), 0 (nada), 1 (derecha: D o flecha derecha)
        bool throwAction = playerInput.actions["Throw"].WasPressedThisFrame(); // Detecta si se presion� Space

        if (move < 0)
        {
            discreteActions[0] = 1; // Mover a la izquierda
            Debug.Log("Asignada acci�n: Mover izquierda");
        }
        else if (move > 0)
        {
            discreteActions[0] = 2; // Mover a la derecha
            Debug.Log("Asignada acci�n: Mover derecha");
        }
        else if (throwAction)
        {
            discreteActions[0] = 3; // Lanzar mineral
            Debug.Log("Asignada acci�n: Lanzar mineral");
        }
        else
        {
            discreteActions[0] = 0; // No hacer nada
            Debug.Log("Asignada acci�n: No hacer nada");
        }
    }

    /// <summary>
    /// M�todo para mover el jugador.
    /// </summary>
    private void MovePlayer(int direction)
    {
        if (player == null)
        {
            Debug.LogError("? El jugador no est� asignado. No se puede mover.");
            return;
        }

        float moveSpeed = 5f;
        Debug.Log($"?? Moviendo al jugador en direcci�n {direction}");

        // Movimiento en 2D (modifica solo X)
        player.transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// M�todo para lanzar un mineral.
    /// Implementa la l�gica para que el jugador lance el mineral.
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