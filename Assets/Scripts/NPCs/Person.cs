using System;
using Cinemachine;
using TMPro;
using UnityEngine;

public class Person : MonoBehaviour, IInteractable
{
    public GameObject cam;
    public CharacterScript dialogue;

    void Awake()
    {
        cam = transform.GetChild(0).gameObject;
    }

    void Update()
    {

        
    }

    public void Interact()
    {
        if(!PlayerManager.InBar) return;
        
        PlayerManager.LastInteractedPerson = this;
        PlayerManager.CharacterController.enabled = false;

        DialogueManager.Instance.currentCharacterScript = dialogue;
        DialogueManager.Instance.ShowText();
        
        cam.SetActive(true);
        UIManager.Instance.customerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}