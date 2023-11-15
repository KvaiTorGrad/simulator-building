using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelAction : SingletonBase<PanelAction>
{
    [SerializeField] private Button _buttonAction;
    public Button ButtonAction { get => _buttonAction; }

    [SerializeField] private Transform _panel;
    public Transform Panel { get => _panel; }

    private List<Button> _button = new List<Button>();

    public void AddButtons(Button button)
    {
        _button.Add(button);
    }
    public void DestroyButtons()
    {
        foreach (Button button in _button)
            Destroy(button.gameObject);
        _button.Clear();

    }
}
