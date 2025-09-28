using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerManager.grounded = true;
    }

    void OnTriggerStay(Collider other)
    {
        PlayerManager.grounded = true;
    }

    void OnTriggerExit(Collider other)
    {
        PlayerManager.grounded = false;
    }

    float GetNormalAngle(Terrain terrain)
    {
        Vector3 terrainPos = terrain.transform.position;
        Vector3 position = transform.position;
        
        TerrainData terrainData = terrain.terrainData;
        
        // Convert world position to normalized terrain coordinates
        float normX = (position.x - terrainPos.x) / terrainData.size.x;
        float normZ = (position.z - terrainPos.z) / terrainData.size.z;

        Vector3 normal = terrainData.GetInterpolatedNormal(normX, normZ);

        return Vector3.Angle(normal,Vector3.up);
    }
}
