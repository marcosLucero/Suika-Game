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
    private CircleCollider2D _circleCollider;

    public Bounds Bounds { get; private set; }

    private const float Extra_Tamaño = 0.2f;

    public bool CanThrow { get; set; } = true;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();

        SpamMinerles(_selector.RandomMineralThrow());
    }

    private void Update()
    {
        if (UserInput.IsThrowPressed && CanThrow)
        {
            SpriteIndex index = MineralActual.GetComponent<SpriteIndex>();
            Quaternion rot = MineralActual.transform.rotation;

            GameObject go = Instantiate(MineralesSelector.Instance.Minerales[index.Index], MineralActual.transform.position, rot);
            go.transform.SetParent(_parienteAntesThrow);

            Destroy(MineralActual);

            CanThrow = false;
        }
    }

    public void SpamMinerles(GameObject mineral)
    {
        GameObject go = Instantiate(mineral, _mineralesTransform);
        MineralActual = go;
        _circleCollider = MineralActual.GetComponent<CircleCollider2D>();
        Bounds = _circleCollider.bounds;
    }
}
