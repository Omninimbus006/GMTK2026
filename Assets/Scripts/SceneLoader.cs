using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }
    
    public void LoadSceneAsync()
    {
        SceneManager.LoadSceneAsync(SceneName);
    }
}
