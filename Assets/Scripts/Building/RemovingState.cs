using BuildingItemsOnGrid.ManipulationsObject;
using UnityEngine;
namespace BuildingItemsOnGrid.IBuilding
{
    public class RemovingState : IBuildingState
    {
        private int _gameObjectIndex = -1;
        private Grid _grid;
        private PreviewSystem _previewSystem;
        private GridData _furniturData;
        private ObjectPlacer _objectPlacer;
        private SoundFeedback _soundFeedback;
        public RemovingState(Grid grid, PreviewSystem previewSystem, GridData furniturData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
        {
            _grid = grid;
            _previewSystem = previewSystem;
            _furniturData = furniturData;
            _objectPlacer = objectPlacer;
            _soundFeedback = soundFeedback;
            previewSystem.StartShowingRemovePreview();
        }

        public void EndState()
        {
            _previewSystem.StopShowPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            GridData selectData = null;
            if (_furniturData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
                selectData = _furniturData;
            if (selectData == null)
                _soundFeedback.PlaySound(SoundType.WrongPlacement);
            else
            {
                _gameObjectIndex = selectData.GetRepresentationIndex(gridPosition);
                if (_gameObjectIndex == -1)
                    return;
                selectData.RemoveObjectAt(gridPosition);
                _objectPlacer.RemoveObjectAt(_gameObjectIndex);
                _soundFeedback.PlaySound(SoundType.Remove);
            }
            Vector3 cellPosition = _grid.CellToWorld(gridPosition);
            _previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
        }

        private bool CheckIfSelectionIsValid(Vector3Int gridPosition) => !_furniturData.CanPlaceObjectAt(gridPosition, Vector2Int.one);
        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIfSelectionIsValid(gridPosition);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), validity);
        }
    }
}