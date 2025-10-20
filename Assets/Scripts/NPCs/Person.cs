using System;
using Cinemachine;
using TMPro;
using UnityEngine;

public abstract class Person : MonoBehaviour, IInteractable
{
    GameObject _cam;
    TMP_Text _text;
    float _time = 10;

    void Awake()
    {
        _cam = transform.GetChild(0).gameObject;
    }

    public virtual void Drink<T>(T drink) where T : Drink
    {
        SetText("Mmm delicious!");
    }

    public void SetText(string text)
    {
        _text.SetText(text);
        _time = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time > 2)
        {
            _text.SetText("");
            _time = 0;
        }
    }

    public void Interact()
    {
        if(!PlayerManager.inBar) return;
        
        PlayerManager.lastInteractedPerson = this;
        _cam.SetActive(true);
        UIManager.Instance.customerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}