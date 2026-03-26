using System;
using TMPro;
using UnityEngine;

public class ShakerText : MonoBehaviour
{

    float _time = 100f;
    TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_time < 1)
        {
            _time += Time.deltaTime;

            _text.enabled = true;
        }
        else _text.enabled = false;
    }

    public void Show() => _time = 0;
}
