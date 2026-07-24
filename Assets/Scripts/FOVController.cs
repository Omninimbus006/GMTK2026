using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FOVController : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera cinemachineCamera;
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        UpdateFOV();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateFOV();
    }

    private void UpdateFOV()
    {
        float newFOV = PlayerPrefs.GetFloat("FOV", 40);
        cinemachineCamera.Lens.FieldOfView = newFOV;
    }
}
