using UnityEngine;
[CreateAssetMenu(fileName = "Include", menuName = "StatePlayer/Include")]
public class IncludeState : State
{
    private Vector3 _playerPosition;

    public override void Init()
    {
        _playerPosition = Controlleble.Agent.transform.position;
    }
    public override void Run()
    {
        if (IsFinished) return;
        MoveToItem();
    }

    private void MoveToItem()
    {
        var distance = Vector3.Distance(Controlleble.Agent.destination, _playerPosition);
        if (distance < 1.1f)
        {
            Controlleble.RotateToObject(UseObnject);
            Include();
        }
        else
        {
            _playerPosition = Controlleble.Agent.transform.position;
        }
    }
    private void Include()
    {
        var include = UseObnject.GetComponent<IInclude>();
        include.Include();
        Controlleble.Works = true;
    }
}
