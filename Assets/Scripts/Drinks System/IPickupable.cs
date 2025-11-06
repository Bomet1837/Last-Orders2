using UnityEngine;

public interface IPickupable
{
    void OnPickup();
    public GameObject Origin { get; set; }
    public ObjectPlaceholder ObjectPlaceholder { get; set; }
}
