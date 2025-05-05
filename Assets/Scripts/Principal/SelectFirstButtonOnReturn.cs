using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectFirstButtonOnReturn : MonoBehaviour
{
    [SerializeField] private Button defaultButton;
    private const string FirstTimeKey = "AlreadyVisitedLeaderboard";

    void Start()
    {
        if (!PlayerPrefs.HasKey(FirstTimeKey))
        {
            // Es la primera vez, no seleccionamos ning�n bot�n
            PlayerPrefs.SetInt(FirstTimeKey, 1);
        }
        else
        {
            // Ya se ha visitado antes, seleccionamos el primer bot�n
            if (defaultButton != null)
            {
                EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
            }
        }
    }
}
