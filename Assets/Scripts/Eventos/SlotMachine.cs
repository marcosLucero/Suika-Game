using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SlotMachine : MonoBehaviour
{
    public RectTransform slotContainer;
    public float spinSpeed = 500f;
    public float slowDownTime = 2f;
    public Sprite[] slotImages;
    private bool isSpinning = false;
    private int resultIndex = 0;
    private Action<int> onSpinComplete; // 📌 Callback para notificar el resultado

    public void StartSpin(Action<int> callback)
    {
        if (!isSpinning)
        {
            isSpinning = true;
            resultIndex = UnityEngine.Random.Range(0, slotImages.Length);
            onSpinComplete = callback; // 📌 Guardamos el método a llamar
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
            currentSpeed = spinSpeed * Mathf.Pow(1 - (timeElapsed / slowDownTime), 3);
            slotContainer.anchoredPosition -= new Vector2(0, currentSpeed * Time.deltaTime);
            resultIndex = UnityEngine.Random.Range(0, slotImages.Length);
            slotContainer.GetComponent<Image>().sprite = slotImages[resultIndex];

            if (slotContainer.anchoredPosition.y <= -slotContainer.sizeDelta.y / 2)
            {
                slotContainer.anchoredPosition += new Vector2(0, slotContainer.sizeDelta.y);
            }

            yield return null;
        }

        AlignToResult();
        isSpinning = false;
        onSpinComplete?.Invoke(resultIndex); // 📌 Llamamos a `TriggerEventFromSlot`
    }

    private void AlignToResult()
    {
        float targetY = -resultIndex * (slotContainer.sizeDelta.y / slotImages.Length);
        slotContainer.anchoredPosition = new Vector2(slotContainer.anchoredPosition.x, targetY);
        Debug.Log("Ruleta detenida en: " + slotImages[resultIndex].name);
    }
}
