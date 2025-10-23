using System.Collections.Generic;
using System.IO;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public Dictionary<string, string> dialogueDict;
    public CharacterScript currentCharacterScript;
    
    int _index;
    string _currentKey = "generic_skibidi_closed_0";
    readonly string _path = "Dialogue/dialogue.json";
    TMP_Text _dialogueText;
    TMP_Text _characterSpeaking;
    
    void Awake()
    {
        if (Instance) Debug.LogError("There are multiple dialogue managers! there should only be 1!");
        Instance = this;
        
        _dialogueText = transform.GetChild(0).GetComponent<TMP_Text>();
        _characterSpeaking = transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        
        GetDialogueJson();
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
            _dialogueText.SetText(dialogueDict[nextText]);
            _currentKey = nextText;
        }
        else Debug.Log("Final text!");
    }

    public void NextDialogueInScript()
    {
        if (_index + 1 == currentCharacterScript.dialogueKeys.Length)
        {
            if (currentCharacterScript.dialogueOptionScripts.Length != 1) return;
            
            currentCharacterScript = currentCharacterScript.dialogueOptionScripts[0];
            ShowText();
            return;
        }
        _index++;

        string key = currentCharacterScript.dialogueKeys[_index];

        if (GetDialogueType(key) == "choice")
        {
            GenerateChoices(dialogueDict[key]);
            return;
        }
        _characterSpeaking.SetText(GetCharacterFromKey(key));
        _dialogueText.SetText(dialogueDict[key]);
    }

    void GenerateChoices(string choice)
    {
        GameObject choiceBox1 = transform.GetChild(3).GetChild(0).gameObject;
        GameObject choiceBox2 = transform.GetChild(3).GetChild(1).gameObject;
        
        choiceBox1.transform.parent.gameObject.SetActive(true);
        
        TMP_Text choiceBox1Text = choiceBox1.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text choiceBox2Text = choiceBox2.transform.GetChild(0).GetComponent<TMP_Text>();

        string[] choices = choice.Split("|");
        
        choiceBox1Text.SetText(formatChoiceText(choices[0]));
        choiceBox2Text.SetText(formatChoiceText(choices[1]));
    }

    public void SetChoice(int index)
    {
        currentCharacterScript = currentCharacterScript.dialogueOptionScripts[index];
        _index = 0;
        
        ShowText();
    }

    public string formatChoiceText(string text)
    {
        return text.Split("<")[0];
    }

    public void ShowText()
    {
        string key = currentCharacterScript.dialogueKeys[_index];
        if(!dialogueDict.ContainsKey(key)) return;
        
        _characterSpeaking.SetText(GetCharacterFromKey(key));
        _dialogueText.SetText(dialogueDict[key]);
    }
    
    public static int GetEndOfKey(string key)
    {
        int lastUnderscore = key.LastIndexOf("_");
        int.TryParse(key.Substring(lastUnderscore + 1), out int result);
        return result;
    }

    public static string GetCharacterFromKey(string key)
    {
        int firstUnderscore = key.IndexOf("_");
        string characterName = key.Substring(0, firstUnderscore);
        string capitalizedName = char.ToUpper(characterName[0]) + characterName.Substring(1);
        return capitalizedName;
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
