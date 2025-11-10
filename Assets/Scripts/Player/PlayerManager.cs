using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public static PlayerInput PlayerInput;
    public static bool Grounded = false;
    public static bool InBar = false;
    public static Person LastInteractedPerson;
    public static FirstPersonCharacterController FirstPersonController;
    public static GameObject HeldItem;
    public static ICanInteract CurrentHeldInteract;
    public static Drink currentDrink;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance) Debug.LogError("Multiple player managers! there should only be one!");
        
        Instance = this;
        PlayerInput = GetComponent<PlayerInput>();
        FirstPersonController = GetComponent<FirstPersonCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
