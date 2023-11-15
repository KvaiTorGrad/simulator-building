using UnityEngine;
using UnityEngine.AI;

public abstract class State : ScriptableObject
{
    public bool IsFinished { get; protected set; }
    public IControlleble Controlleble;
    public Vector3 PlayerPosition { get;  set; }
    public Vector3 TargetPosition { get; set; }
    public Transform UseObnject {  get; set; }
    public virtual void Init() { }
    public abstract void Run();
}
