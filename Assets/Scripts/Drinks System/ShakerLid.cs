using UnityEngine;

public class ShakerLid : MonoBehaviour, ICanInteract, IPickupable, IDropable
{
    public GameObject Origin { get; set; }
    public ObjectPlaceholder ObjectPlaceholder { get; set; }

    void Start()
    {
        ObjectPlaceholder = GetComponent<ObjectPlaceholder>();
    }
    
    public void Interact(RaycastHit hit)
    {
        Shaker targetShaker;
        
        if (hit.transform.gameObject == Origin)
        {
            PlayerManager.CharacterController.Drop();
            OnDrop();
        }
        
        if(!hit.transform.TryGetComponent(out targetShaker)) return;
        
        Debug.Log("[Lid] Shaking!");
        targetShaker.ShakeAndCheckCocktail(); //shaker checks cocktail
    }
    
    public void OnPickup()
    {
        ObjectPlaceholder.SetPlaceholder();
    }

    public void OnDrop()
    {
        Origin.GetComponent<ObjectPlaceholder>().UnSetPlaceholder();
        Destroy(gameObject);
    }
}
