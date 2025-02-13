using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineralesSelector : MonoBehaviour
{
    public static MineralesSelector Instance;

    public GameObject[] Minerales;
    public GameObject[] NoPhysicsMinerales;
    public int HigestStartingIndex = 3; // por el mineral que empieza?

    [SerializeField] private Image _nextMineralImage;
    [SerializeField] private Sprite[] _mineralSprites;

    public GameObject NextFruit { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("❌ Error: Ya existe una instancia de MineralesSelector.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SiguienteMineral(); // ✅ Aseguramos que NextFruit tenga un valor inicial
    }

    public GameObject RandomMineralThrow()
    {
        int randomIndex = Random.Range(0, HigestStartingIndex + 1);

        if (randomIndex < NoPhysicsMinerales.Length)
        {
            GameObject randomMineral = NoPhysicsMinerales[randomIndex];
            return randomMineral;
        }

        Debug.LogError("❌ Error: No se encontró un mineral aleatorio válido.");
        return null;
    }

    public void SiguienteMineral()
    {
        int randomIndex = Random.Range(0, HigestStartingIndex + 1);

        if (randomIndex < NoPhysicsMinerales.Length)
        {
            NextFruit = NoPhysicsMinerales[randomIndex]; // ✅ Se asigna correctamente el próximo mineral
            Debug.Log($"✅ Nuevo NextFruit asignado: {NextFruit.name}");
        }
        else
        {
            Debug.LogError("❌ Error: No se pudo asignar un nuevo NextFruit.");
            NextFruit = null; // Evita errores
        }

        if (randomIndex < _mineralSprites.Length)
        {
            _nextMineralImage.sprite = _mineralSprites[randomIndex]; // ✅ Se actualiza la UI
        }
    }
}
