using System;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    Drink drink = new UnhappyDrink();
    
    float _time;
    bool interacted = false;

    void Update()
    {
        if (!interacted) return;
        
        _time += Time.deltaTime;
        if (_time > 2)
        {
            interacted = false;
            _time = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);
        }
    }
    public void Interact()
    {
        PlayerManager.heldDrink = drink;
        interacted = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10f);
    }
}
