using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    public int choiceIndex;
    
    public void SetChoice()
    {
        DialogueManager.Instance.SetChoice(choiceIndex);

        
        //Commit die after the choice so that new choices can be added, we need to kill ourselves last
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            GameObject choiceBox = transform.parent.GetChild(i).gameObject;
            
            if(choiceBox.GetInstanceID() == gameObject.GetInstanceID()) continue;
            
            Destroy(choiceBox);
        }
        
        Destroy(gameObject);
    }
}
