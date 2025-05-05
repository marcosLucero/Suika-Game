using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject primerCanvas;
    public GameObject botonPorSeleccionar;

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame ||
            Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            primerCanvas.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botonPorSeleccionar);
        }
    }
}
