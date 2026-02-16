using UnityEngine;

public interface IState
{


    public void Enter(Person person);
    public void Update(Person person);
    public void Exit(Person person);
}
