using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowInput : MonoBehaviour
{
    TMP_Text _text;
    [SerializeField] InputActionReference input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        string inputString = input.action.GetBindingDisplayString();
        _text.SetText($"[{inputString}]");
    }
}
