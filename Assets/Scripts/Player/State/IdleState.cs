using UnityEngine;

[CreateAssetMenu (fileName = "Idle", menuName = "StatePlayer/Idle")]
public class IdleState : State
{
    public override void Run()
    {
        if (IsFinished) return;
        Idle();
    }

    private void Idle()
    {
        IsFinished = true;
    }
}
