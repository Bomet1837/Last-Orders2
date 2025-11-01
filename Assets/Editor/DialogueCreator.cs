using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class DialogueCreator : EditorWindow
{
    readonly string _path = "Dialogue/dialogue.json";
    
    Dictionary<string, string> _dialogue = new Dictionary<string, string>();
    Dictionary<string, string> _outputDict = new Dictionary<string, string>();
    string _character;
    string _tag;
    int _index = 0;
    GUIStyle _centeredAlignment;
    Vector2 scrollPos;
    
    [MenuItem("Tools/Dialogue Creator")]
    public static void ShowWindow()
    {
        GetWindow<DialogueCreator>("Dialogue Creator");
    }
    
    void OnGUI()
    {
        _centeredAlignment = new GUIStyle(EditorStyles.label);
        _centeredAlignment.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Dialogue Creator", _centeredAlignment);

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        
        GUILayout.BeginHorizontal();
        
        GUILayout.Label("Character");
        _character = GUILayout.TextField(_character, GUILayout.Width(80));
        
        GUILayout.Label("tag");
        _tag = GUILayout.TextField(_tag, GUILayout.Width(80));
        
        GUILayout.EndHorizontal();

        List<string> keyList = new List<string>(_dialogue.Keys);
        
        foreach (string key in keyList)
        {
            GUILayout.Label(key);
            
            GUILayout.BeginHorizontal();
            _dialogue[key] = GUILayout.TextArea(_dialogue[key]);
            _outputDict[key] = _dialogue[key];
            
            if (DialogueManager.GetDialogueType(key) == "choice")
            {
                _outputDict[key] = _dialogue[key];
            }
            
            if (GUILayout.Button("X",GUILayout.Width(20)))
            {
                string nextKey = DialogueManager.RemoveEndOfKey(key) + "_" + (DialogueManager.GetEndOfKey(key) + 1);
                if (!_dialogue.ContainsKey(nextKey))
                {
                    GUILayout.EndHorizontal();
                    _dialogue.Remove(key);
                    _outputDict.Remove(key);
                    _index--;
                    Math.Clamp(_index, 0, 99999);
                    continue;
                }
            }
            
            GUILayout.EndHorizontal();
        }
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Dialogue"))
        {
            string key = FormatKey(_character, _tag, "closed", _index);
            
            if(_dialogue.ContainsKey(key)) key = GetNextDialogueKey(key);
            _dialogue.Add(key, "");
            _outputDict.Add(key, "");
            _index++;
        }
        if (GUILayout.Button("Add Dialogue Choice"))
        {
            string key = FormatKey(_character, _tag, "choice", _index);

            if (_dialogue.ContainsKey(key)) key = GetNextDialogueKey(key);
            _dialogue.Add(key,"");
            _outputDict.Add(key,"");
            _index++;
        }
        if (GUILayout.Button("Clear"))
        {
            _index = 0;
            _dialogue = new Dictionary<string, string>();
            _outputDict = new Dictionary<string, string>();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save as JSON"))
        {
            Debug.Log(JsonConvert.SerializeObject(_outputDict, Formatting.Indented));
            AppendToJson();
        } 
        
        EditorGUILayout.EndScrollView();
    }

    string FormatKey(string character, string tag, string type, int index)
    {
        return character + "_" + tag + "_" + type + "_" + index;
    }

    string GetNextDialogueKey(string keyToAdd)
    {
        bool foundLastDialogue = false;
        int index = DialogueManager.GetEndOfKey(keyToAdd);
        int count = 0;
        string key = DialogueManager.RemoveEndOfKey(keyToAdd);
        string nextKey = keyToAdd;
        
        while (!foundLastDialogue)
        {
            nextKey = key + "_" + index;
            foundLastDialogue = !_dialogue.ContainsKey(nextKey);
            count++;
            index++;
            if (count > 100)
            {
                Debug.LogError($"Can't find last dialogue! {nextKey}");
                return "";
            }
        }

        return nextKey;
    }

    void AppendToJson()
    {
        string path = Path.Combine(Application.dataPath, _path);
        string existingJson = File.ReadAllText(path);
        
        Dictionary<string,string> json = JsonConvert.DeserializeObject<Dictionary<string, string>>(existingJson) ?? new Dictionary<string, string>();

        foreach (KeyValuePair<string,string> kvp in _outputDict)
        {
            json.Add(kvp.Key,kvp.Value);
        }

        string updatedJson = JsonConvert.SerializeObject(json, Formatting.Indented);
        
        File.WriteAllText(path,updatedJson);
        
        AssetDatabase.Refresh();
    }
}
