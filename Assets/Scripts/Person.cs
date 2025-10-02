using System;
using UnityEngine;

public abstract class Person : MonoBehaviour
{
    public virtual void Drink<T>(T drink) where T : Drink
    {
        Debug.Log("Mmm Delicious!");
    }
}