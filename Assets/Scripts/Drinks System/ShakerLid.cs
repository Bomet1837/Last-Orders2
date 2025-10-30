using UnityEngine;

public class ShakerLid : MonoBehaviour, ICanInteract
{
    public void Interact(RaycastHit hit)
    {
        Shaker targetShaker;
        if(!hit.transform.TryGetComponent(out targetShaker)) return;
        
        Debug.Log("[Lid] Shaking!");
        targetShaker.ShakeAndCheckCocktail(); //shaker checks cocktail
    }
}
