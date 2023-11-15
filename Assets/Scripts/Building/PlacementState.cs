using BuildingItemsOnGrid.DataBaseObject;
using BuildingItemsOnGrid.ManipulationsObject;
using UnityEngine;
namespace BuildingItemsOnGrid.IBuilding
{
    public class PlacementState : IBuildingState
    {
        private int _selectedObjectIndex = -1;
        private int _id;
        private Grid _grid;
        private PlacementSystem _placementSystem;
        private PreviewSystem _previewSystem;
        private ObjectsDataBase _dataBase;
        private GridData _furniturData;
        private ObjectPlacer _objectPlacer;
        private SoundFeedback _soundFeedback;
        private Vector2Int _size;
        public PlacementState(int id, Grid grid, PlacementSystem placementSystem, PreviewSystem previewSystem, ObjectsDataBase dataBase, GridData firnitureData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
        {
            _id = id;
            _grid = grid;
            _placementSystem = placementSystem;
            _previewSystem = previewSystem;
            _dataBase = dataBase;
            _furniturData = firnitureData;
            _objectPlacer = objectPlacer;
            _soundFeedback = soundFeedback;
            _selectedObjectIndex = _dataBase.objectDatas.FindIndex(data => data.ID == _id);
            _size = _dataBase.objectDatas[_selectedObjectIndex].Size;
            if (_selectedObjectIndex > -1)
                _previewSystem.StartShowingPlacementPreview(_dataBase.objectDatas[_selectedObjectIndex].Prefab, _size);
            else
                throw new System.Exception($"No object with ID {id}");
        }

        public void EndState()
        {
            _previewSystem.StopShowPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
            if (!placementValidity)
            {
                _soundFeedback.PlaySound(SoundType.WrongPlacement);
                return;
            }
            _soundFeedback.PlaySound(SoundType.Place);
            int index = _objectPlacer.PlaceObject(_dataBase.objectDatas[_selectedObjectIndex].Prefab, _grid.CellToWorld(gridPosition));
            _furniturData.AddObjectAt(gridPosition, _size, _dataBase.objectDatas[_selectedObjectIndex].ID, index);
            _previewSystem.UpdatePosition(_grid.WorldToCell(gridPosition), false);
            _placementSystem.StopPlacement();
        }
        private bool CheckPlacementValidity(Vector3Int gridPosition, int selectObjectIndex) => _furniturData.CanPlaceObjectAt(gridPosition, _dataBase.objectDatas[selectObjectIndex].Size);
        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
        }
    }
}