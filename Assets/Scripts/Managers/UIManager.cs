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
        Instance = this;
    }

    public void CloseUI(GameObject ui)
    {
        ui.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Serve()
    {
        PlayerManager.lastInteractedPerson.Drink(PlayerManager.heldDrink);
    }
}
