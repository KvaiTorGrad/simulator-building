using BuildingItemsOnGrid;
using ControllerSystem;
using System.Collections;
using UnityEngine;
namespace ItemSetting.Electronic
{
    public class Washer : Item, IElectronics
    {
        private ActionSky _actionBroken;
        public ActionSky ActionBroken { get => _actionBroken; set => _actionBroken = value; }
        private ActionSky _actionAbort;
        public ActionSky ActionAbort { get => _actionAbort; set => _actionAbort = value; }
        public bool IsBroken { get; set; }

        private ActionSky _actionInclude;
        public ActionSky ActionInclude { get => _actionInclude; set => _actionInclude = value; }

        [SerializeField] private float _timeRepair;
        public float TimeRepair { get; set; }
        public bool IsRunBroken { get; set; }
        public bool IsWorking { get; set; }
        public bool IsInclude { get; set; }
        [SerializeField] private int _priceBroken;
        public int PriceBroken { get => _priceBroken; set => _priceBroken = value; }

        protected override void Awake()
        {
            base.Awake();
            _actionAbort = Resources.Load("AbortRepare") as ActionSky;
            _actionBroken = Resources.Load("Broken") as ActionSky;
            _actionInclude = Resources.Load("Include") as ActionSky;
        }
        public override void CreateItemInScene()
        {
            base.CreateItemInScene();
            Include();
            GameManager.Instance.Electronics.Add(IsObjectID, this);
            CheckingForState();
        }
        public override void CreateSkyAction(Vector3 spawnPoint)
        {
            var panelAction = PanelAction.Instance;
            panelAction.DestroyButtons();
            panelAction.transform.position = new Vector3(spawnPoint.x, panelAction.transform.position.y, spawnPoint.z);
            if (!IsRunBroken)
            {
                new CreateSky(panelAction, ActionToSell, ToSell, Price.ToString() + "K");
                new CreateSky(panelAction, ActionMoveItem, MoveItem);
                if (!IsBroken)
                    new CreateSky(panelAction, ActionInclude, SetStateInclude, TextIncludeGnerate());
                if (IsBroken)
                    new CreateSky(panelAction, ActionBroken, Fix, PriceBroken.ToString() + "K");
            }
            else
            {
                new CreateSky(panelAction, ActionAbort, AbortRepare);
            }
        }
        protected override void CheckingForState()
        {
            if (IsBroken && IsInclude || IsBroken && !IsInclude)
                SetState("Broken");
            else if (!IsInclude)
                SetState("Off");
            else
                SetState("Available");
        }
        private void SetState(string checkigResult)
        {
            switch (checkigResult)
            {
                case "Available":
                    _childRenderer.material.color = Color.white;
                    break;
                case "Off":
                    IsWorking = false;
                    _childRenderer.material.color = Color.gray;
                    break;
                case "Broken":
                    _childRenderer.material.color = Color.red;
                    break;
            }
        }
        #region Include
        private string TextIncludeGnerate() => IsInclude ? "(Включино)" : "(Выключино)";
        public void SetStateInclude()
        {
            PanelAction.Instance.DestroyButtons();
            Controller.Instance.SetStateControlleble(ActionInclude.Actions.State);
        }
        public void Include()
        {
            IsInclude = !IsInclude;
            CheckingForState();
            Controller.Instance.SetStateControlleble(ActionAbort.Actions.State);
        }

        #endregion Include

        #region Broken
        public void Broken()
        {
            IsInclude = false;
            IsBroken = true;
            IsWorking = false;
            CheckingForState();
            GameManager.Instance.Broken.Add(IsObjectID, this);
        }

        public void Fix()
        {
            PanelAction.Instance.DestroyButtons();
            Controller.Instance.SetStateControlleble(ActionBroken.Actions.State);
        }
        public void AbortRepare()
        {
            PanelAction.Instance.DestroyButtons();
            Controller.Instance.SetStateControlleble(ActionAbort.Actions.State);
            IsRunBroken = false;
            StopCoroutine("Repair");
        }
        public void StartRepair()
        {
            if (!IsRunBroken)
                StartCoroutine("Repair");
            IsRunBroken = true;
        }
        private IEnumerator Repair()
        {
            yield return new WaitForSeconds(_timeRepair);
            EndRepare();
        }
        public void EndRepare()
        {
            IsRunBroken = false;
            IsBroken = false;
            IsInclude = true;
            GameManager.Instance.Broken.Remove(IsObjectID);
            Controller.Instance.SetStateControlleble(ActionAbort.Actions.State);
            CheckingForState();
        }
        #endregion Broken

        public override void ToSell()
        {
            PanelAction.Instance.DestroyButtons();
            PlacementSystem.Instance.StartRemove(transform.position);
            GameManager.Instance.Electronics.Remove(IsObjectID);
            GameManager.Instance.Broken.Remove(IsObjectID);
        }
    }

}