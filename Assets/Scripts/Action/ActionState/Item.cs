using BuildingItemsOnGrid;
using UnityEngine;

namespace ItemSetting
{
    public abstract class Item : MonoBehaviour, IItem, IToSell, IMoveItem
    {

        public int ID;
        public int IsObjectID { get; private set; }
        [SerializeField] private int _price;
        private ActionSky _actionToSell;
        public int Price { get => _price; }
        public ActionSky ActionToSell { get => _actionToSell; set => _actionToSell = value; }
        private ActionSky _actionMoveItem;
        public ActionSky ActionMoveItem { get => _actionMoveItem;}
        [SerializeField] private Transform _faceTarget;
        public Transform FaceTarget { get => _faceTarget; }
        private Collider _collider;
        public Collider Collider { get => _collider; }
        protected Renderer _childRenderer;

        protected virtual void Awake()
        {
            _actionToSell = Resources.Load("ToSell") as ActionSky;
            _actionMoveItem = Resources.Load("MoveItem") as ActionSky;
            _collider = GetComponent<Collider>();
            _childRenderer = transform.GetComponentInChildren<Renderer>();
        }

        public virtual void CreateItemInScene()
        {
            do
                IsObjectID = Random.Range(1, 999999999);
            while (GameManager.Instance.Items.ContainsKey(IsObjectID));
            Collider.isTrigger = false;
            NewPosToFaceTarget();
        }
        protected virtual void NewPosToFaceTarget()
        {
            FaceTarget.transform.position = new Vector3(FaceTarget.transform.position.x, 0.5f, FaceTarget.transform.position.z);
        }

        public virtual void InitActiveItem(bool isActive)
        {
            CheckingForState();
        }
        protected virtual void CheckingForState() { }
        public abstract void CreateSkyAction(Vector3 spawnPoint);

        public virtual void MoveItem()
        {
            PanelAction.Instance.DestroyButtons();
            PlacementSystem.Instance.StarTransfer(transform.position);
        }

        public virtual void ToSell() { }
    }
}

//[SerializeField] private GameObject _gridPanel;
//public GameObject GridPanel { get => _gridPanel; }
//public virtual void AddNewGridAndStartPosToFaceTarget()
//{
//    GameManager.Instance.GridVisualizations.Add(_gridPanel);
//    _gridPanel.SetActive(true);
//}
//        GameManager.Instance.GridVisualizations.Remove(_gridPanel);