using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] GameObject[] cameras;
    GameObject _previousCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance) Debug.LogError("There is more than one camera manager! there should only be 1!");

        Instance = this;
    }


    public void SwitchCameras(int index)
    {
        if(!cameras[index]) return;
        
        _previousCamera?.SetActive(false);
        _previousCamera = cameras[index];
        cameras[index].SetActive(true);
    }
    
}
