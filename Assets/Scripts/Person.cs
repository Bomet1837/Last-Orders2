using System;
using UnityEngine;

public abstract class Person : MonoBehaviour, IInteractable
{
    public virtual void Drink<T>(T drink) where T : Drink
    {
        Debug.Log("Mmm Delicious!");
    }

    public void Interact()
    {
        Debug.Log("Hello");
    }
}