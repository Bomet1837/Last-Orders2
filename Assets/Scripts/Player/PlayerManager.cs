using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public static PlayerInput playerInput;
    public static bool grounded = false;
    public static bool inBar = false;
    public static Drink heldDrink = new Drink();
    public static Person lastInteractedPerson;
    public static FirstPersonCharacterController characterController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        heldDrink.name = "Beer";
        if(instance) Debug.LogError("Multiple player managers! there should only be one!");
        
        instance = this;
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<FirstPersonCharacterController>();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
