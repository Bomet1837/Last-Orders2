using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text timeText;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance) Debug.LogError("There is multiple UIMangers, there should only be 1!");
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
