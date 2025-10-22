using System;
using Cinemachine;
using TMPro;
using UnityEngine;

public class Person : MonoBehaviour, IInteractable
{
    public GameObject cam;

    void Awake()
    {
        cam = transform.GetChild(0).gameObject;
    }

    public virtual void Drink<T>(T drink) where T : Drink
    {
        
    }

    void Update()
    {

        
    }

    public void Interact()
    {
        if(!PlayerManager.inBar) return;
        
        PlayerManager.lastInteractedPerson = this;
        PlayerManager.characterController.enabled = false;
        cam.SetActive(true);
        UIManager.Instance.customerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}