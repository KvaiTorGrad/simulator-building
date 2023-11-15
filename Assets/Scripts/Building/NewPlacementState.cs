using BuildingItemsOnGrid.DataBaseObject;
using BuildingItemsOnGrid.ManipulationsObject;
using ItemSetting;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingItemsOnGrid.IBuilding
{
    public class NewPlacementState : IBuildingState
    {
        private int _gameObjectIndex = -1;
        private Grid _grid;
        private Item _selectItem;
        private PreviewSystem _previewSystem;
        private PlacementSystem _placementSystem;
        private ObjectsDataBase _dataBase;
        private GridData _furniturData;
        private GridData _selectData;
        private ObjectPlacer _objectPlacer;
        private SoundFeedback _soundFeedback;
        private Vector3Int _startPosition;
        public NewPlacementState(Grid grid, Vector3Int startPosition, PlacementSystem placementSystem, PreviewSystem previewSystem, ObjectsDataBase dataBase, GridData furniturData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
        {
            _grid = grid;
            _startPosition = startPosition;
            _placementSystem = placementSystem;
            _previewSystem = previewSystem;
            _dataBase = dataBase;
            _furniturData = furniturData;
            _objectPlacer = objectPlacer;
            _soundFeedback = soundFeedback;
            _selectData = null;
            if (_furniturData.CanPlaceObjectAt(_startPosition, Vector2Int.one) == false)
                _selectData = _furniturData;
            if (_selectData == null)
                _soundFeedback.PlaySound(SoundType.WrongPlacement);
            else
                _gameObjectIndex = _selectData.GetRepresentationIndex(_startPosition);
            if (_gameObjectIndex > -1)
            {
                _selectItem = GameManager.Instance.Items.GetValueOrDefault(_gameObjectIndex);
                _gameObjectIndex = _selectItem.ID;
                _previewSystem.StartShowingPlacementPreview(_selectItem.gameObject, _dataBase.objectDatas[_gameObjectIndex].Size);
            }
            else
                throw new System.Exception($"No object with ID {_gameObjectIndex}");
            _selectData.RemoveObjectAt(_startPosition);
        }
        public void EndState()
        {
            _previewSystem.StopShowPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            if (_gameObjectIndex == -1)
                return;
            Vector3 cellPosition = _grid.CellToWorld(gridPosition);
            _previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));

            bool placementValidity = CheckPlacementValidity(gridPosition);
            if (!placementValidity)
            {
                _soundFeedback.PlaySound(SoundType.WrongPlacement);
                return;
            }
            _soundFeedback.PlaySound(SoundType.Place);
            int index = _objectPlacer.NewPlaceObject(_selectItem.gameObject, _grid.CellToWorld(gridPosition));
            _selectData = /*_dataBase.objectDatas[_gameObjectIndex].ID == 0 ? _floorData : */_furniturData;
            _selectData.AddObjectAt(gridPosition, _dataBase.objectDatas[_gameObjectIndex].Size, _dataBase.objectDatas[_gameObjectIndex].ID, index);
            _previewSystem.UpdatePosition(_grid.WorldToCell(gridPosition), false);
            _placementSystem.StopPlacement();
        }
        private bool CheckPlacementValidity(Vector3Int gridPosition) => _furniturData.CanPlaceObjectAt(gridPosition, _dataBase.objectDatas[_gameObjectIndex].Size);
        private bool CheckIfSelectionIsValid(Vector3Int gridPosition) => !_furniturData.CanPlaceObjectAt(gridPosition, Vector2Int.one);

        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
        }

    }
}