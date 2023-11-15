using UnityEngine;
using UnityEngine.AI;

public interface IControlleble : IMove, IState, StateIdle
{

}
public interface IState
{
    public State CurentState { get; set; }
    public bool Works { get; set; }
    public void InitState();
    public void SetState(State state, Transform useObject);
}

public interface IMove
{
    public bool IsMoving { get; set; }
    public NavMeshAgent Agent { get;}
    public void SetTartgetToAgent(Vector3 point);
    public void RotateToObject(Transform useObject);
}

public interface StateIdle
{
    public State IdleState { get;}
}
public interface StateFix
{
    public State FixState { get; }
}