using TMPro;
using UnityEngine;

public class SetTextToDrink : MonoBehaviour
{

    TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.SetText($"Serve {PlayerManager.heldDrink.name}");
    }
}
