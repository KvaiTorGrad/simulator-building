using ItemSetting;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    private Dictionary<int, Item> _items = new();
    public Dictionary<int, Item> Items { get => _items; }
    private Dictionary<int, IElectronics> _electronics = new();
    public Dictionary<int, IElectronics> Electronics { get => _electronics; }

    private Dictionary<int, IBroken> _broken = new();
    public Dictionary<int, IBroken> Broken { get => _broken; }

    private List<GameObject> _gridVisualizations = new();
    public List<GameObject> GridVisualizations { get => _gridVisualizations; }

    /// <summary>
    /// Для тестов
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RepeareAll();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (var elec in Electronics.Values)
                elec.Broken();
        }
    }
    private void RepeareAll()
    {
        foreach (var broken in _broken.Values)
            broken.StartRepair();
    }
}
