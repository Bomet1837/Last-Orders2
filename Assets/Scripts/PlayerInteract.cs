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

    private GameObject heldObject;     // Currently held object
    private Rigidbody heldRb;          // Rigidbody of held object
    private GameObject targetObject;   // Object currently in reach
    private Coroutine clearTargetRoutine;



    void Start()
    {
        
        if ( itemTextUI== null)
            itemTextUI = GameObject.Find("PickupPrompt")?.GetComponent<TextMeshProUGUI>();

        if (promptCanvasGroup == null && itemTextUI != null)
            promptCanvasGroup = itemTextUI.GetComponent<CanvasGroup>();

        if (promptCanvasGroup != null)
            promptCanvasGroup.alpha = 0; // start hidden
    }

    void Update()
    {
        CheckForObject();

        // Handle pickup/drop
        //might add place functionality later
        if (Input.GetKeyDown(interactKey))
        {
            if (heldObject == null && targetObject != null)
            {
                PickUp(targetObject);
            }
            else if (heldObject != null)
            {
                Drop();
            }
        }

        // Only bottles can add to shaker
        if (heldObject != null && heldObject.GetComponent<Bottle>() && Input.GetKeyDown(KeyCode.F))
        {
            TryAddIngredientToShaker();
        }

        // If holding a lid, allow shaking when looking at the shaker
        if (heldObject != null && heldObject.GetComponent<ShakerLid>() != null && Input.GetKeyDown(KeyCode.F))
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


        if (Physics.Raycast(ray, out hit, reachDistance, ignoreMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(pickupTag))
            {
                targetObject = hitObject;

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
        if (targetObject != null)
        {
            targetObject = null;
            if (promptCanvasGroup != null)
                promptCanvasGroup.alpha = 0;
        }
    }

    private IEnumerator ClearTargetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetObject != null && heldObject == null)
        {
            Debug.Log("Nothing in reach");
            targetObject = null;
        }
        clearTargetRoutine = null;
    }
    
    private void PickUp(GameObject obj) 
    {
        heldObject = obj;
        heldRb = obj.GetComponent<Rigidbody>();

        if (heldRb)
        {
            heldRb.isKinematic = true;
            heldRb.useGravity = false;
        }

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        //  Let the object know it's being held
        if (obj.TryGetComponent(out Bottle bottle))
            bottle.GetComponent<Bottle>();

        if (obj.TryGetComponent(out ShakerLid lid))
            lid.SetHeld(true);

        Debug.Log($"[Pickup] Picked up: {obj.name}");
    }

    private void Drop()
    {
        if (heldObject == null) return;

        // Tell object it's no longer held
        if (heldObject.TryGetComponent(out ShakerLid lid))
            lid.SetHeld(false);

        if (heldRb)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
        }

        heldObject.layer = LayerMask.NameToLayer("Pickup");
        heldObject.transform.SetParent(null);
        Vector3 dropPos = transform.position + transform.forward * 0.75f;
        heldObject.transform.position = dropPos;

        Debug.Log($"[Pickup] Dropped: {heldObject.name}");

        heldObject = null;
        heldRb = null;
    }

    private void TryAddIngredientToShaker()
    {
        // Only try if you're holding a bottle
        if (heldObject == null) return;

        Bottle heldBottle = heldObject.GetComponent<Bottle>();
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
                   /// Drop();
                }
            }
        }
    }

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
