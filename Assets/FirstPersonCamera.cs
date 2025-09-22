using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    PlayerInput _playerInput;
    InputAction _look;
    float _rotationY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _look = PlayerManager.PlayerInput.actions.FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = _look.ReadValue<Vector2>();
        Camera camera = Camera.main;
        mouseDelta *= 0.1f;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseDelta.x, transform.rotation.eulerAngles.z);

        _rotationY -= mouseDelta.y;
        _rotationY = Mathf.Clamp(_rotationY, -85f, 85f);

        camera.transform.localRotation = Quaternion.Euler(_rotationY,0,0);
    }
}
