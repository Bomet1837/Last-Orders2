using UnityEngine;

public class ShakerLid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float checkDistance = 2f; // how close to check for shaker
    [SerializeField] private KeyCode shakeKey = KeyCode.F;
    private bool isHeld = false; // changes to true when held
    private bool canShake = false; //changes to true when in distance
    private Shaker targetShaker; 

    void Update()
    {
        if (!isHeld) return; // only check when held
        CheckForShaker(); 

        if (canShake && Input.GetKeyDown(shakeKey))
        {
            Debug.Log("[Lid] Shaking!");
            targetShaker.ShakeAndCheckCocktail(); //shaker checks cocktail
            canShake = false; //prevent multiple shakes
        }
    }

    private void CheckForShaker()
    {

        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * checkDistance, Color.yellow);


        if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
        {
            if (hit.collider.CompareTag("Shaker"))
            {
                if (!canShake)
                {
                    targetShaker = hit.collider.GetComponent<Shaker>();
                    canShake = true;
                    Debug.Log("[Lid] Hovering over shaker! Press F to shake.");
                }
                return;
            }
        }

        // Reset when no shaker is in front
        if (canShake)
        {
            Debug.Log("[Lid] Not pointing at shaker anymore.");
            canShake = false; 
            targetShaker = null;
        }
    }

    public void SetHeld(bool held)
    {
        isHeld = held; //update held state

       // if (held)
         //   Debug.Log("[Lid] Picked up lid!");
       // else
          //  Debug.Log("[Lid] Dropped lid!");
    }
}
