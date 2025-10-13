using System;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    void Awake()
    {
        if (Instance) Debug.LogError("There are multiple dialogue managers! there should only be 1!");
        Instance = this;
    }
}
