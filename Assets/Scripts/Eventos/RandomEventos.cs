using UnityEngine;

public class RandomEvents : MonoBehaviour
{
    private int lastScore = 0;
    private GameManager gameManager;
    private BouncyEffect bouncyEffect;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        bouncyEffect = FindObjectOfType<BouncyEffect>(); // Referencia al script de rebote
    }

    private void Update()
    {
        if (gameManager != null && gameManager.CurrentScore >= lastScore + 50)
        {
            lastScore = gameManager.CurrentScore;
            TriggerBouncyEffect();
        }
    }

    private void TriggerBouncyEffect()
    {
        Debug.Log("Evento activado: Rebote alto por 10 segundos");
        if (bouncyEffect != null)
        {
            bouncyEffect.ActivateBouncyEffect(10f); // Dura 10 segundos ahora
        }
    }
}
