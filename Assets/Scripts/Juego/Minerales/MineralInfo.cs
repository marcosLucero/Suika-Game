using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralInfo : MonoBehaviour
{
    public int MineralIndex = 0;
    public int PuntosCuandoAniquilados = 1;
    public float MineralMassa = 1f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.mass = MineralMassa;
    }
}
