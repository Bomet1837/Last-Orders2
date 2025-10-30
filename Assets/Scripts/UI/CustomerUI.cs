using System;
using UnityEngine;

public class CustomerUI : MonoBehaviour
{
    int index;

    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void ChangeCamera(int indexChange)
    {
        index = Math.Clamp(index + indexChange, 1, 99);
        CameraManager.Instance.SwitchCameras(index);
    }
}
