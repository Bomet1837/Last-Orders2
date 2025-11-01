using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float reachDistance = 3f;        // How far you can reach
    [SerializeField] private Transform holdPoint;             // Where the held object sits
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key for pickup/drop
    [SerializeField] private string pickupTag = "Pickup";     // Tag for pickup items
    [SerializeField] private float dropForwardDistance = 0.75f; // How far in front of player to drop
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI itemTextUI;          // UI for item prompts
    [SerializeField] private CanvasGroup promptCanvasGroup;   // controls visibility of prompt UI

    private GameObject _heldObject;     // Currently held object
    private Rigidbody _heldRb;          // Rigidbody of held object
    private GameObject _targetObject;   // Object currently in reach



    void Start()
    {
        itemTextUI = GameObject.Find("PickupPrompt")?.GetComponent<TextMeshProUGUI>();
        
        promptCanvasGroup = itemTextUI.GetComponent<CanvasGroup>();
        
        promptCanvasGroup.alpha = 0; // start hidden
    }

    void Update()
    {
        CheckForObject();

        // Handle pickup/drop
        // Might add place functionality later
        // Move to base interaction on player character maybe export player interaction to other script
        if (Input.GetKeyDown(interactKey))
        {
            if (_heldObject == null && _targetObject != null)
            {
                PickUp(_targetObject);
            }
            else if (_heldObject != null)
            {
                Drop();
            }
        }

        // Only bottles can add to shaker
        // Move to bottle
        if (_heldObject != null && _heldObject.GetComponent<Bottle>() && Input.GetKeyDown(KeyCode.F))
        {
            TryAddIngredientToShaker();
        }

        // If holding a lid, allow shaking when looking at the shaker
        // Move to Shader Lid
        if (_heldObject != null && _heldObject.GetComponent<ShakerLid>() != null && Input.GetKeyDown(KeyCode.F))
        {
            TryShakeShaker();
        }
    }

    void CheckForObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        // Ignore NPCs when holding item
        int npcLayer = LayerMask.NameToLayer("NPC");
        int heldItemLayer = LayerMask.NameToLayer("HeldItem"); //ray gets blocked by held item
        int ignoreMask = ~(1 << npcLayer | 1 << heldItemLayer); // inverts the mask so it hits everything except NPCs


        //Move to UI Manager, use a layer instead
        if (Physics.Raycast(ray, out hit, reachDistance, ignoreMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(pickupTag))
            {
                _targetObject = hitObject;

                // Make sure the UI is visible
                if (promptCanvasGroup != null)
                    promptCanvasGroup.alpha = 1;

                // Update the prompt text in real-time
                if (itemTextUI != null)
                    itemTextUI.text = $"{hitObject.name}";

                return;
            }
        }

        // if no pick up no text
        if (_targetObject != null)
        {
            _targetObject = null;
            if (promptCanvasGroup != null)
                promptCanvasGroup.alpha = 0;
        }
    }
    
    private void PickUp(GameObject obj) 
    {
        _heldObject = obj;
        _heldRb = obj.GetComponent<Rigidbody>();

        if (_heldRb)
        {
            _heldRb.isKinematic = true;
            _heldRb.useGravity = false;
        }

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        //  Let the object know it's being held
        if (obj.TryGetComponent(out Bottle bottle))
            bottle.GetComponent<Bottle>();

        Debug.Log($"[Pickup] Picked up: {obj.name}");
    }

    private void Drop()
    {
        if (_heldObject == null) return;

        if (_heldRb)
        {
            _heldRb.isKinematic = false;
            _heldRb.useGravity = true;
        }

        _heldObject.layer = LayerMask.NameToLayer("Pickup");
        _heldObject.transform.SetParent(null);
        Vector3 dropPos = transform.position + transform.forward * 0.75f;
        _heldObject.transform.position = dropPos;

        Debug.Log($"[Pickup] Dropped: {_heldObject.name}");

        _heldObject = null;
        _heldRb = null;
    }

    
    //Move this whole thing to bottle interact method
    private void TryAddIngredientToShaker()
    {
        // Only try if you're holding a bottle
        if (_heldObject == null) return;

        Bottle heldBottle = _heldObject.GetComponent<Bottle>();
        if (heldBottle == null) return; // only bottles can add ingredients

        Ray ray = new Ray(transform.position + transform.forward * 0.3f, transform.forward);
        int npcLayer = LayerMask.NameToLayer("NPC");
        int heldItemLayer = LayerMask.NameToLayer("HeldItem");
        int ignoreMask = ~(1 << npcLayer | 1 << heldItemLayer);

        if (Physics.Raycast(ray, out RaycastHit hit, 2f, ignoreMask))
        {
            // Hit something tagged Shaker
            if (hit.collider.CompareTag("Shaker"))
            {
                Shaker shaker = hit.collider.GetComponentInParent<Shaker>();
                if (shaker != null)
                {
                    shaker.AddIngredient(heldBottle.Ingredient);
                    Debug.Log($"[Player] Added {heldBottle.Ingredient.ingredientName} to shaker!");
                   // Drop();
                }
            }
        }
    }

    // Move to Lid
    // Shake shaker if holding lid
    private void TryShakeShaker()
    {
        Ray ray = new Ray(transform.position + transform.forward * 0.3f, transform.forward);

        int npcLayer = LayerMask.NameToLayer("NPC");
        
        int heldItemLayer = LayerMask.NameToLayer("HeldItem");

        int ignoreMask = ~(1 << npcLayer | 1 << heldItemLayer);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f,ignoreMask))
        {
            if (hit.collider.CompareTag("Shaker"))
            {
                Shaker shaker = hit.collider.GetComponentInParent<Shaker>();
                if (shaker != null)
                {
                    Debug.Log("[Player] Shaking shaker...");
                    shaker.ShakeAndCheckCocktail();
                }
            }
        }
    }
}
