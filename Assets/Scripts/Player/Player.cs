using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class Player : SingletonBase<Player>, IControlleble
{
    private NavMeshAgent _agent;
    [Space]
    [SerializeField] private bool _isMoving;
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }
    public bool Works { get; set; }

    [SerializeField] private State _idleState;
    public State IdleState { get => _idleState; }

    [SerializeField] private State _curentState;
    public State CurentState { get => _curentState; set => _curentState = value; }

    public NavMeshAgent Agent { get => _agent; }


    protected override void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        _isMoving = true;
        SetState(IdleState, null);
    }
    public void InitState()
    {
        if (!CurentState.IsFinished)
            CurentState.Run();
        else
            SetState(IdleState, null);
    }
    public void SetState(State state, Transform useObject)
    {
        if (!Works)
        {
            NewState(state, useObject);
        }
        else
        {
            if (state == IdleState)
            {
                Works = false;
                NewState(state, useObject);
            }
        }
    }
    public void RotateToObject(Transform useObject)
    {
        transform.rotation = Quaternion.LookRotation(-useObject.forward);
    }
    private void NewState(State state, Transform useObject)
    {
        CurentState = Instantiate(state);
        CurentState.Controlleble = this;
        CurentState.PlayerPosition = transform.position;
        CurentState.TargetPosition = _agent.destination;
        CurentState.UseObnject = useObject;
        CurentState.Init();
    }
    public void SetTartgetToAgent(Vector3 point)
    {
        if (IsMoving && !Works)
            _agent.SetDestination(point);
    }

}
