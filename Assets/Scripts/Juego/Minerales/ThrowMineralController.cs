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

    private PlayerController _playerController;

    private Rigidbody2D _rb;
    private PolygonCollider2D _circleCollider;

    public Bounds Bounds { get; private set; }

    private const float Extra_width = 0.2f;

    public bool CanThrow { get; set; } = true;

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
        if (UserInput.IsThrowPressed && CanThrow)
        {
            if (MineralActual == null)
            {
                Debug.LogError("❌ Error: MineralActual es null. No se puede lanzar.");
                return;
            }

            SpriteIndex index = MineralActual.GetComponent<SpriteIndex>();
            Quaternion rot = MineralActual.transform.rotation;

            GameObject newMineral = Instantiate(MineralesSelector.Instance.Minerales[index.Index],
                                                 MineralActual.transform.position, rot);

            newMineral.transform.SetParent(_parienteAntesThrow);
            newMineral.transform.localScale = MineralActual.transform.localScale; // Mantener escala


            Destroy(MineralActual);

            CanThrow = false;
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
}
