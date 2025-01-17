using UnityEngine;
using UnityEngine.SceneManagement;

public class JugarSolo : MonoBehaviour
{
    public void LoadGameScene(string escena)
    {
        SceneManager.LoadScene(escena);
    }
}
