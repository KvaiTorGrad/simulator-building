using ControllerSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace BuildingItemsOnGrid
{
    public class InputBuildManager : SingletonBase<InputBuildManager>
    {
        private Vector3 _lastPosition;
        [SerializeField] private LayerMask _placementLayerMask;
        public event Action OnClicked, OnExit;
        public bool IsBuildActive { get; set; }
        private void Start()
        {
            Controller.Instance.ActionsInput.Building.CreateBuild.performed += build => StartOnClicked();
            Controller.Instance.ActionsInput.Building.ClearBuild.performed += build => EndOnClicked();
        }
        private void StartOnClicked()
        {
            OnClicked?.Invoke();
        }
        private void EndOnClicked()
        {
            OnExit?.Invoke();
        }
        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
        public Vector3 GetSelectMapPosition()
        {
            if (Physics.Raycast(Controller.Instance.Ray(), out RaycastHit hit, 100, _placementLayerMask))
                _lastPosition = hit.point;
            return _lastPosition;
        }
        private void OnDestroy()
        {
            Controller.Instance.ActionsInput.Building.ClearBuild.performed -= build => EndOnClicked();
            Controller.Instance.ActionsInput.Building.CreateBuild.performed -= build => StartOnClicked();
        }
    }
}