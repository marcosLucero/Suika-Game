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
    public AudioSource spinSound; // 🎵 Referencia al sonido

    private bool isSpinning = false;
    private int resultIndex = 0;
    private Action<int> onSpinComplete;

    private void Start()
    {
        // 🎵 Registrar el sonido en el SoundManager
        if (spinSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.RegisterAudioSource(spinSound);
            spinSound.mute = SoundManager.Instance.IsMuted(); // aplicar estado actual
        }
    }

    public void StartSpin(Action<int> callback)
    {
        if (!isSpinning)
        {
            isSpinning = true;

            // 🎵 Reproducir el sonido si está asignado
            if (spinSound != null)
            {
                spinSound.Play();
            }

            resultIndex = UnityEngine.Random.Range(0, slotImages.Length);
            onSpinComplete = callback;
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
        onSpinComplete?.Invoke(resultIndex);
    }

    private void AlignToResult()
    {
        float targetY = -resultIndex * (slotContainer.sizeDelta.y / slotImages.Length);
        slotContainer.anchoredPosition = new Vector2(slotContainer.anchoredPosition.x, targetY);
        Debug.Log("Ruleta detenida en: " + slotImages[resultIndex].name);
    }
}
