using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMineralController : MonoBehaviour
{
    public static ThrowMineralController instance;

    public GameObject MineralActual { get; set; }
    [SerializeField] private Transform _mineralesTransform;
    [SerializeField] private Transform _parienteAntesThrow;
    [SerializeField] private MineralesSelector _selector;
    [SerializeField] private Animator throwAnimator; // Referencia al Animator
    [SerializeField] private Animator fusionAnimator; // Referencia al Animator de fusión

    private PlayerController _playerController;

    private Rigidbody2D _rb;
    private PolygonCollider2D _circleCollider;

    public Bounds Bounds { get; private set; }

    private const float Extra_width = 0.2f;

    public bool CanThrow { get; set; } = true;
    private int eventActive = 0;

    private bool _isAnimating = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("❌ Error: Hay más de un ThrowMineralController en la escena.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();

        GameObject firstMineral = _selector.RandomMineralThrow();
        if (firstMineral != null)
        {
            SpamMinerles(firstMineral);
        }
        else
        {
            Debug.LogError("❌ Error: No se pudo generar el primer mineral.");
        }
    }

    private void Update()
    {
        if (eventActive == 0)
        {
            if (UserInput.IsThrowPressed && CanThrow && !_isAnimating)
            {
                if (MineralActual == null)
                {
                    Debug.LogError("❌ Error: MineralActual es null. No se puede lanzar.");
                    return;
                }

                _isAnimating = true;
                SpriteIndex index = MineralActual.GetComponent<SpriteIndex>();
                Quaternion rot = MineralActual.transform.rotation;

                // Reproducir la animación de lanzamiento
                if (throwAnimator != null)
                {
                    Debug.Log("Activando animación de lanzamiento");
                    throwAnimator.SetTrigger("Throw"); // Activar el trigger de la animación
                }

                // Reproducir la animación de fusión
                if (fusionAnimator != null)
                {
                    Debug.Log("Activando animación de fusión");
                    fusionAnimator.SetTrigger("Throw"); // Activar el trigger de la animación
                }

                GameObject newMineral = Instantiate(MineralesSelector.Instance.Minerales[index.Index],
                                                     MineralActual.transform.position, rot);

                newMineral.transform.SetParent(_parienteAntesThrow);
                newMineral.transform.localScale = MineralActual.transform.localScale; // Mantener escala

                Destroy(MineralActual);

                CanThrow = false;
                StartCoroutine(ResetAnimationFlag());
            }
        }
    }

    private IEnumerator ResetAnimationFlag()
    {
        yield return new WaitForSeconds(0.4f); // Esperar un poco más que la duración de la animación
        _isAnimating = false;
        if (throwAnimator != null)
        {
            throwAnimator.ResetTrigger("Throw"); // Resetear el trigger después de la animación
        }
        if (fusionAnimator != null)
        {
            fusionAnimator.ResetTrigger("Throw"); // Resetear el trigger después de la animación
        }
    }

    public void SpamMinerles(GameObject mineral)
    {

        if (mineral == null)
        {
            Debug.LogError("❌ Error: No se puede instanciar un mineral null.");
            return;
        }

        GameObject go = Instantiate(mineral, _mineralesTransform);
        MineralActual = go;
        _circleCollider = MineralActual.GetComponent<PolygonCollider2D>();

        if (_circleCollider != null)
        {
            Bounds = _circleCollider.bounds;
        }
        else
        {
            Debug.LogError("❌ Error: No se encontró PolygonCollider2D en el mineral.");
        }

        _playerController.ChangeBoundary(Extra_width);

        VanishingEvent vanishingEvent = FindObjectOfType<VanishingEvent>();
        if (vanishingEvent != null)
        {
            vanishingEvent.RegisterNewMineral(go);
        }
    }
    public void isEventActive( int value)
    {
        eventActive = value;
    }
}
