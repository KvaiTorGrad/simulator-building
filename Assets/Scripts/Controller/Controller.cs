using BuildingItemsOnGrid;
using UnityEngine;
namespace ControllerSystem
{
    public class Controller : SingletonBase<Controller>
    {
        private IControlleble _controlleble;
        public IControlleble Controllerble { get => _controlleble; }
        private ActionInput _action;
        public ActionInput ActionsInput { get => _action; }
        private Transform _useObject;
        private Vector3 _target;

        protected override void Awake()
        {
            base.Awake();
            _action = new ActionInput();
            _action.Enable();
            _action.Player.MouseClick.performed += click => InitClickToObject();
        }
        void Start()
        {
            _controlleble = GameObject.FindGameObjectWithTag("Player").GetComponent<IControlleble>();
        }
        private void Update()
        {
            _controlleble.InitState();
        }
        private void InitClickToObject()
        {
            if (InputBuildManager.Instance.IsBuildActive) return;
            RaycastHit hit;
            if (Physics.Raycast(Ray(), out hit, 100))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("ClickPoint"))
                {

                    _useObject = hit.transform;
                    var action = _useObject.GetComponent<IItem>();
                    action.CreateSkyAction(_useObject.position);
                    _target = action.FaceTarget.position;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    return;
                }
                else
                {
                    PanelAction.Instance.DestroyButtons();
                }
            }
        }
        public Ray Ray() => Camera.main.ScreenPointToRay(_action.Player.Move.ReadValue<Vector2>());
        public void SetStateControlleble(State state)
        {
            if (InputBuildManager.Instance.IsBuildActive) return;
            _controlleble.SetTartgetToAgent(_target);
            _controlleble.SetState(state, _useObject);
        }
        private void OnDestroy()
        {
            _action.Player.MouseClick.performed -= click => InitClickToObject();
        }
    }
}