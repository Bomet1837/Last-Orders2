using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueCreator : EditorWindow
{
    Dictionary<string, string> _dialogue = new Dictionary<string, string>();
    string _character;
    string _tag;
    string _type;
    string _key;
    int _index;
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
        keyList.Sort((a, b) => GetEndOfKey(a).CompareTo(GetEndOfKey(b)));
        
        foreach (string key in keyList)
        {
            GUILayout.Label(key);
            
            GUILayout.BeginHorizontal();
            _dialogue[key] = GUILayout.TextArea(_dialogue[key]);
            
            if (GUILayout.Button("X",GUILayout.Width(20)))
            {
                string nextKey = RemoveEndOfKey(key) + "_" + (GetEndOfKey(key) + 1);
                if (!_dialogue.ContainsKey(nextKey))
                {
                    _dialogue.Remove(key);
                    GUILayout.EndHorizontal();
                    return;
                }
            }
            GUILayout.EndHorizontal();
            
            if (GetDialogueType(key) == "choice")
            {
                GUILayout.Label("Options", _centeredAlignment);
                
                GUILayout.BeginHorizontal();
                _dialogue[key] = GUILayout.TextArea(_dialogue[key]);
                _dialogue[key] = GUILayout.TextArea(_dialogue[key]);
                GUILayout.EndHorizontal();
            }
        }
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Dialogue"))
        {
            _dialogue.Add(FormatKey(_character,_tag,"closed",_index),"");
            _index++;
        }
        if (GUILayout.Button("Add Dialogue Choice"))
        {
            _dialogue.Add(FormatKey(_character,_tag,"choice",_index),"");
            _index++;
        }
        if (GUILayout.Button("Reset Index"))
        {
            _index = 0;
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndScrollView();
    }

    string FormatKey(string character, string tag, string type, int index)
    {
        return character + "_" + tag + "_" + type + "_" + index;
    }

    int GetEndOfKey(string key)
    {
        int lastUnderscore = key.LastIndexOf("_");
        int.TryParse(key.Substring(lastUnderscore + 1), out int result);
        return result;
    }

    string RemoveEndOfKey(string key)
    {
        int lastUnderscore = key.LastIndexOf("_");
        return key.Substring(0, lastUnderscore);
    }

    string GetDialogueType(string key)
    {
        string tempKey = RemoveEndOfKey(key);
        int lastUnderscore = tempKey.LastIndexOf("_");
        return tempKey.Substring(lastUnderscore + 1);
    }
}
