using UnityEngine;

public class DrinkObject : MonoBehaviour, IPickupable, IDropable, ICanInteract
{
    public GameObject Origin { get; set; }
    public ObjectPlaceholder ObjectPlaceholder { get; set; }
    public string contains = "Empty";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectPlaceholder = GetComponent<ObjectPlaceholder>();
    }
    
    public void OnPickup()
    {
        Destroy(gameObject);
    }
    
    public void OnDrop()
    {
        Origin.GetComponent<ObjectPlaceholder>().UnSetPlaceholder();
    }
    
    public void Interact(RaycastHit hit)
    {
        GameObject hitObject = hit.transform.gameObject;
        
        if (hitObject.layer == LayerMask.NameToLayer("TableDrink"))
        {
            PlayerManager.FirstPersonController.Drop();
            transform.position = hitObject.transform.position;
            transform.rotation = Quaternion.Euler(0,0,0);
            hitObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
