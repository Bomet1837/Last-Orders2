using UnityEngine;

public class ShiftOver : MonoBehaviour
{

    public GameObject ShiftOverPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.Instance.currentTime.Hours >= 10)
        {
            PlayerManager.FirstPersonController.enabled = false;
            ShiftOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
