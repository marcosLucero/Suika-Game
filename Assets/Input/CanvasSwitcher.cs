using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // Solo si usas el nuevo Input System

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject primerCanvas;
    public GameObject botonPorSeleccionar; // botón del segundo canvas

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            primerCanvas.SetActive(false);

            // ✳️ Forzar selección tras ocultar el primer canvas
            EventSystem.current.SetSelectedGameObject(null); // limpiar selección anterior
            EventSystem.current.SetSelectedGameObject(botonPorSeleccionar);
        }
    }
}
