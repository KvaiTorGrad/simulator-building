using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingItemsOnGrid
{
    public class GridData
    {
        private Dictionary<Vector3Int, PlacementData> _placeObjects = new();

        public void AddObjectAt(Vector3Int gridPostion, Vector2Int objectSize, int ID, int placedObjectIndex)
        {
            List<Vector3Int> positionToOccupy = CalculatePosition(gridPostion, objectSize);
            PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
            foreach (var pos in positionToOccupy)
            {
                if (_placeObjects.ContainsKey(pos))
                    throw new Exception($"Dictionary already contains this cell position {pos}");
                _placeObjects[pos] = data;
            }
        }

        private List<Vector3Int> CalculatePosition(Vector3Int gridPostion, Vector2Int objectSize)
        {
            List<Vector3Int> returnVal = new();
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    returnVal.Add(gridPostion + new Vector3Int(x, 0, y));
                }
            }
            return returnVal;
        }

        public bool CanPlaceObjectAt(Vector3Int gridPostion, Vector2Int objectSize)
        {
            List<Vector3Int> positionToOccupy = CalculatePosition(gridPostion, objectSize);
            foreach (var pos in positionToOccupy)
            {
                if (_placeObjects.ContainsKey(pos))
                    return false;
            }
            return true;
        }

        public int GetRepresentationIndex(Vector3Int gridPosition)
        {
            if (_placeObjects.ContainsKey(gridPosition) == false)
                return -1;
            var placeIndex = _placeObjects[gridPosition].PlacedObjectIndex;
            if (GameManager.Instance.Items.ContainsKey(placeIndex) == false)
                return -1;
            return placeIndex;
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            foreach (var pos in _placeObjects[gridPosition].occupiedPosition)
            {
                _placeObjects.Remove(pos);
            }
        }
    }

    public class PlacementData
    {
        public List<Vector3Int> occupiedPosition;
        public int ID { get; private set; }
        public int PlacedObjectIndex { get; private set; }
        public PlacementData(List<Vector3Int> occupiedPosition, int iD, int placedObjectIndex)
        {
            this.occupiedPosition = occupiedPosition;
            ID = iD;
            PlacedObjectIndex = placedObjectIndex;
        }
    }
}