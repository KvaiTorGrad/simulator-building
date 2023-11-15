using UnityEngine;

public interface IItem
{
    public Transform FaceTarget { get;}
    public Collider Collider { get;}
    public void CreateSkyAction(Vector3 spawnPoint);
}
public interface IElectronics : IBroken, IInclude
{
    public ActionSky ActionAbort { get; set; }
    public bool IsWorking { get; set; }
}
public interface IGrid
{
    public GameObject GridPanel { get;}
    public abstract void AddNewGrid();
}
public interface IToSell
{
    public int Price { get;}
    public ActionSky ActionToSell { get; set; }
    public virtual void ToSell() { }
}
public interface IMoveItem
{
    public ActionSky ActionMoveItem { get;}
    public virtual void MoveItem() { }
}
public interface IBroken
{
    public int PriceBroken { get; set; }
    public float TimeRepair { get; set; }
    public bool IsBroken { get; set; }
    public bool IsRunBroken {get; set; }
    public ActionSky ActionBroken { get; set; }
    public void Broken();
    public void Fix();
    public abstract void StartRepair();
    public abstract void EndRepare();
}
public interface IInclude
{
    public bool IsInclude { get; set; }
    public ActionSky ActionInclude { get; set; }
    public void SetStateInclude() { }
    public void Include() { }
}