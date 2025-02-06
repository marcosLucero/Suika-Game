using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineralesSelector : MonoBehaviour
{
    public static MineralesSelector Instance;

    public GameObject[] Minerales;
    public GameObject[] NoPhysicsMinerales;
    public int HigestStartingIndex = 3;// por el mineral que empieza?

    [SerializeField] private Image _nextMineralImage;
    [SerializeField] private Sprite[] _mineralSprites;

    public GameObject NextFruit { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SiguienteMineral();
    }


    public GameObject RandomMineralThrow()
    {
        int randomIndex = Random.Range(0, HigestStartingIndex + 1);

        if (randomIndex < NoPhysicsMinerales.Length) 
        {
            GameObject randomMineral = NoPhysicsMinerales[randomIndex];
            return randomMineral;
        }

        return null;
    }


    public void SiguienteMineral()
    {
        int randomIndex = Random.Range(0, HigestStartingIndex + 1);
        if (randomIndex < _mineralSprites.Length)
        {
            GameObject nextMineral = NoPhysicsMinerales[randomIndex];
        }
    }
}
