using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameInputHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backToMenuButton; // Nuevo bot�n para volver al men�
    [SerializeField] private TextMeshProUGUI confirmationText;

    private void Start()
    {
        if (nameInputField != null)
        {
            nameInputField.characterLimit = 6; // Limita a 9 caracteres
        }
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(SubmitName);
        }

        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(BackToMenu);
        }

        if (confirmationText != null)
        {
            confirmationText.gameObject.SetActive(false);
        }
    }

    private void SubmitName()
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            Debug.Log("Nombre ingresado: " + playerName);

            // Desactivar el campo de entrada y el bot�n de env�o
            nameInputField.interactable = false;
            submitButton.interactable = false;

            ShowConfirmationMessage();
        }
        else
        {
            Debug.Log("Por favor, ingresa un nombre v�lido.");
        }
    }


    private void ShowConfirmationMessage()
    {
        if (confirmationText != null)
        {
            confirmationText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideConfirmationMessage));
            Invoke(nameof(HideConfirmationMessage), 2f);
        }
    }

    private void HideConfirmationMessage()
    {
        if (confirmationText != null)
        {
            confirmationText.gameObject.SetActive(false);
        }
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("EscenaPrincipal"); // Aseg�rate de que el nombre de la escena es correcto
    }
}
