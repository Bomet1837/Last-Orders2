using System;
using UnityEngine;

public class Bottle : MonoBehaviour, ICanInteract, IPickupable, IDropable
{
    [SerializeField] IngredientData ingredientData;

    public IngredientData Ingredient => ingredientData;
    public GameObject Origin { get; set; }
    public ObjectPlaceholder ObjectPlaceholder { get; set; } 
    
    void Start()
    {
        ObjectPlaceholder = GetComponent<ObjectPlaceholder>();
    }

    public void Interact(RaycastHit hit)
    {
        // Hit something tagged Shaker
        if (hit.collider.CompareTag("Shaker"))
        {
            Shaker shaker = hit.collider.GetComponentInParent<Shaker>();
            if (shaker != null)
            {
                shaker.AddIngredient(Ingredient);
                Debug.Log($"[Player] Added {Ingredient.ingredientName} to shaker!");
            }
        }
        else if (hit.transform.gameObject == Origin)
        {
            PlayerManager.FirstPersonController.Drop();
            OnDrop();
        }
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
