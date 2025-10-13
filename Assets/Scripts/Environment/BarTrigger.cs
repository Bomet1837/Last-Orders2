using System;
using UnityEngine;

public class BarTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerManager.inBar = true;
    }

    void OnTriggerExit(Collider other)
    {
        PlayerManager.inBar = false;
    }
}
