using UnityEngine;
using TMPro;

public class AutoScrollText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float scrollSpeed = 50f; // Velocidad de desplazamiento

    private string fullText;
    private float textWidth;
    private float timer = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        fullText = textComponent.text; // Guarda el texto original
        textWidth = textComponent.preferredWidth; // Ancho del texto
        originalPosition = textComponent.rectTransform.localPosition; // Guarda la posición original
    }

    void Update()
    {
        timer += Time.deltaTime * scrollSpeed;
        float newX = Mathf.PingPong(timer, textWidth) - textWidth / 2;
        textComponent.rectTransform.localPosition = new Vector3(originalPosition.x - newX, originalPosition.y, originalPosition.z);
    }
}
