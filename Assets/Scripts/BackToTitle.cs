using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{

    public void OpenTitleScreen()
    {
        SceneManager.LoadScene(0);
    }
}
