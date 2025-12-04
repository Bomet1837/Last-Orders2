using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // keep vertical rotation locked

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
