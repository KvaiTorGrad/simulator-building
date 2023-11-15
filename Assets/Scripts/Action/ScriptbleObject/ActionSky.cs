using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkyActions")]
public class ActionSky : ScriptableObject
{
    [SerializeField] private ActionsStruct _action;
    public ActionsStruct Actions { get => _action; }
}
[Serializable]
public struct ActionsStruct
{
    [SerializeField] private string _actionText;
    [SerializeField] private State _state;
    public string ActionText { get => _actionText; }
    public State State { get => _state; }
}
