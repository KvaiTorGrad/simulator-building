using TMPro;
using UnityEngine;

public class CreateSky
{
    public CreateSky(PanelAction panelAction, ActionSky action, UnityEngine.Events.UnityAction item)
    {
        var actionButton = Object.Instantiate(panelAction.ButtonAction);
        actionButton.transform.SetParent(panelAction.Panel, false);
        panelAction.AddButtons(actionButton);
        actionButton.onClick.AddListener(item);
        var text = actionButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = action.Actions.ActionText;
    }
    public CreateSky(PanelAction panelAction, ActionSky action, UnityEngine.Events.UnityAction item, string price)
    {
        var actionButton = Object.Instantiate(panelAction.ButtonAction);
        actionButton.transform.SetParent(panelAction.Panel, false);
        panelAction.AddButtons(actionButton);
        actionButton.onClick.AddListener(item);
        var text = actionButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"{action.Actions.ActionText}: {price}";
    }
}