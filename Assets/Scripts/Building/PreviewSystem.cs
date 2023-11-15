using UnityEngine;

namespace BuildingItemsOnGrid
{
    public class PreviewSystem : SingletonBase<PreviewSystem>
    {
        [SerializeField] private float _previewYOffset = 0.06f;
        [SerializeField] private GameObject _cellIndicator;
        private GameObject _previewObject;
        [SerializeField] private Material _previewMaterialPrefab;
        private Material _previewMaterialInstance;
        private Renderer _cellIndiactorRenderer;
        private void Start()
        {
            _previewMaterialInstance = new Material(_previewMaterialPrefab);
            _cellIndicator.gameObject.SetActive(false);
            _cellIndiactorRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
        }

        public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
        {
            _previewObject = Instantiate(prefab);
            PreparePrevie(_previewObject);
            PrepareCursor(size);
            _cellIndicator.SetActive(true);
        }

        private void PreparePrevie(GameObject prefab)
        {
            Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                    materials[i] = _previewMaterialInstance;
                renderer.materials = materials;
            }
        }

        private void PrepareCursor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0)
            {
                _cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
                _cellIndiactorRenderer.material.mainTextureScale = size;
            }
        }

        public void StopShowPreview()
        {
            _cellIndicator.SetActive(false);
            if (_previewObject != null)
                Destroy(_previewObject);
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (_previewObject != null)
            {
                MovePreview(position);
                ApllyFeedbackToPreview(validity);
            }
            MoveCursor(position);
            ApllyFeedbackToCursor(validity);
        }

        private void ApllyFeedbackToPreview(bool validity)
        {
            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            _previewMaterialInstance.color = c;
        }
        private void ApllyFeedbackToCursor(bool validity)
        {
            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            _cellIndiactorRenderer.material.color = c;
        }

        private void MoveCursor(Vector3 position)
        {
            _cellIndicator.transform.position = new Vector3(position.x, position.y, position.z);
        }
        private void MovePreview(Vector3 position)
        {
            _previewObject.transform.position = new Vector3(position.x, position.y + _previewYOffset, position.z);
        }
        internal void StartShowingRemovePreview()
        {
            _cellIndicator.SetActive(true);
            PrepareCursor(Vector2Int.one);
            ApllyFeedbackToCursor(false);
        }
    }
}