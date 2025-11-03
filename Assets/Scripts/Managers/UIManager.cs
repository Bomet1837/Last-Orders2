using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text timeText;
    public GameObject customerUI;
    
    [SerializeField] private TextMeshProUGUI itemTextUI;
    [SerializeField] private CanvasGroup promptCanvasGroup; 
    
    float _reach;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance) Debug.LogError("There is multiple UIMangers, there should only be 1!");
        _reach = PlayerManager.CharacterController.interactRange;
        
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

    void Update()
    {
        CheckForObject(Camera.main.transform);
    }

    void CheckForObject(Transform origin)
    {
        Ray ray = new Ray(origin.position, origin.forward);
        RaycastHit hit;
        GameObject hitObject;
        
        // Ignore NPCs when holding item
        int pickupLayer = LayerMask.NameToLayer("Pickup");
        int ignoreMask = 1 << pickupLayer;
        
        Debug.DrawLine(ray.origin, origin.forward * _reach, Color.green);
        
        if (Physics.Raycast(ray, out hit, _reach, ignoreMask))
        {
            hitObject = hit.collider.gameObject;

            // Make sure the UI is visible
            if (promptCanvasGroup != null)
                promptCanvasGroup.alpha = 1;

            // Update the prompt text in real-time
            if (itemTextUI != null)
                itemTextUI.text = $"{hitObject.name}";

            return;
        }
        
        if (promptCanvasGroup != null) promptCanvasGroup.alpha = 0;
    }
}
