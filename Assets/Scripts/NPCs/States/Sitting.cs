using UnityEngine;

public class Sitting : NPCState
{
    public override void Enter(Person person)
    {
        if (person.animationEnabled)
        {
            person.animator.SetFloat("Speed", 0);
            person.animator.SetBool("Sitting", true);
        }
        person.RotateTowardsBar();
    }
}
