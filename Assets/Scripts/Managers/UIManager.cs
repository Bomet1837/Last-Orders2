using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text timeText;
    public GameObject customerUI;

    [SerializeField] Button button;
    [SerializeField] private TextMeshProUGUI itemTextUI;
    [SerializeField] private CanvasGroup promptCanvasGroup;
    [SerializeField] Color crosshairColour;
    [SerializeField] Color shakerHoverColor;
    [SerializeField] Image crosshair;
    
    float _reach;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instance) Debug.LogError("There is multiple UIMangers, there should only be 1!");
        _reach = PlayerManager.FirstPersonController.interactRange;
        
        Cursor.lockState = CursorLockMode.Locked;
        Instance = this;
    }

    public void CloseUI(GameObject ui)
    {
        PlayerManager.LastInteractedPerson.cam.SetActive(false);
        PlayerManager.LastInteractedPerson = null;
        PlayerManager.FirstPersonController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        ui.SetActive(false);
    }

    public void ServeDrink()
    {
        GameObject objectToKill = PlayerManager.LastInteractedPerson.gameObject;
        PlayerManager.LastInteractedPerson.stool.occupied = false;
        CloseUI(customerUI);
        Destroy(objectToKill);
    }

    void Update()
    {
        CheckForObject(Camera.main.transform);
        
        if (PlayerManager.currentDrink?.Name == PlayerManager.LastInteractedPerson?.Drink.Name) button.interactable = true;
        else button.interactable = false;
    }

    void CheckForObject(Transform origin)
    {
        Ray ray = new Ray(origin.position, origin.forward);
        RaycastHit hit;
        GameObject hitObject;
        
        // Ignore NPCs when holding item
        int pickupLayer = LayerMask.NameToLayer("Pickup");
        int shakerLayer = LayerMask.NameToLayer("Shaker");
        int ignoreMask = 1 << pickupLayer | 1 << shakerLayer;
        
        Debug.DrawLine(ray.origin, origin.forward * _reach, Color.green);
        crosshair.color = crosshairColour;
        
        if (Physics.Raycast(ray, out hit, _reach, ignoreMask))
        {
            hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Shaker"))
            {
                if(PlayerManager.HeldItem == null) return;
                crosshair.color = shakerHoverColor;
            }
            
            
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
