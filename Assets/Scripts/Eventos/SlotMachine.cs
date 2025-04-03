using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public RectTransform slotContainer; // Contenedor de las imágenes
    public float spinSpeed = 1000f; // Velocidad de giro
    public float slowDownTime = 2f; // Tiempo de frenado
    public Sprite[] slotImages; // Imágenes disponibles en la ruleta
    private bool isSpinning = false; // Estado del giro
    private int resultIndex = 0; // Índice de la imagen final

    private void Start()
    {
        ResetSlots(); // Asegurar que las imágenes estén en orden
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            resultIndex = Random.Range(0, slotImages.Length); // Elegir imagen final aleatoria
            StartCoroutine(SpinRoutine());
        }
    }

    private IEnumerator SpinRoutine()
    {
        float timeElapsed = 0f;
        float currentSpeed = spinSpeed;

        while (timeElapsed < slowDownTime)
        {
            timeElapsed += Time.deltaTime;
            currentSpeed = Mathf.Lerp(spinSpeed, 0, timeElapsed / slowDownTime); // Frenado progresivo

            slotContainer.anchoredPosition -= new Vector2(0, currentSpeed * Time.deltaTime);

            if (slotContainer.anchoredPosition.y <= -slotContainer.sizeDelta.y / 2)
            {
                slotContainer.anchoredPosition += new Vector2(0, slotContainer.sizeDelta.y);
            }

            yield return null;
        }

        // Ajustar la posición final al resultado
        AlignToResult();
        isSpinning = false;
    }

    private void AlignToResult()
    {
        float targetY = -resultIndex * (slotContainer.sizeDelta.y / slotImages.Length);
        slotContainer.anchoredPosition = new Vector2(slotContainer.anchoredPosition.x, targetY);
        Debug.Log("Se detuvo en la imagen: " + slotImages[resultIndex].name);
    }

    private void ResetSlots()
    {
        Image[] slotImagesUI = slotContainer.GetComponentsInChildren<Image>();

        for (int i = 0; i < slotImagesUI.Length; i++)
        {
            slotImagesUI[i].sprite = slotImages[i % slotImages.Length];
        }
    }
}
