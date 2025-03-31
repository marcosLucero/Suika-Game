using UnityEngine;

public class DatabaseToggle : MonoBehaviour
{
    public GameObject database1; // Primer GameObject con la base de datos
    public GameObject database2; // Segundo GameObject con la otra base de datos

    private void Start()
    {
        // Aseguramos que database1 esté activo y database2 desactivado al inicio
        database1.SetActive(true);
        database2.SetActive(false);
    }

    public void ToggleDatabase()
    {
        bool isDatabase1Active = database1.activeSelf;

        database1.SetActive(!isDatabase1Active);
        database2.SetActive(isDatabase1Active);

        Debug.Log(isDatabase1Active ? "Cambiado a Base de Datos 2" : "Cambiado a Base de Datos 1");
    }
}
