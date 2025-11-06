using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        camPos.y = transform.position.y; 
        transform.LookAt(camPos);
    }
}
