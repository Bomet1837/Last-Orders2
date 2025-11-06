using System.Collections.Generic;
using UnityEngine;

public class ObjectPlaceholder : MonoBehaviour
{
    [SerializeField] Material _material;
    Renderer[] _children;
    Material[] _childMaterials;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Renderer> children = new List<Renderer>();
        List<Material> childMaterials = new List<Material>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).GetComponent<Renderer>());
            
            childMaterials.Add(children[i].material);
        }

        if (transform.childCount == 0)
        {
            children.Add(GetComponent<Renderer>());
            childMaterials.Add(children[0].material);
        }

        _children = children.ToArray();
        _childMaterials = childMaterials.ToArray();
    }

    public void SetPlaceholder()
    {
        for (int i = 0; i < _children.Length; i++)
        {
            Renderer child = _children[i];

            child.material = _material;
        }
    }

    public void UnSetPlaceholder()
    {
        for (int i = 0; i < _children.Length; i++)
        {
            Renderer child = _children[i];

            child.material = _childMaterials[i];
        }
    }
}
