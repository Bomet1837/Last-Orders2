using System;
using UnityEngine;

public class Button2 : MonoBehaviour, IInteractable
{
    Drink drink = new Drink();
    float _time;
    bool interacted = false;
    
    void Awake()
    {
        drink.name = "Beer";
    }
    
    void Update()
    {
        if (!interacted) return;
        
        _time += Time.deltaTime;
        if (_time > 2)
        {
            interacted = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);
            _time = 0;
        }
    }
    public void Interact()
    {
        PlayerManager.heldDrink = drink;
        interacted = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10f);
    }
}
