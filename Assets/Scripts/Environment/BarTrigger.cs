using System;
using UnityEngine;

public class BarTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerManager.InBar = true;
    }

    void OnTriggerExit(Collider other)
    {
        PlayerManager.InBar = false;
    }
}
