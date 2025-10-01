using UnityEngine;
[ExecuteInEditMode]

public class MainCameraUsesShader : MonoBehaviour
{
    public Material mat;


    void OnRenderImage (RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);
    }
}
