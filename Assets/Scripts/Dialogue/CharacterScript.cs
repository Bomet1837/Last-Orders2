using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScript", menuName = "Scriptable Objects/CharacterScript")]
public class CharacterScript : ScriptableObject
{
    public string[] dialogueKeys;
    public CharacterScript[] dialogueOptionScripts;
}
