using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text timeText;
    public GameObject customerUI;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance) Debug.LogError("There is multiple UIMangers, there should only be 1!");
        Cursor.lockState = CursorLockMode.Locked;
        Instance = this;
    }

    public void CloseUI(GameObject ui)
    {
        ui.SetActive(false);
        PlayerManager.LastInteractedPerson.cam.SetActive(false);
        PlayerManager.CharacterController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
