using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] InputAction _look;
    float _rotationY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _look = PlayerManager.playerInput.actions.FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = _look.ReadValue<Vector2>();

        Transform playerTransform = transform.parent;
        
        mouseDelta *= 0.1f;

        playerTransform.rotation = Quaternion.Euler(playerTransform.rotation.eulerAngles.x, playerTransform.rotation.eulerAngles.y + mouseDelta.x, playerTransform.rotation.eulerAngles.z);

        _rotationY -= mouseDelta.y;
        _rotationY = Mathf.Clamp(_rotationY, -85f, 85f);

        transform.localRotation = Quaternion.Euler(_rotationY,0,0);
    }
}
