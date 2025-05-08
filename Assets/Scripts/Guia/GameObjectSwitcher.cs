using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour
{
    public GameObject[] objectsToToggle;
    private int currentIndex = 0;

    private void Start()
    {
        UpdateActiveObject();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            Next();
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            Previous();
        }
    }

    public void Next()
    {
        if (objectsToToggle.Length == 0) return;

        objectsToToggle[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % objectsToToggle.Length;
        objectsToToggle[currentIndex].SetActive(true);
    }

    public void Previous()
    {
        if (objectsToToggle.Length == 0) return;

        objectsToToggle[currentIndex].SetActive(false);
        currentIndex = (currentIndex - 1 + objectsToToggle.Length) % objectsToToggle.Length;
        objectsToToggle[currentIndex].SetActive(true);
    }

    private void UpdateActiveObject()
    {
        for (int i = 0; i < objectsToToggle.Length; i++)
        {
            objectsToToggle[i].SetActive(i == currentIndex);
        }
    }
}
