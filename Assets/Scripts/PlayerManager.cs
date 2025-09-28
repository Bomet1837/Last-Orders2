using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public static PlayerInput playerInput;
    public static bool grounded = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance) Debug.LogError("Multiple player managers! there should only be one!");
        
        instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
