using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCharacterController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float gravityScale;
    [SerializeField] float jumpForce;
    [SerializeField] float interactRange;
    
    [SerializeField] private Transform holdPoint;   
    [SerializeField] LayerMask interactMask;
    
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _interactAction;

    Rigidbody _heldRb;
    
    float _yVelocity;
    CharacterController _characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = PlayerManager.PlayerInput.actions.FindAction("Move");
        _jumpAction = PlayerManager.PlayerInput.actions.FindAction("Jump");
        _interactAction = PlayerManager.PlayerInput.actions.FindAction("Interact");
        
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = _moveAction.ReadValue<Vector2>();
        
        if(_interactAction.triggered) Interact();

        if (!PlayerManager.Grounded) _yVelocity += -9.81f * gravityScale * Time.deltaTime;
        else _yVelocity = 0f;
        

        if (_jumpAction.triggered && PlayerManager.Grounded)
        {
            _yVelocity = jumpForce;
            PlayerManager.Grounded = false;
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

            if (PlayerManager.CurrentHeldInteract != null)
            {
                PlayerManager.CurrentHeldInteract.Interact(hit);
                return;
            }
            
            if (PlayerManager.HeldItem == null && hit.transform.gameObject.CompareTag("Pickup"))
            {
                PickUp(hit.transform.gameObject);
            }
            else
            {
                Drop();
            }
            
            if (interactable == null) return;
            
            interactable.Interact();
        }
        else Drop();
    }
    
    private void PickUp(GameObject obj)
    {
        PlayerManager.HeldItem = obj;
        _heldRb = obj.GetComponent<Rigidbody>();

        if (_heldRb)
        {
            _heldRb.isKinematic = true;
            _heldRb.useGravity = false;
        }

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        if (obj.TryGetComponent(out ICanInteract canInteract)) PlayerManager.CurrentHeldInteract = canInteract;
        
        Debug.Log($"[Pickup] Picked up: {obj.name}");
    }
    
    public void Drop()
    {
        if (PlayerManager.HeldItem == null) return;

        if (_heldRb)
        {
            _heldRb.isKinematic = false;
            _heldRb.useGravity = true;
        }

        PlayerManager.HeldItem.layer = LayerMask.NameToLayer("Pickup");
        PlayerManager.HeldItem.transform.SetParent(null);
        Vector3 dropPos = transform.position + transform.forward * 0.75f;
        PlayerManager.HeldItem.transform.position = dropPos;

        Debug.Log($"[Pickup] Dropped: {PlayerManager.HeldItem.name}");
        
        _heldRb = null;
        PlayerManager.HeldItem = null;
        PlayerManager.CurrentHeldInteract = null;
    }
}
