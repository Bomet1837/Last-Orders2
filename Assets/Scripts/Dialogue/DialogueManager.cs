using System.Collections.Generic;
using System.IO;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public Dictionary<string, string> dialogueDict;

    string _currentKey = "generic_skibidi_closed_0";
    readonly string _path = "Dialogue/dialogue.json";
    TMP_Text _text;
    
    void Awake()
    {
        if (Instance) Debug.LogError("There are multiple dialogue managers! there should only be 1!");
        _text = transform.GetChild(0).GetComponent<TMP_Text>();
        Instance = this;
        
        GetDialogueJson();
        _text.SetText(dialogueDict[_currentKey]);
    }

    void GetDialogueJson()
    {
        string path = Path.Combine(Application.dataPath, _path);
        string existingJson = File.ReadAllText(path);

        dialogueDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(existingJson);
    }

    string GetNextText(string currentKey)
    {
        int index = GetEndOfKey(currentKey) + 1;
        string tempKey = RemoveEndOfKey(currentKey);

        return tempKey + $"_{index}";
    }

    public void NextDialogue()
    {
        string nextText = GetNextText(_currentKey);
        
        Debug.Log(nextText);
        
        if (dialogueDict.ContainsKey(nextText))
        {
            _text.SetText(dialogueDict[nextText]);
            _currentKey = nextText;
        }
        else Debug.Log("Final text!");
    }
    
    public static int GetEndOfKey(string key)
    {
        int lastUnderscore = key.LastIndexOf("_");
        int.TryParse(key.Substring(lastUnderscore + 1), out int result);
        return result;
    }

    public static string RemoveEndOfKey(string key)
    {
        int lastUnderscore = key.LastIndexOf("_");
        return key.Substring(0, lastUnderscore);
    }

    public static string GetDialogueType(string key)
    {
        string tempKey = RemoveEndOfKey(key);
        int lastUnderscore = tempKey.LastIndexOf("_");
        return tempKey.Substring(lastUnderscore + 1);
    }
}
