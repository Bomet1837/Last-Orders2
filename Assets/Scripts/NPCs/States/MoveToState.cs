using UnityEngine;

public class MoveToState : NPCState
{
    Vector3 _target;
    IState _nextState;
    bool _killOnEnd;

    public MoveToState(Vector3 target, IState state)
    {
        _target = target;
        _nextState = state;
    }
    
    public MoveToState(Vector3 target, bool killOnEnd)
    {
        _target = target;
        _killOnEnd = killOnEnd;
    }
    
    public override void Enter(Person person)
    {
        if (person.animationEnabled)
        {
            person.animator.SetFloat("Speed",  1);
            person.animator.SetBool("Sitting", false);
        }
        
        person.navMeshAgent.SetDestination(_target);
        _target.y = 0;
    }

    public override void Update(Person person)
    {
        Vector3 personPosition = person.transform.position;
        personPosition.y = 0;
        
        person.RotateTowardsDestination(person.transform.position + person.navMeshAgent.velocity);
        
        if (Vector3.Distance(personPosition, _target) < 0.05f)
        {
            if (_killOnEnd)
            {
                person.kill();
                return;
            }
            person.SwitchStates(_nextState);
        }
    }
}

