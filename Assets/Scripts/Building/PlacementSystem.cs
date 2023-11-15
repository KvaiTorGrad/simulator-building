using BuildingItemsOnGrid.DataBaseObject;
using BuildingItemsOnGrid.IBuilding;
using BuildingItemsOnGrid.ManipulationsObject;
using System.Collections.Generic;
using UnityEngine;
namespace BuildingItemsOnGrid
{
    public class PlacementSystem : SingletonBase<PlacementSystem>
    {
        [SerializeField] private InputBuildManager _inputBuildManager;
        [SerializeField] private Grid _grid;
        [SerializeField] private ObjectsDataBase _dataBase;
        [SerializeField] private GameObject _gridVisualization;
        private GridData _furniturData;
        private PreviewSystem _previewSystem;
        private Vector3Int _lastDetectedPosition = Vector3Int.zero;
        private ObjectPlacer _objectPlacer;
        private IBuildingState _buildingState;
        private SoundFeedback _soundFeedback;
        protected override void Awake()
        {
            base.Awake();
            _previewSystem = FindObjectOfType<PreviewSystem>();
            _objectPlacer = FindObjectOfType<ObjectPlacer>();
            _soundFeedback = FindObjectOfType<SoundFeedback>();
        }
        private void Start()
        {
            StopPlacement();
            GameManager.Instance.GridVisualizations.Add(_gridVisualization);
            TryActiveGridVisualization(false);
            _furniturData = new();
        }

        private void Update()
        {
            if (_buildingState == null) return;
            Vector3 mousePos = _inputBuildManager.GetSelectMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePos);
            if (_lastDetectedPosition != gridPosition)
            {
                _buildingState.UpdateState(gridPosition);
                _lastDetectedPosition = gridPosition;
            }
        }
        private void TryActiveGridVisualization(bool isActiveGridVisual)
        {
            foreach (var grid in GameManager.Instance.GridVisualizations)
                grid.SetActive(isActiveGridVisual);
        }
        public void StartPlacement(int ID)
        {
            StopPlacement();
            InputBuildManager.Instance.IsBuildActive = true;
            _soundFeedback.PlaySound(SoundType.Click);
            _buildingState = new PlacementState(ID, _grid, this, _previewSystem, _dataBase, _furniturData, _objectPlacer, _soundFeedback);
            TryActiveGridVisualization(true);
            _inputBuildManager.OnClicked += PlaceStructure;
            _inputBuildManager.OnExit += StopPlacement;
        }
        public void StarTransfer(Vector3 positionObject)
        {
            InputBuildManager.Instance.IsBuildActive = true;
            StopPlacement();
            _soundFeedback.PlaySound(SoundType.Click);
            Vector3Int gridPosition = _grid.WorldToCell(positionObject);
            _buildingState = new NewPlacementState(_grid, gridPosition, this, _previewSystem, _dataBase, _furniturData, _objectPlacer, _soundFeedback);
            TryActiveGridVisualization(true);
            _inputBuildManager.OnClicked += PlaceStructure;
        }
        public void StartRemove(Vector3 positionObject)
        {
            StopPlacement();
            _buildingState = new RemovingState(_grid, _previewSystem, _furniturData, _objectPlacer, _soundFeedback);
            PlaceStructureRemove(positionObject);
            StopPlacement();
        }

        private void PlaceStructure()
        {
            if (_inputBuildManager.IsPointerOverUI()) return;
            Vector3 mousePos = _inputBuildManager.GetSelectMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePos);
            _buildingState.OnAction(gridPosition);
        }
        private void PlaceStructureRemove(Vector3 positionObject)
        {
            Vector3Int gridPosition = _grid.WorldToCell(positionObject);
            _buildingState.OnAction(gridPosition);
        }

        public void StopPlacement()
        {
            if (_buildingState == null) return;
            TryActiveGridVisualization(false);
            _buildingState.EndState();
            _inputBuildManager.OnClicked -= PlaceStructure;
            _inputBuildManager.OnExit -= StopPlacement;
            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;
            InputBuildManager.Instance.IsBuildActive = false;
        }
    }
}