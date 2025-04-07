using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameInputHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private TextMeshProUGUI confirmationText;

    private void Start()
    {
        if (nameInputField != null)
        {
            nameInputField.characterLimit = 8; // ✅ Limita a 8 caracteres
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
        string rawName = nameInputField.text.Trim();

        if (!string.IsNullOrWhiteSpace(rawName))
        {
            string formattedName = FormatName(rawName);

            Debug.Log("Nombre ingresado: " + formattedName);

            // Puedes guardar el nombre formateado en PlayerPrefs u otra lógica
            PlayerPrefs.SetString("PlayerName", formattedName);

            nameInputField.text = formattedName; // ✅ Mostrarlo en formato correcto
            nameInputField.interactable = false;
            submitButton.interactable = false;

            ShowConfirmationMessage();
        }
        else
        {
            Debug.Log("Por favor, ingresa un nombre válido.");
        }
    }

    private string FormatName(string name)
    {
        name = name.ToLower(); // Convertir todo a minúsculas
        return char.ToUpper(name[0]) + name.Substring(1); // Primera mayúscula
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
        SceneManager.LoadScene("EscenaPrincipal");
    }
}
