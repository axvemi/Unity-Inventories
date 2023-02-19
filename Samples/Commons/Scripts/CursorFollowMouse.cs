using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    public class CursorFollowMouse : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private RectTransform _canvasRectTransform;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Vector2 startingPosition = _rectTransform.anchoredPosition;
        }

        private void Update()
        {
            switch (canvas.renderMode)
            {
                case RenderMode.ScreenSpaceCamera:
                    FollowMouseOnCameraSpace();
                    break;
                case RenderMode.ScreenSpaceOverlay:
                    FollowMouseOnOverlaySpace();
                    break;
            }
        }

        private void FollowMouseOnOverlaySpace()
        {
            _rectTransform.anchoredPosition = Input.mousePosition / _canvasRectTransform.localScale.x;
        }

        private void FollowMouseOnCameraSpace()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 canvasPoint);
            _rectTransform.anchoredPosition = canvasPoint;
        }
    }
}