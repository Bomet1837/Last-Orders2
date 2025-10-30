using UnityEngine;

public class Bottle : MonoBehaviour, ICanInteract
{
    [SerializeField] private IngredientData ingredientData;

    public IngredientData Ingredient => ingredientData;


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
        else PlayerManager.CharacterController.Drop();
    }
}
