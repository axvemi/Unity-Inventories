using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    public class CursorFollowMouse : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private RectTransform canvasRectTransform;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Vector2 startingPosition = rectTransform.anchoredPosition;
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
            rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        }

        private void FollowMouseOnCameraSpace()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 canvasPoint);
            rectTransform.anchoredPosition = canvasPoint;
        }
    }
}