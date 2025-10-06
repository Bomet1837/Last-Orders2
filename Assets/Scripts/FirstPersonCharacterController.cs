using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCharacterController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float gravityScale;
    [SerializeField] float jumpForce;
    [SerializeField] float interactRange;

    [SerializeField] LayerMask interactMask;

    Drink _heldDrink;
    InputAction _moveAction;
    InputAction _jumpAction;
    private InputAction _interactAction;
    
    float _yVelocity;
    CharacterController _characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = PlayerManager.playerInput.actions.FindAction("Move");
        _jumpAction = PlayerManager.playerInput.actions.FindAction("Jump");
        _interactAction = PlayerManager.playerInput.actions.FindAction("Interact");
        
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = _moveAction.ReadValue<Vector2>();
        
        if(_interactAction.triggered) Interact();

        if (!PlayerManager.grounded) _yVelocity += -9.81f * gravityScale * Time.deltaTime;
        else _yVelocity = 0f;
        

        if (_jumpAction.triggered && PlayerManager.grounded)
        {
            _yVelocity = jumpForce;
            PlayerManager.grounded = false;
        }
        
        float targetAngle = GetAngleTowardsVectorFromCamera(moveInput);

        Vector3 move = Vector3.zero;
        
        //Move based on the direction the camera is facing
        if (moveInput.magnitude > 0.1f)
        {
            move = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * (speed * Time.deltaTime);
        }
        move.y = _yVelocity * Time.deltaTime;

        _characterController.Move(move);
    }
    
    /// <summary>
    /// Gets the angle between the characters forward direction, and the way the camera is currently facing useful for correcting movement towards camera
    /// </summary>
    public static float GetAngleTowardsVectorFromCamera(Vector2 targetVector)
    {
        if (Camera.main == null) return Mathf.Atan2(targetVector.x, targetVector.y) * Mathf.Rad2Deg;
        
        float targetAngle = Mathf.Atan2(targetVector.x, targetVector.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        return targetAngle;
    }

    void Interact()
    {
        RaycastHit hit;
        Transform origin = Camera.main.transform;

        if (Physics.Raycast(origin.position, origin.forward,out hit, interactRange, interactMask))
        {
            Debug.Log($"hit{hit.transform.gameObject.name}");
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable == null) return;
            
            interactable.Interact();
        }
    }
}
