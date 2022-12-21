using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    public class CursorFollowMouse : MonoBehaviour
    {
        [SerializeField] private Canvas canvas = null;
        private RectTransform canvasRectTransform = null;
        private RectTransform rectTransform = null;


        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
        private void OnEnable() {
            Vector2 startingPosition = rectTransform.anchoredPosition;
        }

        private void Update() {
            if(canvas.renderMode == RenderMode.ScreenSpaceCamera){
                FollowMouseOnCameraSpace();
            }
            else if(canvas.renderMode == RenderMode.ScreenSpaceOverlay){
                FollowMouseOnOverlaySpace();
            }
        }

        private void FollowMouseOnOverlaySpace(){
            rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        }

        private void FollowMouseOnCameraSpace(){
            Vector2 canvasPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, canvas.worldCamera , out canvasPoint);
            rectTransform.anchoredPosition = canvasPoint;
        }
    }
}
