using UnityEngine;
[CreateAssetMenu(fileName = "Fix",menuName = "StatePlayer/Fix")]
public class FixState : State
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
            Repair();
        }
        else
        {
            _playerPosition = Controlleble.Agent.transform.position;
        }
    }
    private void Repair()
    {
        var broken = UseObnject.GetComponent<IBroken>();
        broken.StartRepair();
        Controlleble.Works = true;
    }
}
