using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float speed;
    
    [SerializeField] InputAction _moveAction;
    Rigidbody _rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = PlayerManager.PlayerInput.actions.FindAction("Move");
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = _moveAction.ReadValue<Vector2>();
        if (moveInput.magnitude < 0.1f) return;
        
        float targetAngle = GetAngleTowardsVectorFromCamera(moveInput);
        
        //Move based on the direction the camera is facing
        Vector3 move = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * (speed * Time.deltaTime);
        move.y = 0;

        _rb.linearVelocity += move;
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
}
