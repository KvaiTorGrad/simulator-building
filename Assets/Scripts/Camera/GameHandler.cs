using ControllerSystem;
using UnityEngine;
namespace CameraController.Handler
{
    public class GameHandler : MonoBehaviour
    {
        private CameraFollow _cameraFollow;
        private Controller _controller;
        private float edgeSize = 10f;
        [SerializeField, Range(10, 100)] private int _moveAmount;
        private Vector3 _cameraFollowPosition;
        private bool _isEdgeScrolling;
        public bool IsEdgeScrolling { get => _isEdgeScrolling; set => _isEdgeScrolling = value; }
        private void Awake()
        {
            _cameraFollow = FindObjectOfType<CameraFollow>();
            _controller = FindObjectOfType<Controller>();
        }
        private void Start()
        {
            _cameraFollowPosition = _cameraFollow.transform.position;
            _isEdgeScrolling = true;
            _cameraFollow.Setup(() => _cameraFollowPosition);
        }

        private void Update()
        {
            if (IsEdgeScrolling)
            {
                if (_controller.ActionsInput.Player.Move.ReadValue<Vector2>().x < Screen.width - edgeSize)
                    _cameraFollowPosition.x += _moveAmount * Time.deltaTime;
                if (_controller.ActionsInput.Player.Move.ReadValue<Vector2>().x > edgeSize)
                    _cameraFollowPosition.x -= _moveAmount * Time.deltaTime;
                if (_controller.ActionsInput.Player.Move.ReadValue<Vector2>().y < Screen.height - edgeSize)
                    _cameraFollowPosition.z += _moveAmount * Time.deltaTime;
                if (_controller.ActionsInput.Player.Move.ReadValue<Vector2>().y > edgeSize)
                    _cameraFollowPosition.z -= _moveAmount * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                IsEdgeScrolling = !IsEdgeScrolling;
            }
        }
    }
}