using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "RandomizedDialogue", menuName = "Scriptable Objects/RandomizedDialogue")]
public class RandomizedDialogue : CharacterScript
{
    void Awake()
    {
        if(dialogueOptionScripts.Length > 0) Debug.LogError("There is dialogue option on a randomized script, there should never be this!");
    }
    public override string GetKey(int index)
    {
        //Generate random from our keys
        return dialogueKeys[Mathf.RoundToInt(Random.Range(0, dialogueKeys.Length - 1))];
    }
}
