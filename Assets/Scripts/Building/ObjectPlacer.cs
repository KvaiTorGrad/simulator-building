using ItemSetting;
using UnityEngine;
namespace BuildingItemsOnGrid.ManipulationsObject
{
    public class ObjectPlacer : MonoBehaviour
    {
        public int PlaceObject(GameObject prefab, Vector3 position)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.position = position;
            var item = newObject.GetComponent<Item>();
            item.CreateItemInScene();
            GameManager.Instance.Items.Add(item.IsObjectID, item);
            return item.IsObjectID;
        }
        public int NewPlaceObject(GameObject prefab, Vector3 position)
        {
            prefab.transform.position = position;
            Item item = prefab.GetComponent<Item>();
            return item.IsObjectID;
        }
        public void RemoveObjectAt(int gameObjectIndex)
        {
            if (GameManager.Instance.Items.ContainsKey(gameObjectIndex) == false || GameManager.Instance.Items[gameObjectIndex] == null)
                return;
            Destroy(GameManager.Instance.Items[gameObjectIndex].gameObject);
            GameManager.Instance.Items.Remove(gameObjectIndex);
        }
    }
}