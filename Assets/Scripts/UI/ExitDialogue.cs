using UnityEngine;

public class ExitDialogue : MonoBehaviour
{

    public void Exit()
    {
        foreach (Person person in DialogueManager.Instance.Characters.Values)
        {
            person.cam.SetActive(false);
        }
    }
}
